using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

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
            public Vector3 scale;
        }

        [ContextMenu("load file")]
        void ContextMenuLoad()
        {
            base.LoadFile();
        }

        [ContextMenu("save data")]
        void ContextMenuSave()
        {
            base.Save();
        }

        [ContextMenu("save as..")]
        void ContextMenuSaveAs()
        {
            base.SaveAs();
        }

#if UNITY_EDITOR
        [MenuItem("Assets/Create/LoadableSetting/LayoutSetting")]
        public static void Create()
        {
            var path = "Assets";
            foreach (var obj in Selection.GetFiltered<Object>(SelectionMode.Assets))
            {
                var p = AssetDatabase.GetAssetPath(obj);
                if (p != "")
                    path = p;
                if (!AssetDatabase.IsValidFolder(path))
                    path = Path.GetDirectoryName(path);
            }

            ProjectWindowUtil.CreateAsset(CreateInstance<LayoutSetting>(),
                AssetDatabase.GenerateUniqueAssetPath(Path.Combine(path, "LayoutSetting.asset")));
        }
#endif
    }
}