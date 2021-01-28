using System;
using System.IO;
using System.Linq;
using UnityEngine;
using SFB;

namespace sugi.cc.data
{
    public class AppSetting : MonoBehaviour
    {
        [SerializeField] private string settingFilePath = "setting.json";
        [SerializeField] private LoadableSetting[] loadableSettings;
        [SerializeField] private FilePathData filePathData;

        private string m_FilePath;

        private void Awake()
        {
            filePathData.filePaths = loadableSettings.Select(setting => setting.GetNameAndPath()).ToArray();
            m_FilePath = Path.Combine(Application.streamingAssetsPath, settingFilePath);
            LoadSettingFilePaths();
            LoadLoadableSettings();
        }

        public void LoadSettingFilePaths()
        {
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

        [Serializable]
        public struct FilePathData
        {
            public DataNameAndFilePath[] filePaths;
        }
    }
}