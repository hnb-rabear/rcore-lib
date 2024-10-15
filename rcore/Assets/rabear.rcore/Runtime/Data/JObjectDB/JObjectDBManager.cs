using System.Collections.Generic;
using UnityEngine;
using RCore.Common;

namespace RCore.Data.JObject
{
	public abstract class JObjectDBManager : MonoBehaviour
	{
		[SerializeField, Range(1, 10)] private int m_saveDelay = 3;
		[SerializeField] private bool m_enabledSave = true;

		protected List<JObjectCollection> m_collections = new List<JObjectCollection>();
		protected List<JObjectController> m_controllers = new List<JObjectController>();
	
		public UserSessionData userSessionData;
		
		protected bool m_initialized;
		private float m_saveCountdown;
		private float m_saveDelayCustom;

		public bool Initialzied => m_initialized;

		private void Update()
		{
			if (!m_initialized)
				return;
			
			foreach (var collection in m_collections)
				collection.OnUpdate(Time.deltaTime);
			foreach (var controller in m_controllers)
				controller.OnUpdate(Time.deltaTime);

			//Save with a delay to prevent too many save calls in a short period of time
			if (m_saveCountdown > 0)
			{
				m_saveCountdown -= Time.deltaTime;
				if (m_saveCountdown <= 0)
					Save(true);
			}
		}

		private void OnApplicationPause(bool pause)
		{
			if (!m_initialized)
				return;

			int utcNowTimestamp = TimeHelper.GetUtcNowTimestamp();
			int offlineSeconds = 0;
			if (!pause)
				offlineSeconds = GetOfflineSeconds();
			foreach (var collection in m_collections)
				collection.OnPause(pause, utcNowTimestamp, offlineSeconds);
			foreach (var controller in m_controllers)
				controller.OnPause(pause, utcNowTimestamp, offlineSeconds);
		}

		//============================================================================
		// Public / Internal
		//============================================================================

		/// <summary>
		/// Override this method and create DB Collections in here, then call Init
		/// </summary>
		public abstract void Load();
		
		public virtual void Init()
		{
			if (m_initialized)
				return;

			userSessionData = CreateCollection<UserSessionData>("UserSessionData");

			Load();
			PostLoad();
			m_initialized = true;
		}

		public virtual void Save(bool now = false, float saveDelayCustom = 0)
		{
			if (!m_enabledSave)
				return;
			
			if (now)
			{
				int utcNowTimestamp = TimeHelper.GetUtcNowTimestamp();
				foreach (var collection in m_collections)
					collection.Save(utcNowTimestamp);
				m_saveDelayCustom = 0; // Reset save delay custom
				return;
			}
			
			m_saveCountdown = m_saveDelay;
			if (saveDelayCustom > 0)
			{
				if (m_saveDelayCustom <= 0)
					m_saveDelayCustom = saveDelayCustom;
				else if (m_saveDelayCustom > saveDelayCustom)
					m_saveDelayCustom = saveDelayCustom;
				if (m_saveCountdown > m_saveDelayCustom)
					m_saveCountdown = m_saveDelayCustom;
			}
		}

		public void Import(string data)
		{
			if (!m_enabledSave)
				return;

			m_collections.ImportData(data);
			foreach (var collection in m_collections)
				collection.Load();
			PostLoad();
		}

		public void EnableSave(bool value)
		{
			m_enabledSave = value;
		}

		public int GetOfflineSeconds()
		{
			int offlineSeconds = 0;
			if (userSessionData.lastActive > 0)
			{
				int utcNowTimestamp = TimeHelper.GetUtcNowTimestamp();
				offlineSeconds = utcNowTimestamp - userSessionData.lastActive;
			}
			return offlineSeconds;
		}

		//============================================================================
		// Private / Protected
		//============================================================================

		protected T CreateCollection<T>(string key) where T : JObjectCollection, new()
		{
			var newCollection = JObjectDB.CreateCollection<T>(key);
			if (newCollection != null)
				m_collections.Add(newCollection);
			return newCollection;
		}
		
		protected T CreateController<T>() where T : JObjectController, new()
		{
			var newController = new T();
			newController.manager = this;
			m_controllers.Add(newController);
			return newController;
		}

		private void PostLoad()
		{
			int offlineSeconds = GetOfflineSeconds();
			var utcNowTimestamp = TimeHelper.GetUtcNowTimestamp();
			foreach (var collection in m_collections)
				collection.OnPostLoad(utcNowTimestamp, offlineSeconds);
			foreach (var controller in m_controllers)
				controller.OnPostLoad(utcNowTimestamp, offlineSeconds);
		}
	}
}