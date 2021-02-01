using UnityEngine;
using UnityEngine.UIElements;

namespace sugi.cc.ui
{
    class FloatTextField : ValueTextField<float>
    {
        internal override bool TryParse(string s, out float result) => float.TryParse(s, out result);

        public new class UxmlFactory : UxmlFactory<FloatTextField, UxmlTraits>
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

    class IntTextField : ValueTextField<int>
    {
        internal override bool TryParse(string s, out int result) => int.TryParse(s, out result);

        public new class UxmlFactory : UxmlFactory<IntTextField, UxmlTraits>
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

    class Vector3TextField : VectorTextField<Vector3>
    {
        protected override int subelementCount => 3;

        public override void SetValue(Vector3 vec3)
        {
            for (var i = 0; i < 3; i++)
                subFields[i].SetValue(vec3[i]);
        }
        protected override void SetValue(int idx, float val)
        {
            vectorValue[idx] = val;
        }

        public new class UxmlFactory : UxmlFactory<Vector3TextField, UxmlTraits>
        {
        }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            UxmlStringAttributeDescription m_label = new UxmlStringAttributeDescription { name = "label", defaultValue = "Vector3 Field" };
            UxmlFloatAttributeDescription m_X_val = new UxmlFloatAttributeDescription { name = "x", defaultValue = 0f };
            UxmlFloatAttributeDescription m_Y_val = new UxmlFloatAttributeDescription { name = "y", defaultValue = 0f };
            UxmlFloatAttributeDescription m_Z_val = new UxmlFloatAttributeDescription { name = "z", defaultValue = 0f };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var field = (Vector3TextField)ve;
                field.label = m_label.GetValueFromBag(bag, cc);
                field.SetValue(
                    new Vector3(
                        m_X_val.GetValueFromBag(bag, cc),
                        m_Y_val.GetValueFromBag(bag, cc),
                        m_Z_val.GetValueFromBag(bag, cc)
                    )
                );
            }
        }
    }

    abstract class VectorTextField<T> : VisualElement
    {
        public abstract void SetValue(T val);
        public T GetValue() => vectorValue;
        protected T vectorValue;
        protected abstract void SetValue(int idx, float val);

        public string label { get => m_label; set { m_label = value; labelElement.text = value; } }
        string m_label;

        protected Label labelElement;
        protected FloatTextField[] subFields;
        protected abstract int subelementCount { get; }
        FloatTextField[] subelements
        {
            get
            {
                subFields = new FloatTextField[subelementCount];
                for (var i = 0; i < subelementCount; i++)
                    subFields[i] = new FloatTextField { value = "0" };
                return subFields;
            }
        }

        public VectorTextField() : base()
        {
            AddToClassList("value-field");

            labelElement = new Label();
            labelElement.AddToClassList("unity-base-field__label");

            var container = new VisualElement();
            container.AddToClassList("vector-elements");

            foreach (var sub in subelements)
                container.Add(sub);

            Add(labelElement);
            Add(container);

            container.Query<FloatTextField>().ForEach(field =>
            {
                field.RegisterValueChangedCallback(evt =>
                {
                    var field = (FloatTextField)evt.target;
                    var idx = field.parent.IndexOf(field);
                    SetValue(idx, field.GetValue());
                });
            });
        }
    }

    abstract class ValueTextField<T> : TextField
    {
        public void SetValue(T value)
        {
            this.value = value.ToString();
            m_value = value;
        }
        public T GetValue() => m_value;
        protected T m_value;

        protected ValueTextField() : base()
        {
            isDelayed = true;
            AddToClassList("value-field");
            RegisterCallback<ChangeEvent<string>>((evt) =>
            {
                var newValue = evt.newValue;
                var field = (TextField)evt.target;
                T val = m_value;
                if (TryParse(newValue, out val))
                    m_value = val;
                text = field.value = m_value.ToString();
            });
        }
        internal abstract bool TryParse(string s, out T result);
    }
}