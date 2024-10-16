using RCore.Editor;
using RCore.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RCore.Data.JObject.Editor
{
	public class JObjectDBWindow : EditorWindow
	{
		private Dictionary<string, string> m_data;

		private void OnEnable()
		{
			m_data = JObjectDB.GetAllData();
		}

		private void OnGUI()
		{
			var actions = new List<IDraw>();
			actions.Add(new EditorButton
			{
				label = "Clear",
				onPressed = () =>
				{
					if (EditorHelper.ConfirmPopup())
					{
						JObjectDB.DeleteAll();
						m_data = JObjectDB.GetAllData();
						Repaint();
					}
				}
			});
			actions.Add(new EditorButton
			{
				label = "Back Up",
				onPressed = () => JObjectDB.Backup(openDirectory:true)
			});
			actions.Add(new EditorButton
			{
				label = "Restore",
				onPressed = () =>
				{
					string path = EditorUtility.OpenFilePanel("Select Backup Data File", Application.dataPath, "json");
					if (!string.IsNullOrEmpty(path))
					{
						JObjectDB.Restore(path);
						m_data = JObjectDB.GetAllData();
						Repaint();
					}
				}
			});
			actions.Add(new EditorButton
			{
				label = "Log and copy",
				onPressed = JObjectDB.Log
			});
			actions.Add(new EditorButton
			{
				label = "Save (In Game)",
				color = Application.isPlaying ? Color.yellow : Color.grey,
				onPressed = () =>
				{
					if (!Application.isPlaying)
					{
						Debug.Log("This Function should be called in Playing!");
						return;
					}
					JObjectDB.Save();
				},
			});
			EditorHelper.GridDraws(2, actions);
			
			GUILayout.BeginVertical("box");
			foreach (var pair in m_data)
			{
				GUILayout.BeginHorizontal();
				EditorHelper.LabelField(pair.Key, 150);
				EditorHelper.TextField(pair.Value, null);
				if (EditorHelper.Button("Edit", 60))
					Edit(pair.Key, pair.Value);
				GUILayout.EndHorizontal();
			}
			GUILayout.EndVertical();
		}

		private void Edit(string key, string content)
		{
			TextEditorWindow.ShowWindow(content, result =>
			{
				PlayerPrefs.SetString(key, result);
				m_data = JObjectDB.GetAllData();
			});
		}
	}
}