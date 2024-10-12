/***
 * Author RadBear - nbhung71711 @gmail.com - 2018
 **/

using TMPro;
using UnityEngine;
using RCore.Common;
using UnityEngine.Serialization;

namespace RCore.UI
{
	[AddComponentMenu("RCore/UI/SimpleTMPButton")]
	public class SimpleTMPButton : JustButton
	{
		[SerializeField] protected TextMeshProUGUI mLabelTMP;

		public TextMeshProUGUI labelTMP
		{
			get
			{
				if (mLabelTMP == null && !mFindLabel)
				{
					mLabelTMP = GetComponentInChildren<TextMeshProUGUI>();
					mFindLabel = true;
				}
				return mLabelTMP;
			}
		}

		private bool mFindLabel;

		[FormerlySerializedAs("mFontColorSwap")]
		[SerializeField] protected bool m_fontColorOnOffSwap;
		[FormerlySerializedAs("mFontColorActive")]
		[SerializeField] protected Color m_fontColorOn;
		[FormerlySerializedAs("mFontColorInactive")]
		[SerializeField] protected Color m_fontColorOff;

		[FormerlySerializedAs("m_LabelMatSwap")]
		[SerializeField] protected bool m_labelMatOnOffSwap;
		[FormerlySerializedAs("m_LabelMatActive")]
		[SerializeField] public Material m_labelMatOn;
		[FormerlySerializedAs("m_LabelMatInactive")]
		[SerializeField] public Material m_labelMatOff;

#if UNITY_EDITOR
		[ContextMenu("Validate")]
		protected override void OnValidate()
		{
			base.OnValidate();

			if (mLabelTMP == null)
				mLabelTMP = GetComponentInChildren<TextMeshProUGUI>();
			if (mLabelTMP == null)
				m_labelMatOnOffSwap = false;
			if (!m_labelMatOnOffSwap)
			{
				m_labelMatOn = null;
				m_labelMatOff = null;
			}
			else if (m_labelMatOn == null)
			{
				m_labelMatOn = mLabelTMP.fontSharedMaterial;
			}
		}
#endif
		
		public override void SetEnable(bool pValue)
		{
			base.SetEnable(pValue);

			if (pValue)
			{
				if (m_fontColorOnOffSwap)
					mLabelTMP.color = m_fontColorOn;
				if (m_labelMatOnOffSwap && m_labelMatOn != null && m_labelMatOff != null)
				{
					var labels = gameObject.FindComponentsInChildren<TextMeshProUGUI>();
					foreach (var label in labels)
					{
						if (label.font == mLabelTMP.font && label.fontSharedMaterial == mLabelTMP.fontSharedMaterial)
							label.fontSharedMaterial = m_labelMatOn;
					}
				}
			}
			else
			{
				if (m_fontColorOnOffSwap)
					mLabelTMP.color = m_fontColorOff;
				if (m_labelMatOnOffSwap && m_labelMatOn != null && m_labelMatOff != null)
				{
					var labels = gameObject.FindComponentsInChildren<TextMeshProUGUI>();
					foreach (var label in labels)
					{
						if (label.font == mLabelTMP.font && label.fontSharedMaterial == mLabelTMP.fontSharedMaterial)
							label.fontSharedMaterial = m_labelMatOff;
					}
				}
			}
		}
	}
}