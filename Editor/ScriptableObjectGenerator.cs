using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


namespace sugi.cc.editor
{
    public class ScriptableObjectGenerator : EditorWindow
    {
        [MenuItem("sugi.cc/ScriptableObject Generator")]
        public static void Open()
        {
            GetWindow<ScriptableObjectGenerator>("ScriptableObject Generator");
        }

        MonoScript m_script;
        Button m_button;
        void OnEnable()
        {
            var root = rootVisualElement;

            var objectField = new ObjectField("Script");
            objectField.objectType = typeof(MonoScript);
            objectField.RegisterValueChangedCallback((val) =>
            {
                var script = val.newValue as MonoScript;
                if (script.GetClass().IsSubclassOf(typeof(ScriptableObject)) || script == null)
                    m_script = script;
                objectField.value = m_script;
                if (script == null)
                    m_button.style.display = DisplayStyle.None;
                else
                {
                    m_button.style.display = DisplayStyle.Flex;
                    m_button.text = $"Create {m_script.GetClass().Name} Instance";
                }
            });

            m_button = new Button(() =>
            {
                var t = m_script.GetClass();
                var obj = CreateInstance(t);
                var path = AssetDatabase.GenerateUniqueAssetPath($"Assets/new{t.Name}.asset");
                AssetDatabase.CreateAsset(obj, path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                Selection.activeObject = obj;
            });
            m_button.text = "Create";
            m_button.style.display = DisplayStyle.None;

            root.Add(objectField);
            root.Add(m_button);
        }
    }
}