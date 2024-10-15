using RCore.Common;
using System;
using UnityEditor;
using UnityEngine;

namespace RCore.Data.JObject.Editor
{
	[CustomEditor(typeof(JObjectDBManager))]
	public class JObjectDBManagerEditor : UnityEditor.Editor
	{
		protected JObjectDBManager m_target;

		private void OnEnable()
		{
			m_target = target as JObjectDBManager;
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
				
			GUILayout.BeginVertical("box");
				
			if (GUILayout.Button("Save"))
				JObjectDB.Save(DateTime.UtcNow.ToUnixTimestampInt());
				
			if (GUILayout.Button("Back Up"))
			{
				var time = DateTime.Now;
				string path = EditorUtility.SaveFilePanelInProject("Backup Data", "GameData_"
					+ $"{time.Year % 100}{time.Month:00}{time.Day:00}_{time.Hour:00}h{time.Minute:00}"
					+ ".json", "json,txt", "Please enter a file name to save!");

				if (!string.IsNullOrEmpty(path))
					JObjectDB.BackupData(path);
			}
				
			if (GUILayout.Button("Log"))
				JObjectDB.LogData();
				
			if (!Application.isPlaying)
			{
				if (GUILayout.Button("Delete All"))
					JObjectDB.DeleteAllData();
				
				if (GUILayout.Button("Restore"))
				{
					string filePath = EditorUtility.OpenFilePanel("Select Data File", Application.dataPath, "json,txt");
					if (!string.IsNullOrEmpty(filePath))
						JObjectDB.RestoreData(filePath);
				}
			}
				
			GUILayout.EndVertical();
		}
	}
}