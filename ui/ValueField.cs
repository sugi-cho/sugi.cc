using UnityEngine;
using UnityEngine.UIElements;

namespace sugi.cc.ui
{
    class CustomFloatField : TextField
    {
        public new class UxmlFactory : UxmlFactory<CustomFloatField, UxmlTraits>
        {
        }

        public new class UxmlTraits : TextField.UxmlTraits
        {
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
            }
        }
    }

    class FloatField : TextField
    {
        public new class UxmlFactory : UxmlFactory<FloatField, UxmlTraits>
        {
        }

        public new class UxmlTraits : TextField.UxmlTraits
        {
            public float value { get; set; }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                Debug.Log("float field init");
                base.Init(ve, bag, cc);
                var field = (TextField) ve;
                field.isDelayed = true;
                field.AddToClassList("value-field");
                field.RegisterCallback<ChangeEvent<string>>((evt) =>
                {
                    var val = evt.newValue;
                    var field = (TextField) evt.target;
                    var newVal = value;
                    if (float.TryParse(val, out newVal))
                        value = newVal;
                    field.value = value.ToString();
                });
            }
        }
    }

    class IntField : TextField
    {
        public new class UxmlFactory : UxmlFactory<IntField, UxmlTraits>
        {
        }

        public new class UxmlTraits : TextField.UxmlTraits
        {
            public int value { get; set; }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var field = (TextField) ve;

                field.AddToClassList("value-field");
                field.RegisterCallback<ChangeEvent<string>>((evt) =>
                {
                    var val = evt.newValue;
                    var field = (TextField) evt.target;
                    var newVal = value;
                    if (int.TryParse(val, out newVal))
                        value = newVal;
                    field.value = value.ToString();
                });
            }
        }
    }

    class Vector3Field : ValueField<Vector3>
    {
        public Vector3Field(string label, Vector3 value)
        {
            var v3Element = new VisualElement();
            var xField = new TextField("X");
            var yField = new TextField("Y");
            var zField = new TextField("Z");
            xField.value = value.x.ToString();
            yField.value = value.y.ToString();
            zField.value = value.z.ToString();

            v3Element.Add(xField);
            v3Element.Add(yField);
            v3Element.Add(zField);
            v3Element.AddToClassList("vector-elements");

            v3Element.Query<TextField>().ForEach(tf =>
            {
                tf.isDelayed = true;
                tf.RegisterCallback<ChangeEvent<string>>((evt) =>
                {
                    var newVal = evt.newValue;
                    var field = (TextField) evt.target;
                    var index = field.parent.IndexOf(field);
                    var val = m_Value[index];
                    if (float.TryParse(newVal, out val))
                        m_Value[index] = val;
                    field.value = Value[index].ToString();
                    field.AddToClassList("value-field");
                });
            });
            if (label == null)
            {
                Element = v3Element;
            }
            else
            {
                var labelField = new Label(label);
                labelField.AddToClassList("unity-base-field__label");
                Element = new VisualElement();
                Element.AddToClassList("value-field");
                Element.Add(labelField);
                Element.Add(v3Element);
            }
        }
    }

    abstract class ValueField<T>
    {
        public static implicit operator VisualElement(ValueField<T> field) => field.Element;

        protected VisualElement Element;
        protected T m_Value;
        public T Value => m_Value;
    }
}