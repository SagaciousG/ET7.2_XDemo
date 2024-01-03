using System;
using UnityEngine;

namespace ET
{
    [RequireComponent(typeof(XToggle))]
    public class XUITab : MonoBehaviour
    {
        public RectTransform TabContent;
        public GameObject uiPrefab;

        public GameObject Instance => _instance;
        public event Action<GameObject> OnCreateInstance;

        public bool isOn
        {
            get => _toggle.isOn;
            set => _toggle.isOn = value;
        }
        private XToggle _toggle;
        private GameObject _instance;
        private void Awake()
        {
            _toggle = GetComponent<XToggle>();
            _toggle.onBeforeValueChanged += OnValueChanged;
        }

        private void OnValueChanged(XToggle arg1, bool arg2)
        {
            if (arg2)
            {
                if (_instance == null)
                {
                    _instance = Instantiate(uiPrefab, TabContent, false);
                    OnCreateInstance?.Invoke(_instance);
                }
                _instance.Display(true);
            }
            else
            {
                if (_instance != null)
                    _instance.Display(false);
            }
        }

        private void OnDestroy()
        {
            _toggle.onBeforeValueChanged -= OnValueChanged;
        }
    }
}