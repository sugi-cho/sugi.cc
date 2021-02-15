using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Linq;

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

    class EnumButtonField : VisualElement
    {
        public string label;
        public Enum enumValue
        {
            get => m_enumValue;
            private set
            {
                m_enumValue = value;
                if (OnValueChanged != null)
                    OnValueChanged.Invoke(m_enumValue);
            }
        }
        Enum m_enumValue;

        public Action<Enum> OnValueChanged;

        public EnumButtonField(string label, Enum defaultValue, Action<Enum> callback = null) : base()
        {
            if (callback != null)
                OnValueChanged += callback;

            enumValue = defaultValue;
            var type = defaultValue.GetType();
            var names = Enum.GetNames(type).ToList();

            style.flexDirection = FlexDirection.Row;
            var labelField = new Label(label);
            labelField.style.flexGrow = 1;
            Add(labelField);

            var selectField = new VisualElement();
            Add(selectField);

            var selections = new VisualElement();
            var button = new Button(() => { 
                enumValue = m_enumValue;
                selections.style.display = DisplayStyle.None;
            });
            button.text = enumValue.ToString();
            button.style.flexGrow = 1;

            selectField.Add(button);
            selectField.Add(selections);

            foreach (var name in names)
            {
                var item = new Label(name);
                item.AddToClassList("unity-list-view__item");
                item.RegisterCallback<ClickEvent>((evt) =>
                {
                    var l = (evt.target as Label);

                    if (!l.ClassListContains("unity-list-view__item--selected"))
                    {
                        l.parent.Query(null, "unity-list-view__item--selected")
                        .ForEach((selected) => selected.RemoveFromClassList("unity-list-view__item--selected"));
                        l.AddToClassList("unity-list-view__item--selected");
                        button.text = l.text;
                        enumValue = (Enum)Enum.Parse(type, button.text);
                    }
                    selections.style.display = DisplayStyle.None;
                });
                selections.Add(item);
            }

            selections.style.display = DisplayStyle.None;
            button.RegisterCallback<PointerEnterEvent>((evt) => selections.style.display = DisplayStyle.Flex);
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