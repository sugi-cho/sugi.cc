using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace sugi.cc.ui
{
    [RequireComponent(typeof(UIDocument))]
    public class KeyboardUIControl : MonoBehaviour
    {
        [FormerlySerializedAs("m_ActivateAction")] [SerializeField] private InputAction activateAction;
        [SerializeField] private string targetClassName;
        [SerializeField] private string additionalClassName;

        private void OnEnable()
        {
            activateAction.Enable();
            activateAction.performed += OnActivate;
        }

        void OnActivate(InputAction.CallbackContext context)
        {
            Debug.Log(context.valueType);
        }
    }
}
