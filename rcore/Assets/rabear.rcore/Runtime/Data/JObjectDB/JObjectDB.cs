using RCore.Common;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace RCore.Data.JObject
{
	[Serializable]
	public class KeyValueSS : IComparable<KeyValueSS>
	{
		[SerializeField] private string k;
		[SerializeField] private string v;
		public string Key { get => k; set => k = value; }
		public string Value { get => v; set => v = value; }
		public KeyValueSS(string pKey, string pValue)
		{
			k = pKey;
			v = pValue;
		}
		public int CompareTo(KeyValueSS other)
		{
			return String.Compare(k, other.k, StringComparison.Ordinal);
		}
	}
	
	public static class JObjectDB
	{
		/// <summary>
		/// Used to save Key or Data Saver, which lately used for indexing data list
		/// </summary>
		private static readonly string COLLECTIONS = "JObjectDB";
		
		public static Dictionary<string, JObjectCollection> collections = new Dictionary<string, JObjectCollection>();

		public static JObjectCollection GetCollection(string key)
		{
			if (collections.TryGetValue(key, out var collection))
				return collection;
			return null;
		}
		
		public static T CreateCollection<T>(string key) where T : JObjectCollection, new()
		{
			if (collections.ContainsKey(key))
			{
				Debug.LogError($"Collection {key} Existed");
				return null;
			}

			var collection = new T();
			collection.key = key;
			collection.Load();

			SaveCollectionKey(key);
			collections.Add(key, collection);

			return collection;
		}
		
		private static void SaveCollectionKey(string key)
		{
			string keysStr = PlayerPrefs.GetString(COLLECTIONS);
			string[] keys = keysStr.Split(':');
			for (int i = 0; i < keys.Length; i++)
				if (keys[i] == key)
					return;

			if (keys.Length == 0)
				keysStr += key;
			else
				keysStr += ":" + key;

			PlayerPrefs.SetString(COLLECTIONS, keysStr);
		}
		
		public static string[] GetCollectionKeys()
		{
			if (collections.Count == 0)
			{
				string keysStr = PlayerPrefs.GetString(COLLECTIONS);
				if (string.IsNullOrEmpty(keysStr))
					return Array.Empty<string>();

				string[] keys = keysStr.Split(':');
				return keys;
			}
			else
			{
				var keys = new string[collections.Count];
				int i = 0;
				foreach (var pair in collections)
				{
					keys[i] = pair.Key;
					i++;
				}
				return keys;
			}
		}
		
		/// <summary>
		/// Get data from all collections
		/// </summary>
		public static List<KeyValueSS> GetAllData()
		{
			if (collections.Count == 0)
			{
				var keys = GetCollectionKeys();
				var list = new List<KeyValueSS>();
				foreach (string key in keys)
				{
					var data = PlayerPrefs.GetString(key);
					if (!string.IsNullOrEmpty(data))
						list.Add(new KeyValueSS(key, data));
				}
				return list;
			}
			else
			{
				var list = new List<KeyValueSS>();
				foreach (var pair in collections)
				{
					var data = pair.Value.ToJson();
					list.Add(new KeyValueSS(pair.Key, data));
				}
				return list;
			}
		}

		public static void DeleteAllData()
		{
			if (Application.isPlaying)
			{
				Debug.LogError("Could not Delete Data when Playing!");
				return;
			}

			var saverKeys = GetCollectionKeys();
			for (int i = 0; i < saverKeys.Length; i++)
				PlayerPrefs.DeleteKey(saverKeys[i]);
		}

		public static void ImportData(string jsonData)
		{
			var collectionsJson = JsonHelper.ToList<KeyValueSS>(jsonData);
			foreach (var keyValue in collectionsJson)
			{
				PlayerPrefs.SetString(keyValue.Key, keyValue.Value);
				var collection = GetCollection(keyValue.Key);
				collection?.Load(keyValue.Value);
#if UNITY_EDITOR
				Debug.Log($"Import {keyValue.Key}\n{keyValue.Value}");
#endif
			}
			PlayerPrefs.Save();
		}

		public static void LogData()
		{
			Debug.Log(JsonHelper.ToJson(GetAllData()));
		}

		public static void BackupData(string customFileName = null)
		{
			var time = DateTime.Now;
			string identifier = Application.identifier;
			var idParts = identifier.Split('.');
			string path;
			if (string.IsNullOrEmpty(customFileName))
			{
				string fileName = string.IsNullOrEmpty(identifier) ? "" : $"{idParts[idParts.Length - 1]}_";
				fileName += $"{time.Year % 100}{time.Month:00}{time.Day:00}_{time.Hour:00}h{time.Minute:00}";
				path = GetFilePath(fileName);
			}
			else
				path = GetFilePath(customFileName);
			string jsonData = JsonHelper.ToJson(GetAllData());
			File.WriteAllText(path, jsonData);
#if UNITY_EDITOR
			Debug.Log($"Backup data at path {path}");
#endif
		}

		public static void RestoreData(string filePath)
		{
			using (var sw = new StreamReader(filePath))
			{
				var content = sw.ReadToEnd();
				if (!string.IsNullOrEmpty(content))
					ImportData(content);
			}
		}

		/// <summary>
		/// Discard all changes, back to last data save
		/// </summary>
		public static void Reload()
		{
			foreach (var pair in collections)
				pair.Value.Load();
		}

		public static void Save(int utcNowTimestamp)
		{
			foreach (var pair in collections)
				pair.Value.Save(utcNowTimestamp);
		}
		
		private static string GetFilePath(string fileName)
		{
#if UNITY_EDITOR
			return Path.Combine(Application.dataPath.Replace("Assets", "Saves"), fileName + ".json");
#endif
			return Path.Combine(Application.persistentDataPath, fileName + ".json");
		}
	}

	public static class JObjectDBHelper
	{
		public static List<KeyValueSS> ToListKeyValue(this List<JObjectCollection> collections)
		{
			var list = new List<KeyValueSS>();
			foreach (var pair in collections)
			{
				var data = pair.ToJson();
				list.Add(new KeyValueSS(pair.key, data));
			}
			return list;
		}
		public static string ToJson(this List<JObjectCollection> collections)
		{
			var list = collections.ToListKeyValue();
			return JsonHelper.ToJson(list);
		}
		public static void ImportData(this List<JObjectCollection> collections, string jsonData)
		{
			var keyValuePairs = JsonHelper.ToList<KeyValueSS>(jsonData);
			if (keyValuePairs != null)
				foreach (var pair in keyValuePairs)
					PlayerPrefs.SetString(pair.Key, pair.Value);
		}
	}
}