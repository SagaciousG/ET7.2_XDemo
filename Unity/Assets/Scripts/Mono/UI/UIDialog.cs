using System;
using ET;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ET
{
    public class UIDialog 
    {
        public enum ShowType
        {
            One,
            Two,
            Three,
        }
        
        public enum ClickedBtn
        {
            Right,
            Center,
            Left
        }

        private GameObject gameObject;
        private TextMeshProUGUI title;
        private TextMeshProUGUI desc;
        private TextMeshProUGUI leftText;
        private TextMeshProUGUI centerText;
        private TextMeshProUGUI rightText;
        private XImage leftBtn;
        private XImage rightBtn;
        private XImage centerBtn;

        private static UIDialog _instance;
        private ETTask<ClickedBtn> _task;

        private static UIDialog Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UIDialog();
                    var prefab = Resources.Load<GameObject>("UIDialog");
                    _instance.gameObject = UnityEngine.Object.Instantiate(prefab, Init.Instance.transform.Find("UI"));
                    _instance.Awake();
                }

                return _instance;
            }
        }
        
        private void Awake()
        {
            var canvas = gameObject.GetComponent<Canvas>();
            canvas.worldCamera = Init.Instance.UICamera;
            var rc = gameObject.GetComponent<UIReferenceCollector>();
            title = rc.Get<TextMeshProUGUI>("title");
            desc = rc.Get<TextMeshProUGUI>("desc");
            leftText = rc.Get<TextMeshProUGUI>("leftText");
            centerText = rc.Get<TextMeshProUGUI>("centerText");
            rightText = rc.Get<TextMeshProUGUI>("rightText");
            leftBtn = rc.Get<XImage>("leftBtn");
            rightBtn = rc.Get<XImage>("rightBtn");
            centerBtn = rc.Get<XImage>("centerBtn");
            
            leftBtn.OnClick(OnLeftClick);
            rightBtn.OnClick(OnRightClick);
            centerBtn.OnClick(OnCenterClick);
        }

        private void OnRightClick()
        {
            _task.SetResult(ClickedBtn.Right);
            gameObject.SetActive(false);
        }

        private void OnCenterClick()
        {
            _task.SetResult(ClickedBtn.Right);
            gameObject.SetActive(false);
        }

        private void OnLeftClick()
        {
            _task.SetResult(ClickedBtn.Right);
            gameObject.SetActive(false);
        }

        public static async ETTask<ClickedBtn> Show(string desc)
        {
            Instance._task = ETTask<ClickedBtn>.Create();
            Instance.SetShow(ShowType.Two);
            Instance.desc.text = desc;
            Instance.title.text = "提示";
            Instance.leftText.text = "取 消";
            Instance.rightText.text = "确 定";
            return await Instance._task;
        }
        
        public static async ETTask<ClickedBtn> Show(string desc, string title, string centerText)
        {
            Instance._task = ETTask<ClickedBtn>.Create();
            Instance.SetShow(ShowType.One);
            Instance.desc.text = desc;
            Instance.title.text = title;
            Instance.centerText.text = centerText;
            return await Instance._task;
        }
        
        public static async ETTask<ClickedBtn> Show(string desc, string title, string leftText, string rightText)
        {
            Instance._task = ETTask<ClickedBtn>.Create();
            Instance.SetShow(ShowType.Two);
            Instance.desc.text = desc;
            Instance.title.text = title;
            Instance.leftText.text = leftText;
            Instance.rightText.text = rightText;
            return await Instance._task;
        }
        
        public static async ETTask<ClickedBtn> Show(string desc, string title, string centerText, string leftText, string rightText)
        {
            Instance._task = ETTask<ClickedBtn>.Create();
            Instance.SetShow(ShowType.Three);
            Instance.desc.text = desc;
            Instance.title.text = title;
            Instance.centerText.text = centerText;
            Instance.leftText.text = leftText;
            Instance.rightText.text = rightText;
            return await Instance._task;
        }

        private void SetShow(ShowType showType)
        {
            Instance.gameObject.SetActive(true);
            switch (showType)
            {
                case ShowType.One:
                    leftBtn.gameObject.SetActive(false);
                    rightBtn.gameObject.SetActive(false);
                    centerBtn.gameObject.SetActive(true);
                    break;
                case ShowType.Two:
                    leftBtn.gameObject.SetActive(true);
                    rightBtn.gameObject.SetActive(true);
                    centerBtn.gameObject.SetActive(false);
                    break;
                case ShowType.Three:
                    leftBtn.gameObject.SetActive(true);
                    rightBtn.gameObject.SetActive(true);
                    centerBtn.gameObject.SetActive(true);
                    break;
            }
        }
    }
}