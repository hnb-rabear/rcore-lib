using System;
using System.Collections.Generic;
using UnityEngine;
using RCore.Common;

namespace RCore.Data.JObject
{
	public abstract class JObjectDBManager : MonoBehaviour
	{
		[SerializeField, Range(1, 10)] private int m_saveDelay = 3;
		[SerializeField] private bool m_enabledSave = true;
		[SerializeField] private bool m_saveOnPause = true;
		[SerializeField] private bool m_saveOnQuit = true;

		protected List<JObjectCollection> m_collections = new List<JObjectCollection>();
		protected List<IJObjectController> m_controllers = new List<IJObjectController>();
	
		public UserSessionData userSessionData;
		public UserSessionController userSessionController;
		
		protected bool m_initialized;
		private float m_saveCountdown;
		private float m_saveDelayCustom;
		private float m_lastSave;

		public bool Initialzied => m_initialized;

		private void Update()
		{
			if (!m_initialized)
				return;
			
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
			foreach (var controller in m_controllers)
				controller.OnPause(pause, utcNowTimestamp, offlineSeconds);
			if (pause && m_saveOnPause)
				Save(true);
		}

		private void OnApplicationQuit()
		{
			if (m_saveOnQuit)
				Save(true);
		}

		//============================================================================
		// Public / Internal
		//============================================================================
		
		/// <summary>
		/// Initialize DB Manager
		/// </summary>
		public virtual void Init()
		{
			if (m_initialized)
				return;

			userSessionData = CreateCollection<UserSessionData>("UserSessionData");
			userSessionController = CreateController<UserSessionController, JObjectDBManager>();
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
				// Do not allow multiple Save calls within a short period of time.
				if (Time.unscaledTime - m_lastSave < 0.2f)
					return;
				int utcNowTimestamp = TimeHelper.GetUtcNowTimestamp();
				foreach (var collection in m_collections)
					collection.Save(utcNowTimestamp);
				m_saveDelayCustom = 0; // Reset save delay custom
				m_lastSave = Time.unscaledTime;
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

		public virtual void Import(string data)
		{
			if (!m_enabledSave)
				return;

			m_collections.ImportData(data);
			foreach (var collection in m_collections)
				collection.Load();
			PostLoad();
		}

		public virtual void EnableSave(bool value)
		{
			m_enabledSave = value;
		}

		public virtual int GetOfflineSeconds()
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

		/// <summary>
		/// Override this method then create DB Collections and Controller in here
		/// </summary>
		protected abstract void Load();
		
		protected T CreateCollection<T>(string key) where T : JObjectCollection, new()
		{
			var newCollection = JObjectDB.CreateCollection<T>(key);
			if (newCollection != null)
				m_collections.Add(newCollection);
			return newCollection;
		}
		
		protected T CreateController<T, M>() where T : JObjectController<M> where M : JObjectDBManager
		{
			var newController = Activator.CreateInstance<T>();
			newController.manager = this as M;
			
			m_controllers.Add(newController);
			return newController;
		}

		protected void PostLoad()
		{
			int offlineSeconds = GetOfflineSeconds();
			var utcNowTimestamp = TimeHelper.GetUtcNowTimestamp();
			foreach (var controller in m_controllers)
				controller.OnPostLoad(utcNowTimestamp, offlineSeconds);
		}
	}
}