/***
 * Author RadBear - Nguyen Ba Hung - nbhung71711@gmail.com
 **/

using UnityEditor;
using UnityEngine;
using RCore.Common.Editor;

namespace RCore.Framework.Data.Editor
{
	public static class DataMenuTools
	{
		[MenuItem("RCore/Player Data/Clear PlayerPrefs", priority = 1)]
		private static void ClearPlayerPrefs()
		{
			if (EditorHelper.ConfirmPopup("Clear PlayerPrefs"))
				PlayerPrefs.DeleteAll();
		}

		[MenuItem("RCore/Player Data/Open Player Data Window %_#_'", priority = 101)]
		private static void OpenDataWindow()
		{
			var window = EditorWindow.GetWindow<DataWindow>("Player Data", true);
			window.Show();
		}

		[MenuItem("RCore/Player Data/Clear", priority = 102)]
		private static void ClearSaveData()
		{
			if (EditorHelper.ConfirmPopup())
				DataSaverContainer.DeleteAll();
		}

		[MenuItem("RCore/Player Data/Backup", priority = 103)]
		private static void BackUpData()
		{
			string path = EditorUtility.SaveFilePanelInProject("Save Backup", "PlayerData_" + System.DateTime.Now.ToString().Replace("/", "_").Replace(":", "_")
				+ ".txt", "txt", "Please enter a file name to save!");
			if (!string.IsNullOrEmpty(path))
				DataSaverContainer.BackupData(path);
		}

		[MenuItem("RCore/Player Data/Restore", priority = 104)]
		private static void RestoreData()
		{
			string path = EditorUtility.OpenFilePanel("Select Backup Data File", Application.dataPath, "txt");
			if (!string.IsNullOrEmpty(path))
				DataSaverContainer.RestoreData(path);
		}

		[MenuItem("RCore/Player Data/Log", priority = 105)]
		private static void LogData()
		{
			DataSaverContainer.LogData();
		}
	}
}