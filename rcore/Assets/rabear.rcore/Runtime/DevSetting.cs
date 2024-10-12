﻿using System;
using System.Collections.Generic;
using UnityEngine;
using RCore.Common;
#if UNITY_EDITOR
using RCore.Common.Editor;
#endif

namespace RCore
{
	[CreateAssetMenu(fileName = "DevSetting", menuName = "RCore/Dev Setting")]
	public class DevSetting : ScriptableObject
	{
#region Internal Class

		[Serializable]
		public class Profile
		{
			public string name;
			public List<Directive> defines;
			public void AddDirective(string pName, bool defaultActive)
			{
				if (string.IsNullOrEmpty(pName))
					return;
				var find = GetDirective(pName);
				if (find == null)
					defines.Add(new Directive(pName, defaultActive));
			}
			public Directive GetDirective(string pName)
			{
				if (string.IsNullOrEmpty(pName))
					return null;

				for (int i = 0; i < defines.Count; i++)
					if (defines[i].name == pName)
						return defines[i];
				return null;
			}
		}

		[Serializable]
		public class Directive
		{
			public Directive()
			{
				color = Color.white;
			}
			public Directive(string pName, bool pEnable)
			{
				name = pName;
				enabled = pEnable;
				color = Color.white;
			}
			public string name;
			public Color color;
			public bool enabled = true;
		}

#endregion

		//==================================

		private static DevSetting m_Instance;
		public static DevSetting Instance
		{
			get
			{
				if (m_Instance == null)
				{
					m_Instance = Resources.Load<DevSetting>("DevSetting");
#if UNITY_EDITOR
					if (m_Instance == null)
					{
						EditorHelper.CreateScriptableAsset<DevSetting>("Assets/Resources/DevSetting.asset");
						m_Instance = Resources.Load<DevSetting>("DevSetting");
					}
#endif
					m_Instance.Init();
				}
				return m_Instance;
			}
		}

		public Action onSettingsChanged;
		public Profile profile = new Profile();
		public RPlayerPrefBool enableLogSystem;
		[SerializeField] private bool m_enableLog;
		[SerializeField] private bool m_enableDraw;

		private void Init()
		{
			enableLogSystem = new RPlayerPrefBool("EnableLogSystem");
			RCore.Common.Debug.enabled = m_enableLog;
			RCore.Common.DebugDraw.enabled = m_enableDraw;
		}
		public bool EnableLog
		{
			get => m_enableLog || enableLogSystem.Value;
			set
			{
				if (m_enableLog == value)
					return;
				m_enableLog = value;
				RCore.Common.Debug.enabled = value;
				onSettingsChanged?.Invoke();
			}
		}
		public bool EnableDraw
		{
			get => m_enableDraw;
			set
			{
				if (m_enableDraw == value)
					return;
				m_enableDraw = value;
				DebugDraw.enabled = value;
				onSettingsChanged?.Invoke();
			}
		}
	}
}