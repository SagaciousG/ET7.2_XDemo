using System;
using ET;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ET
{
    public class XInputField : TMP_InputField
    {
        private interface IInputEvent
        {
            void Invoke();
        }
        
        private class InputEvent<T> : IInputEvent
        {
            public Action<XInputField, T> Action;
            public XInputField Self;
            public T Arg;
            
            public void Invoke()
            {
                this.Action.Invoke(this.Self, this.Arg);   
            }
        }
        
        private MultiMap<string, IInputEvent> _inputEvents = new MultiMap<string, IInputEvent>();

        protected override void Start()
        {
            base.Start();
            this.onSelect.AddListener(OnSelect);
            this.onDeselect.AddListener(OnDeselect);
            this.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnValueChanged(string arg0)
        {
            Invoke("OnValueChanged");
        }

        private void OnSelect(string arg0)
        {
            Invoke("OnSelect");
        }
        
        private void OnDeselect(string arg0)
        {
            Invoke("OnDeselect");
        }


        private void Invoke(string key)
        {
            if (this._inputEvents.TryGetValue(key, out var val))
            {
                foreach (IInputEvent inputEvent in val)
                {
                    inputEvent.Invoke();
                }
            }
        }

        public void AddSelectListener<T>(Action<XInputField, T> action, T arg)
        {
            this._inputEvents.Add("OnSelect", new InputEvent<T>(){Action = action, Arg =  arg, Self =  this});
        }
        
        public void AddDeselectListener<T>(Action<XInputField, T> action, T arg)
        {
            this._inputEvents.Add("OnDeselect", new InputEvent<T>(){Action = action, Arg =  arg, Self =  this});
        }
        
        public void AddValueChangeListener<T>(Action<XInputField, T> action, T arg)
        {
            this._inputEvents.Add("OnValueChanged", new InputEvent<T>(){Action = action, Arg =  arg, Self =  this});
        }
    }
}