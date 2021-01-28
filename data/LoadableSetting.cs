using System;
using System.IO;
using UnityEngine;
using SFB;

namespace sugi.cc.data
{
    public abstract class LoadableSetting : ScriptableObject
    {
        [SerializeField] protected string filePath;
        public abstract DataNameAndFilePath GetNameAndPath();
        public abstract void Load(string filePath);
        public abstract void LoadFile();
        public abstract void Save();
        public abstract void SaveAs();
        protected void Save(string path, string json)
        {
            File.WriteAllText(path, json);
        }
    }

    public abstract class LoadableSetting<T> : LoadableSetting
    {
        public T Data => data;
        [SerializeField] protected T data;

        public override DataNameAndFilePath GetNameAndPath()
        {
            return new DataNameAndFilePath()
            {
                dataName = typeof(T).Name,
                filePath = filePath
            };
        }


        private void Reset()
        {
            filePath = $"{typeof(T).Name}.json";
        }

        public override void Load(string filePath)
        {
            base.filePath = filePath;
            Load();
        }

        public override void LoadFile()
        {
            var directoryName = 0 < filePath.Length ? Path.GetDirectoryName(filePath) : "";
            var fileName = 0 < filePath.Length ? Path.GetFileName(filePath) : "";
            var extensions = new[]
            {
                new ExtensionFilter("Json File", "json"),
                new ExtensionFilter("All Files", "*")
            };
            StandaloneFileBrowser.OpenFilePanelAsync(
                $"Load {typeof(T).Name} JSON", directoryName, extensions, false, (paths) =>
                {
                    if (0 < paths.Length)
                        Load(paths[0]);
                });
        }

        private void Load()
        {
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                data = JsonUtility.FromJson<T>(json);
                Debug.Log($"loaded: {filePath}");
            }
            else
            {
                Debug.LogWarning($"file: {filePath} doesn't exist!");
                Save();
            }
        }

        public override void Save()
        {
            var json = JsonUtility.ToJson(data);
            Save(filePath, json);
            Debug.Log($"saved: {filePath}");
        }


        public override void SaveAs()
        {
            var directoryName = 0 < filePath.Length ? Path.GetDirectoryName(filePath) : "";
            var fileName = 0 < filePath.Length ? Path.GetFileName(filePath) : "";
            var extensions = new[]
            {
                new ExtensionFilter("Json File", "json"),
                new ExtensionFilter("All Files", "*")
            };
            StandaloneFileBrowser.SaveFilePanelAsync($"Save {typeof(T).Name}", directoryName, fileName, extensions,
                (path) =>
                {
                    if (path.Length == 0) return;
                    filePath = path;
                    Save();
                });
        }

        public void SaveAs(string json)
        {
            var directoryName = 0 < filePath.Length ? Path.GetDirectoryName(filePath) : "";
            var fileName = 0 < filePath.Length ? Path.GetFileName(filePath) : "";
            var extensions = new[]
            {
                new ExtensionFilter("Json File", "json"),
                new ExtensionFilter("All Files", "*")
            };
            StandaloneFileBrowser.SaveFilePanelAsync($"Save {typeof(T).Name}", directoryName, fileName, extensions,
                (path) =>
                {
                    if (path.Length == 0) return;
                    Save(path, json);
                });
        }
    }

    [Serializable]
    public class DataNameAndFilePath
    {
        public string dataName;
        public string filePath;
    }
}