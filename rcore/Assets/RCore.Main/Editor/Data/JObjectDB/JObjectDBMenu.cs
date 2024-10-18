using RCore.Data.JObject;
using UnityEditor;
using UnityEngine;

namespace RCore.Editor.Data.JObject
{
	public static class JObjectDBMenu
	{
		private const int INDEX = 120;
		
		[MenuItem("RCore/JObjectDB/Open JObjectDB Window %_#_'", priority = INDEX + 1)]
		private static void OpenDataWindow()
		{
			var window = EditorWindow.GetWindow<JObjectDBWindow>("JObjectDB", true);
			window.Show();
		}

		[MenuItem("RCore/JObjectDB/Clear", priority = INDEX + 2)]
		private static void ClearSaveData()
		{
			if (EditorHelper.ConfirmPopup())
				JObjectDB.DeleteAll();
		}

		[MenuItem("RCore/JObjectDB/Backup", priority = INDEX + 3)]
		private static void BackUpData()
		{
			string path = EditorUtility.SaveFilePanelInProject("Save Backup", "PlayerData_" + System.DateTime.Now.ToString().Replace("/", "_").Replace(":", "_")
				+ ".txt", "txt", "Please enter a file name to save!");
			if (!string.IsNullOrEmpty(path))
				JObjectDB.Backup(path);
		}

		[MenuItem("RCore/JObjectDB/Restore", priority = INDEX + 4)]
		private static void RestoreData()
		{
			string path = EditorUtility.OpenFilePanel("Select Backup Data File", Application.dataPath, "txt");
			if (!string.IsNullOrEmpty(path))
				JObjectDB.Restore(path);
		}

		[MenuItem("RCore/JObjectDB/Copy All", priority = INDEX + 5)]
		private static void LogData()
		{
			JObjectDB.CopyAllData();
		}
	}
}