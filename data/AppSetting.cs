using System;
using System.IO;
using System.Linq;
using UnityEngine;

namespace sugi.cc.data
{
    public class AppSetting : MonoBehaviour
    {
        [SerializeField] private string settingFilePath = "setting.json";
        [SerializeField] private LoadableSetting[] loadableSettings;
        [SerializeField] private FilePathData filePathData;

        private string m_FilePath;

        private void OnValidate()
        {
            filePathData.filePaths = loadableSettings.Select(setting => setting.GetNameAndPath()).ToArray();
        }

        private void Awake()
        {
            LoadSettingFilePaths();
            LoadLoadableSettings();
        }

        public void LoadSettingFilePaths()
        {
            m_FilePath = Path.Combine(Application.streamingAssetsPath, settingFilePath);
            if (File.Exists(m_FilePath))
            {
                var json = File.ReadAllText(m_FilePath);
                JsonUtility.FromJsonOverwrite(json, filePathData);
            }
            else
            {
                SaveSettingFilePaths();
            }
        }

        public void SaveSettingFilePaths()
        {
            m_FilePath = Path.Combine(Application.streamingAssetsPath, settingFilePath);
            filePathData.filePaths = loadableSettings.Select(setting => setting.GetNameAndPath()).ToArray();
            var json = JsonUtility.ToJson(filePathData);
            File.WriteAllText(m_FilePath, json);
        }

        void LoadLoadableSettings()
        {
            for (var i = 0; i < loadableSettings.Length; i++)
            {
                loadableSettings[i].Load(filePathData.filePaths[i].filePath);
            }
        }

        [ContextMenu("load setting")]
        void LoadJson() => LoadSettingFilePaths();
        [ContextMenu("save setting")]
        void SaveJson() => SaveSettingFilePaths();

        [Serializable]
        public struct FilePathData
        {
            public DataNameAndFilePath[] filePaths;
        }
    }
}