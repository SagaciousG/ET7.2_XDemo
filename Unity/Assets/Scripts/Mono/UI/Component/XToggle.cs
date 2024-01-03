using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ET
{
    public class XToggle : MonoBehaviour, IPointerClickHandler
    {
        public RectTransform On;
        public RectTransform Off;
        public XToggleGroup ToggleGroup;
        public event Action<XToggle, bool> onBeforeValueChanged;
        public event Action<XToggle, bool> onValueChanged;
        public event Action OnClick;

        private bool _started;
        public bool isOn
        {
            get => _isOn;
            set
            {
                if (!enabled)
                    return;
                _isOn = value;
                if (!_started)
                    return;
                DoValue();
            }
        }

        [SerializeField]
        private bool _isOn;

        private void DoValue()
        {
            if (On != null)
                On.Display(_isOn);
            if (Off != null)
                Off.Display(!_isOn);
            onBeforeValueChanged?.Invoke(this, _isOn);
            onValueChanged?.Invoke(this, _isOn);
            if (_isOn)
            {
                if (ToggleGroup != null)
                    ToggleGroup.NotifyToggleOn(this);
            }
        }

        private void OnEnable()
        {
            if (ToggleGroup != null)
                ToggleGroup.Add(this);
        }

        private void OnDisable()
        {
            if (ToggleGroup != null)
                ToggleGroup.Remove(this);
        }

        private void Start()
        {
            _started = true;
            DoValue();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (enabled)
            {
                OnClick?.Invoke();
                if (ToggleGroup != null && isOn)
                {
                    return;
                }
                isOn = !_isOn;
            }
        }
    }
}