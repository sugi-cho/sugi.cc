using System;
using System.IO;
using UnityEngine;

namespace sugi.cc.data
{
    public abstract class LoadableSetting : ScriptableObject
    {
        public string FilePath => filePath;
        [SerializeField] protected string filePath;
        public abstract DataNameAndFilePath GetNameAndPath();
        public abstract void Load(string filePath);
        public abstract void Save();
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

        private void Load()
        {
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
            base.filePath = path;
            Save();
        }
    }

    [Serializable]
    public class DataNameAndFilePath
    {
        public string dataName;
        public string filePath;
    }
}