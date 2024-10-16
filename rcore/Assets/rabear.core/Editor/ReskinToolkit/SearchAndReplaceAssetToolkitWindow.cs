using RCore.Common;
using RCore.Editor;
using UnityEditor;
using UnityEngine;

namespace RCore.Editor
{
	public class SearchAndReplaceAssetToolkitWindow : EditorWindow
	{
		private Vector2 m_scrollPosition;
		private SearchAndReplaceAssetToolkit m_searchAndReplaceAssetToolkit;
		private string m_tab;

		private void OnGUI()
		{
			m_scrollPosition = GUILayout.BeginScrollView(m_scrollPosition, false, false);
			m_searchAndReplaceAssetToolkit ??= SearchAndReplaceAssetToolkit.Load();
			m_tab = EditorHelper.Tabs("m_assetsReplacer.spriteReplace", "Replace Sprite", "Cut Sprite Sheet", "Update Image Property", "Replace Object");
			GUILayout.BeginVertical("box");
			switch (m_tab)
			{
				case "Replace Sprite":
					m_searchAndReplaceAssetToolkit.replaceSpriteTool.Draw();
					break;
				case "Cut Sprite Sheet":
					m_searchAndReplaceAssetToolkit.cutSpriteSheetTool.Draw();
					break;
				case "Update Image Property":
					m_searchAndReplaceAssetToolkit.updateImagePropertyTool.Draw();
					break;
				case "Replace Object":
					m_searchAndReplaceAssetToolkit.replaceObjectTool.Draw();
					break;
			}
			GUILayout.EndVertical();
			GUILayout.EndScrollView();
		}

		[MenuItem("RCore/Tools/Search And Replace Asset Toolkit")]
		private static void OpenEditorWindow()
		{
			var window = GetWindow<SearchAndReplaceAssetToolkitWindow>("Search And Replace Asset Toolkit", true);
			window.Show();
		}
	}
}