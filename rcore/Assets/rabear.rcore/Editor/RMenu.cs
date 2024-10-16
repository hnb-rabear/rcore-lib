/***
 * Author RadBear - Nguyen Ba Hung - nbhung71711@gmail.com
 **/

using RCore.Common;
using RCore.Editor;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace RCore.Editor
{
	public class RMenu : UnityEditor.Editor
	{
		private const int INDEX = 0;

		private const string ALT = "&";
		private const string SHIFT = "#";
		private const string CTRL = "%";

		[MenuItem("RCore/Open Env Settings %_&_j", priority = INDEX)]
		private static void OpenEnvSettings()
		{
			Selection.activeObject = EnvSetting.Instance;
		}
		
		[MenuItem("RCore/Asset Database/Save Assets " + SHIFT + "_1", priority = INDEX + 1)]
		private static void SaveAssets()
		{
			var objs = Selection.objects;
			if (objs != null)
				foreach (var obj in objs)
					EditorUtility.SetDirty(obj);

			AssetDatabase.SaveAssets();
		}

		[MenuItem("RCore/Asset Database/Refresh Prefabs in folder", priority = INDEX +  2)]
		private static void RefreshPrefabs()
		{
			RefreshAssets("t:GameObject");
		}

		[MenuItem("RCore/Asset Database/Refresh ScriptableObjects in folder", priority = INDEX +  3)]
		private static void RefreshScriptableObjects()
		{
			RefreshAssets("t:ScriptableObject");
		}

		[MenuItem("RCore/Asset Database/Refresh Assets in folder", priority = INDEX +  4)]
		private static void RefreshAll()
		{
			RefreshAssets("t:GameObject t:ScriptableObject");
		}

		private static void RefreshAssets(string filter)
		{
			string folderPath = EditorHelper.OpenFolderPanel();
			folderPath = EditorHelper.FormatPathToUnityPath(folderPath);
			var assetGUIDs = AssetDatabase.FindAssets(filter, new[] { folderPath });
			foreach (string guid in assetGUIDs)
			{
				var path = AssetDatabase.GUIDToAssetPath(guid);
				var asset = AssetDatabase.LoadAssetAtPath<Object>(path);
				if (asset != null)
					EditorUtility.SetDirty(asset);
			}
			AssetDatabase.SaveAssets();
		}

		//==========================================================

		[MenuItem("RCore/Group Scene Objects " + ALT + "_F1", priority = INDEX + 21)]
		private static void GroupSceneObjects()
		{
			var objs = Selection.gameObjects;
			if (objs.Length > 1)
			{
				var group = new GameObject();
				for (int i = 0; i < objs.Length; i++)
				{
					objs[i].transform.SetParent(group.transform);
				}
				Selection.activeObject = group;
			}
		}

		[MenuItem("RCore/Ungroup Scene Objects " + ALT + "_F2", priority = INDEX + 22)]
		private static void UngroupSceneObjects()
		{
			var objs = Selection.gameObjects;
			if (objs.Length > 1)
			{
				for (int i = 0; i < objs.Length; i++)
					objs[i].transform.SetParent(null);
			}
		}

		//==========================================================

		[MenuItem("RCore/Clear PlayerPrefs", priority = INDEX + 61)]
		private static void ClearPlayerPrefs()
		{
			if (EditorHelper.ConfirmPopup("Clear PlayerPrefs"))
				PlayerPrefs.DeleteAll();
		}
		
		//==========================================================
		
		[MenuItem("RCore/Explorer/Open DataPath Folder", false, INDEX + 81)]
		private static void OpenDataPathFolder()
		{
			string path = Application.dataPath;
			var psi = new ProcessStartInfo(path);
			Process.Start(psi);
		}
		
		[MenuItem("RCore/Explorer/Open StreamingAssets Folder", false, INDEX + 82)]
		private static void OpenStreamingAssetsFolder()
		{
			string path = Application.streamingAssetsPath;
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
				AssetDatabase.Refresh();
			}
			var psi = new ProcessStartInfo(path);
			Process.Start(psi);
		}
		
		[MenuItem("RCore/Explorer/Open PersistentData Folder", false, INDEX + 83)]
		private static void OpenPersistentDataFolder()
		{
			string path = Application.persistentDataPath;
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			var psi = new ProcessStartInfo(path);
			Process.Start(psi);
		}
		
		[MenuItem("RCore/Explorer/Open UnityEditor Folder", false, INDEX + 84)]
		private static void OpenUnityEditorFolder()
		{
			string path = EditorApplication.applicationPath.Substring(0, EditorApplication.applicationPath.LastIndexOf("/"));
			var psi = new ProcessStartInfo(path);
			Process.Start(psi);
		}
	}
}