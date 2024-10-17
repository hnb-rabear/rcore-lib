using UnityEngine;
using RCore.UI;
using RCore.Example.Data.KeyValue;
using TMPro;

namespace RCore.Example.UI
{
    public class PanelExample : PanelController
    {
        [SerializeField] private CustomToggleSlider m_toggleSlider;
        [SerializeField] private CustomToggleTab m_tab1;
        [SerializeField] private CustomToggleTab m_tab2;
        [SerializeField] private CustomToggleTab m_tab3;
        [SerializeField] private CustomToggleTab m_tab4;
        [SerializeField] private JustButton m_btnSave;
        [SerializeField] private SimpleTMPButton m_btnLoad;
        [SerializeField] private ProgressBar m_progressBar;
        [SerializeField] private TMP_InputField m_inputField;
        
        private float mTime;

        private ExampleGameData GameData => ExampleGameData.Instance;

        private void Start()
        {
            m_progressBar.Max = 20;
            m_btnSave.onClick.AddListener(SaveData);
            m_btnLoad.onClick.AddListener(LoadData);
        }

        private void OnEnable()
        {
            LoadData();
        }

        private void Update()
        {
            mTime += Time.deltaTime;
            m_progressBar.Value = mTime % 30f;
            //Or
            //mProgressBar.FillAmount = (mTime % 30f) / 30f;
        }

        [InspectorButton]
        private void LoadData()
        {
            m_toggleSlider.isOn = GameData.demoGroup.toggleIsOn.Value;
            m_inputField.text = GameData.demoGroup.inputFieldText.Value;
            m_progressBar.Value = GameData.demoGroup.progressBarValue.Value;
            mTime = m_progressBar.Value;
            switch (GameData.demoGroup.tabIndex.Value)
            {
                case 1: m_tab1.isOn = true; break;
                case 2: m_tab2.isOn = true; break;
                case 3: m_tab3.isOn = true; break;
                case 4: m_tab4.isOn = true; break;
            }
        }

        [InspectorButton]
        private void SaveData()
        {
            GameData.demoGroup.toggleIsOn.Value = m_toggleSlider.isOn;
            GameData.demoGroup.inputFieldText.Value = m_inputField.text;
            GameData.demoGroup.progressBarValue.Value = m_progressBar.Value;
            if (m_tab1.isOn)
                GameData.demoGroup.tabIndex.Value = 1;
            else if (m_tab2.isOn)
                GameData.demoGroup.tabIndex.Value = 2;
            else if (m_tab3.isOn)
                GameData.demoGroup.tabIndex.Value = 3;
            else if (m_tab4.isOn)
                GameData.demoGroup.tabIndex.Value = 4;
        }
    }
}