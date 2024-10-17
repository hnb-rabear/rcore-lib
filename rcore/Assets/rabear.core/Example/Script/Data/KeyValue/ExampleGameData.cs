﻿using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using RCore.Common;
using RCore.Data.KeyValue;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace RCore.Example.Data.KeyValue
{
    public class ExampleGameData : DataManager
    {
        private const bool ENCRYPT_FILE = false;
        private const bool ENCRYPT_SAVER = false;
        private static readonly Encryption FILE_ENCRYPTION = new Encryption();

        public static ExampleGameData mInstance;
        public static ExampleGameData Instance => mInstance;

        public DemoGroup1 exampleGroup;
        public DemoGroup3 demoGroup;

        private KeyValueCollection m_keyValueCollection;
        private bool mInitialized;

        private void Awake()
        {
            if (mInstance == null)
                mInstance = this;
            else if (mInstance != this)
                Destroy(gameObject);
        }

        public override void Init()
        {
            if (mInitialized)
                return;

            mInitialized = true;

            m_keyValueCollection = KeyValueDB.CreateCollection("example", ENCRYPT_SAVER ? FILE_ENCRYPTION : null);

            exampleGroup = AddMainDataGroup(new DemoGroup1(0), m_keyValueCollection);
            demoGroup = AddMainDataGroup(new DemoGroup3(1), m_keyValueCollection);

            base.Init();
        }

        private void RandomizeData()
        {
            exampleGroup.integerData.Value = Random.Range(0, 100);
            exampleGroup.floatData.Value = Random.Range(0, 100) * 100;
            exampleGroup.longData.Value = Random.Range(0, 100) * 10000;
            exampleGroup.stringData.Value = Random.Range(0, 100) + "asd";
            exampleGroup.boolData.Value = Random.Range(0, 100) > 50;
            exampleGroup.dateTimeData.Set(DateTime.Now);
            exampleGroup.timedTask.Start(100);
            exampleGroup.RandomizeData();
        }

        private void Log()
        {
            Debug.Log("integerData: " + exampleGroup.integerData.Value);
            Debug.Log("floatData: " + exampleGroup.floatData.Value);
            Debug.Log("longData: " + exampleGroup.longData.Value);
            Debug.Log("stringData: " + exampleGroup.stringData.Value);
            Debug.Log("boolData: " + exampleGroup.boolData.Value);
            Debug.Log("dateTimeData: " + exampleGroup.dateTimeData.Get());
            Debug.Log("timerTask: " + exampleGroup.timedTask.RemainSeconds);
        }

        private void LogAll()
        {
            var savedData = m_keyValueCollection.GetSavedData();
            var currentData = m_keyValueCollection.GetCurrentData();
            Debug.Log("Saved Data: " + savedData);
            Debug.Log("Running Data: " + currentData);
        }

        public static string LoadFile(string pPath, bool pEncrypt = ENCRYPT_FILE)
        {
            if (pEncrypt)
                return DataManager.LoadFile(pPath, FILE_ENCRYPTION);
            else
                return DataManager.LoadFile(pPath, null);
        }

#if UNITY_EDITOR
        [CustomEditor(typeof(ExampleGameData))]
        private class ExampleGameDataEditor : DataManagerEditor
        {
            private ExampleGameData mScript;

            private void OnEnable()
            {
                mScript = target as ExampleGameData;
            }

            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                if (Application.isPlaying)
                {
                    if (GUILayout.Button("RandomizeData"))
                        mScript.RandomizeData();
                    if (GUILayout.Button("Log"))
                        mScript.Log();
                    if (GUILayout.Button("LogAll"))
                        mScript.LogAll();
                }
                else
                    EditorGUILayout.HelpBox("Click play to see how it work", MessageType.Info);
            }
        }
#endif
    }
}