using UnityEngine;

namespace sugi.cc.data
{
    public class LayoutSetting : LoadableSetting<LayoutSetting.LayoutData>
    {
        public GameObject[] PrefabReferences => prefabReferences;

        [SerializeField] private GameObject[] prefabReferences;

        [System.Serializable]
        public class LayoutData
        {
            public ObjectSetting[] objectSettings;
        }

        [System.Serializable]
        public class ObjectSetting
        {
            public int prefabIdx;
            public Vector3 position;
            public Quaternion rotation;
            public Vector3 scale = Vector3.one;
        }

        [ContextMenu("load file")]
        void ContextMenuLoad()
        {
            base.Load(FilePath);
        }

        [ContextMenu("save data")]
        void ContextMenuSave()
        {
            base.Save();
        }
    }
}