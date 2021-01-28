using UnityEngine;
using UnityEngine.UIElements;

namespace sugi.cc.ui
{
    [ExecuteInEditMode, RequireComponent(typeof(UIDocument))]
    public class DisplayUI : MonoBehaviour
    {
        [SerializeField] private PanelSettings panelSettings;
        [SerializeField] private VisualTreeAsset sourceAsset;

        private UIDocument m_UIDocument;

        void Awake()
        {
            m_UIDocument = GetComponent<UIDocument>();
            m_UIDocument.panelSettings = panelSettings;
            m_UIDocument.visualTreeAsset = sourceAsset;
        }
    }
}