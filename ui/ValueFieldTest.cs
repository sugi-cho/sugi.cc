using UnityEngine;
using UnityEngine.UIElements;

namespace sugi.cc.ui
{
    public class ValueFieldTest : MonoBehaviour
    {
        public string targetClassName = "dataFieldContainer";
        
        private void OnEnable()
        {
            var uiDoc = GetComponent<UIDocument>();
            var root = uiDoc.rootVisualElement;
            var container = root.Q(null, targetClassName);
            
            container.Add(new FloatField("float field", 5f));
            container.Add(new Vector3Field("v3 field", Vector3.one));
        }
    }
}
