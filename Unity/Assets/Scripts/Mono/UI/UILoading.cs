using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace ET
{
    public class UILoading 
    {
        private static UILoading _instance;

        private GameObject gameObject;
        private TextMeshProUGUI proText;
        private XImage slider;

        private Tween _tween;

        private static UILoading Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UILoading();
                    var prefab = Resources.Load<GameObject>("UILoading");
                    _instance.gameObject = UnityEngine.Object.Instantiate(prefab, Init.Instance.transform.Find("UI"));
                    UnityEngine.Object.DontDestroyOnLoad(_instance.gameObject);
                    _instance.Awake();
                }

                return _instance;
            }
        }

        private void Awake()
        {
            var canvas = gameObject.GetComponent<Canvas>();
            canvas.worldCamera = Init.Instance.UICamera;
            var rc = this.gameObject.GetComponent<UIReferenceCollector>();
            this.proText = rc.Get<TextMeshProUGUI>("proTxt");
            this.slider = rc.Get<XImage>("progress");
        }

        public static void Set(float progress, string tips)
        {
            Instance.slider.fillAmount = progress;
            Instance.proText.text = tips;
        }
        
        public static void Show(float progress, string tips, Action onClose = null)
        {
            Instance.gameObject.SetActive(true);
            if (Instance._tween != null)
                Instance._tween.Kill();
            Instance._tween = DOTween.To(() => Instance.slider.fillAmount, a => Instance.slider.fillAmount = a, progress, 0.5f);
            Instance._tween.onComplete = () =>
            {
                if (progress >= 1)
                {
                    Hide();
                    onClose?.Invoke();
                }
            };
        }

        public static void Hide()
        {
            Instance.gameObject.SetActive(false);
        }
        
        
    }
}