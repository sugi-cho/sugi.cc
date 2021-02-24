using System;
using System.IO;
using UnityEngine;

namespace sugi.cc.data
{
    public abstract class LoadableSetting : ScriptableObject
    {
        [SerializeField] protected bool locked;
        public abstract string FilePath { get; }
        public abstract DataNameAndFilePath GetNameAndPath();
        public abstract void Load(string filePath);
        public abstract void Save();
    }

    public abstract class LoadableSetting<T> : LoadableSetting
    {
        public override string FilePath => filePath;
        [SerializeField] private string filePath;
        public T Data => data;
        public void SetData(T val) => data = val;
        [SerializeField] T data;

        public override DataNameAndFilePath GetNameAndPath()
        {
            return new DataNameAndFilePath()
            {
                dataName = typeof(T).Name,
                filePath = filePath
            };
        }

        protected virtual void Reset()
        {
            filePath = $"{typeof(T).Name}.json";
        }

        public override void Load(string filePath)
        {
            this.filePath = filePath;
            Load();
        }

        private void Load()
        {
            if (locked) {
                Debug.LogError($"setting of {name} is locked");
                return;
            }
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                JsonUtility.FromJsonOverwrite(json, data);
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

        public void Save(T data) {
            this.data = data;
            Save();
        }
        public void Save(string path, T data) {
            this.data = data;
            this.filePath = path;
            Save();
        }
        private void Save(string path, string json)
        {
            File.WriteAllText(path, json);
        }
    }

    [Serializable]
    public class DataNameAndFilePath
    {
        public string dataName;
        public string filePath;
    }
}