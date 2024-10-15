using Newtonsoft.Json;
using RCore.Common;
using System;
using UnityEngine;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;

namespace RCore.Data.JObject
{
	[Serializable]
	public abstract class JObjectCollection
	{
		[JsonIgnore] public string key;

		public JObjectCollection() { }
		public JObjectCollection(string pKey)
		{
			key = pKey;
		}
		/// <summary>
		/// Save data 
		/// </summary>
		/// <param name="minimizeSize">True: Minimize json data by Json.Net. False: serialize by JsonUtility; it is recommended due to its better performance.</param>
		public virtual void Save(int utcNowTimestamp, bool minimizeSize = false)
		{
			PlayerPrefs.SetString(key, ToJson(minimizeSize));
		}
		public virtual bool Load()
		{
			var json = PlayerPrefs.GetString(key);
			return Load(json);
		}
		public bool Load(string json)
		{
			if (!string.IsNullOrEmpty(json))
			{
				try
				{
					JsonUtility.FromJsonOverwrite(json, this);
					return true;
				}
				catch (Exception ex)
				{
					Debug.LogError(ex);
				}
			}
			return false;
		}
		public virtual T Load<T>(T defaultVal)
		{
			var json = PlayerPrefs.GetString(key);
			try
			{
				var data = JsonUtility.FromJson<T>(json);
				return data;
			}
			catch (Exception ex)
			{
				Debug.LogError(ex);
				return defaultVal;
			}
		}
		public void Delete() => PlayerPrefs.DeleteKey(key);
		/// <summary>
		/// Get Json
		/// </summary>
		/// <param name="minimizeSize">True: Minimize json data by Json.Net. False: serialize by JsonUtility; it is recommended due to its better performance.</param>
		/// <returns></returns>
		public string ToJson(bool minimizeSize = false)
		{
			if (!minimizeSize)
				return JsonUtility.ToJson(this);
			var serializerSettings = new JsonSerializerSettings
			{
				DefaultValueHandling = DefaultValueHandling.Ignore,
				NullValueHandling = NullValueHandling.Ignore,
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore
			};
			return JsonConvert.SerializeObject(this, serializerSettings);
		}
		public abstract void OnPause(bool pause, int utcNowTimestamp, int offlineSeconds);
		public abstract void OnPostLoad(int utcNowTimestamp, int offlineSeconds);
		public abstract void OnUpdate(float deltaTime);
	}
}