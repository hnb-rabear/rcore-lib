/***
 * Author RadBear - Nguyen Ba Hung - nbhung71711@gmail.com
 **/

using UnityEditor;
using UnityEngine;
using RCore.Editor;

namespace RCore.Data.KeyValue.Editor
{
	public static class KeyValueDBMenu
	{
		private const int INDEX = 120;
		
		[MenuItem("RCore/KeyValueDB/Open KeyValueDB Window %_#_'", priority = INDEX + 1)]
		private static void OpenDataWindow()
		{
			var window = EditorWindow.GetWindow<KeyValueDBWindow>("KeyValueDB", true);
			window.Show();
		}

		[MenuItem("RCore/KeyValueDB/Clear", priority = INDEX + 2)]
		private static void ClearSaveData()
		{
			if (EditorHelper.ConfirmPopup())
				KeyValueDB.DeleteAll();
		}

		[MenuItem("RCore/KeyValueDB/Backup", priority = INDEX + 3)]
		private static void BackUpData()
		{
			string path = EditorUtility.SaveFilePanelInProject("Save Backup", "PlayerData_" + System.DateTime.Now.ToString().Replace("/", "_").Replace(":", "_")
				+ ".txt", "txt", "Please enter a file name to save!");
			if (!string.IsNullOrEmpty(path))
				KeyValueDB.BackupData(path);
		}

		[MenuItem("RCore/KeyValueDB/Restore", priority = INDEX + 4)]
		private static void RestoreData()
		{
			string path = EditorUtility.OpenFilePanel("Select Backup Data File", Application.dataPath, "txt");
			if (!string.IsNullOrEmpty(path))
				KeyValueDB.RestoreData(path);
		}

		[MenuItem("RCore/KeyValueDB/Log", priority = INDEX + 5)]
		private static void LogData()
		{
			KeyValueDB.LogData();
		}
	}
}