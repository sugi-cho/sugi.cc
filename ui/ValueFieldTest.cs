using UnityEngine;
using UnityEngine.UIElements;

namespace sugi.cc.ui
{
    public class ValueFieldTest : MonoBehaviour
    {
        public TestEnum enumValue;

        public string targetClassName = "dataFieldContainer";

        private void OnEnable()
        {
            var uiDoc = GetComponent<UIDocument>();
            var root = uiDoc.rootVisualElement;
            var container = root.Q(null, targetClassName);

            var tf = new TextField();
            container.Add(new FloatTextField { label = "float field", value = "0" });


            var enumContaier = root.Q<VisualElement>("EnumField");
            if (enumContaier != null)
            {
                var enumField = new EnumButtonField<TestEnum>(
                    label: "test field",
                    defaultValue: enumValue,
                    callback: (val) => enumValue = val
                    );
                enumContaier.Add(enumField);
            }
        }

        public enum TestEnum
        {
            Test1, Test2, Test3,
        }
    }
}
