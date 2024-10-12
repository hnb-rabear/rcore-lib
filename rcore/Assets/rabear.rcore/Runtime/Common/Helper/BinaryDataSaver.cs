using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace RCore.Common
{
	public static class BinaryDataSaver
	{
		private const string EXTENSION = ".sav";

		[Serializable]
		public class DataWrap
		{
			public string document;
			public DataWrap() { }
			public DataWrap(string pDocument)
			{
				document = pDocument;
			}
		}

		public static void Save(string data, string pFileName)
		{
			var temp = new DataWrap(data);
			var bf = new BinaryFormatter();
			var folder = Application.persistentDataPath + Path.DirectorySeparatorChar;
#if UNITY_EDITOR
			folder = Application.dataPath.Replace("Assets", "Saves") + Path.DirectorySeparatorChar;
#endif
			var file = File.Create(folder + pFileName + EXTENSION);
			bf.Serialize(file, temp);
			file.Close();
		}

		public static string Load(string pFileName)
		{
			var folder = Application.persistentDataPath + Path.DirectorySeparatorChar;
#if UNITY_EDITOR
			folder = Application.dataPath.Replace("Assets", "Saves") + Path.DirectorySeparatorChar;
#endif
			if (File.Exists(folder + pFileName + EXTENSION))
			{
				var bf = new BinaryFormatter();
				var file = File.Open(folder + pFileName + EXTENSION, FileMode.Open);
				var output = (DataWrap)bf.Deserialize(file);
				file.Close();
				return output.document;
			}
			return "";
		}

		public static void Delete(string pFileName)
		{
			var folder = Application.persistentDataPath + Path.DirectorySeparatorChar;
#if UNITY_EDITOR
			folder = Application.dataPath.Replace("Assets", "Saves") + Path.DirectorySeparatorChar;
#endif
			File.Delete(folder + pFileName + EXTENSION);
		}
	}
}