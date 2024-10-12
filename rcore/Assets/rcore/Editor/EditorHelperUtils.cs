using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCore.Editor
{
	public static class EditorHelperUtils
	{
		public static string[] GetTMPMaterialPresets(TMPro.TMP_FontAsset fontAsset)
		{
			if (fontAsset == null) return null;

			var materialReferences = TMPro.EditorUtilities.TMP_EditorUtility.FindMaterialReferences(fontAsset);
			var materialPresetNames = new string[materialReferences.Length];

			for (int i = 0; i < materialPresetNames.Length; i++)
				materialPresetNames[i] = materialReferences[i].name;

			return materialPresetNames;
		}
	}
}