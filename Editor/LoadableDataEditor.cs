using System;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

using sugi.cc.data;

namespace sugi.cc.editor
{
    public class LoadableDataEditor : EditorWindow
    {
        [MenuItem("sugi.cc/Setting Generator")]
        public static void Open()
        {
            var window = GetWindow<LoadableDataEditor>();
            window.titleContent = new GUIContent("Loadable Setting Generator");
            window.minSize = new Vector2(300, 200);
        }

        Type[] subclasses =>
            AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a =>
                        a.GetTypes().Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(LoadableSetting)))
                    ).ToArray();

        private void OnEnable()
        {
            var root = rootVisualElement;

            foreach (var t in subclasses)
            {
                var ve = new VisualElement();
                var label = new Label($"{t.Name}");
                var button = new Button(() =>
                {
                    var path = AssetDatabase.GenerateUniqueAssetPath($"Assets/new {t.Name}.asset");
                    var obj = CreateInstance(t);
                    AssetDatabase.CreateAsset(obj, path);
                    Selection.activeObject = obj;
                });
                button.text = "Create";

                ve.AddToClassList("box");
                ve.Add(label);
                ve.Add(button);
                root.Add(ve);
            }
        }
    }
}