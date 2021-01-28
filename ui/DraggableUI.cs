using UnityEngine;
using UnityEngine.UIElements;

namespace sugi.cc.ui
{
    [ExecuteInEditMode, RequireComponent(typeof(UIDocument), typeof(EventSystem))]
    public class DraggableUI : MonoBehaviour
    {
        [SerializeField] private string draggableClassName = "draggable";
        [SerializeField] private string draggingClassName = "dragging";

        private VisualElement m_ActiveElement;

        private Vector2 m_StartPosition;
        private Vector2 m_PointerStartPosition;
        private Vector2 m_moveAreaMin;
        private Vector2 m_moveAreaMax;


        private void OnEnable()
        {
            var rootElement = GetComponent<UIDocument>().rootVisualElement;
            rootElement.Query(null, draggableClassName).ForEach(e =>
            {
                e.RegisterCallback<PointerDownEvent>(OnPointerDown);
                e.RegisterCallback<PointerMoveEvent>(OnPointerMove);
                e.RegisterCallback<PointerUpEvent>(OnPointerUp);
            });
        }

        void OnPointerDown(PointerDownEvent evt)
        {
            if (m_ActiveElement == null && evt.currentTarget == evt.target)
            {
                evt.target.CapturePointer(evt.pointerId);
                var ve = (VisualElement) evt.target;

                StartActiveElement(ve, evt.position);
            }
        }

        void OnPointerMove(PointerMoveEvent evt)
        {
            var ve = (VisualElement) evt.target;
            if (evt.target.HasPointerCapture((evt.pointerId)))
            {
                MoveElement(evt.position);
            }
        }

        void OnPointerUp(PointerUpEvent evt)
        {
            if (evt.target.HasPointerCapture((evt.pointerId)))
            {
                var ve = (VisualElement) evt.target;
                evt.target.ReleasePointer(evt.pointerId);
                ReleaseActiveElement();
            }
        }

        void StartActiveElement(VisualElement ve, Vector2 pointerPosition)
        {
            m_ActiveElement = ve;
            var container = ve.parent;
            var size = new Vector2(container.resolvedStyle.width, container.resolvedStyle.height);
            m_moveAreaMin = new Vector2(container.resolvedStyle.paddingLeft, container.resolvedStyle.paddingTop);
            m_moveAreaMax = size - new Vector2(
                ve.resolvedStyle.width + ve.resolvedStyle.marginRight + ve.resolvedStyle.paddingRight +
                container.resolvedStyle.paddingRight,
                ve.resolvedStyle.height + ve.resolvedStyle.marginBottom + ve.resolvedStyle.paddingBottom +
                container.resolvedStyle.paddingBottom);

            m_ActiveElement.AddToClassList(draggingClassName);
            m_StartPosition = m_ActiveElement.layout.position;
            m_PointerStartPosition = pointerPosition;

            ve.PlaceInFront(container.ElementAt(container.childCount - 1));
        }

        void MoveElement(Vector2 pointerPosition)
        {
            var pos = m_StartPosition + pointerPosition - m_PointerStartPosition;
            pos = Vector2.Max(pos, m_moveAreaMin);
            pos = Vector2.Min(pos, m_moveAreaMax);
            m_ActiveElement.style.left = pos.x;
            m_ActiveElement.style.top = pos.y;
        }

        void ReleaseActiveElement()
        {
            m_ActiveElement.RemoveFromClassList(draggingClassName);
            m_ActiveElement = null;
        }
    }
}