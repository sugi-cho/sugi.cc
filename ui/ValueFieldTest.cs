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

            var tf = new TextField();
            container.Add(new FloatTextField{label = "float field", value = "0"});
         //   container.Add(new Vector3Field("v3 field", Vector3.one));
        }
    }
}
