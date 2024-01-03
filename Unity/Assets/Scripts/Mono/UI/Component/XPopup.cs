using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ET;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class XPopup : MonoBehaviour
    {
        [SerializeField] private XText _title;
        [SerializeField] private Graphic _clickArea;
        [SerializeField] private XText _selectText;
        [SerializeField] private XImage _arrowOn;
        [SerializeField] private XImage _arrowOff;
        [SerializeField] private UIList _popUp;
        [SerializeField] private UIClickInRect _areaListener;
        private IList _sourceData;
        private bool _isShowPopup;
        private List<string> _showNames = new List<string>();
        private Canvas _canvas => gameObject.GetComponentInParent<Canvas>();
        [NonSerialized]
        public int defaultSelectIndex = 0;

        
        public event Action<int, object> OnSelect;
        public event Func<int, object, bool> SelectCheckHandler;

        public bool ClickOtherAreaClose;

        private void InAreaListener(UIClickInRect rect, bool obj)
        {
            if (!obj)
            {
                var rectTransform = GetComponent<RectTransform>();
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    rectTransform, Input.mousePosition,
                    _canvas.worldCamera, out var localPoint);
                var inArea = _clickArea.rectTransform.rect.Contains(localPoint);
                if (inArea)
                    return;
                _isShowPopup = false;
                Pop(false);
            }
        }

        public int selectedIndex
        {
            set => SetSelectedIndex(value);
            get => _popUp.SelectedIndex;
        }
        
        public string label
        {
            set
            {
                if (_title != null)
                    _title.text = value;
            }
            get => _title?.text ?? "";
        }

        public object SelectedData
        {
            get => _sourceData[selectedIndex];
        }

        public void SetData(IList data, IEnumerable<string> shows)
        {
            _sourceData = data;

            if (data.Count != shows.Count())
                throw new Exception($"数据数量必须等于展示数量");
            _showNames.Clear();
            _showNames.AddRange(shows);
            _popUp.SetData(_showNames);
        }
        
        public void SetData(IList data, string fieldName = null)
        {
            _sourceData = data;

            _showNames.Clear();
            foreach (var d in data)
            {
                var str = string.IsNullOrEmpty(fieldName) ? d.ToString() : ObjectHelper.GetFieldValue(d, fieldName).ToString();
                _showNames.Add(str);
            }
            _popUp.SetData(_showNames);
        }

        public void FocusRefresh()
        {
            _popUp.FocusRefresh();
        }
        
        public void SetSelectedIndex(int index)
        {
            _popUp.SetSelectIndex(index);
        }

        public void ScrollToIndex(int index)
        {
            _popUp.ScrollToIndex(index);
        }
        
        private void Awake()
        {
            _clickArea.OnClick(OnClick);
            _popUp.OnData += OnData;
            _popUp.OnSelectedDataRefresh += OnDataSelected;
            _popUp.OnSelectedViewRefresh += OnSelected;
            _popUp.SelectCheckHandler += OnSelectCheck;
            _areaListener.OnClickInArea += InAreaListener;
        }

        private void OnData(int index, RectTransform cell, object arg3)
        {
            if (_showNames.Count == 0)
            {
                _selectText.text = "";
                return;
            }
            var rc = cell.GetComponent<UIReferenceCollector>();
            var title = rc.Get<XText>("title");
            title.text = _showNames[index];

        }

        private void OnDataSelected(object arg3, int index)
        {
            _selectText.text = _showNames[index];
            OnSelect?.Invoke(index, _sourceData[index]);
            Pop(false);
        }
        private void OnSelected(RectTransform cell, bool arg2, object arg3, int index)
        {
            var rc = cell.GetComponent<UIReferenceCollector>();
            var select = rc.Get<XImage>("select");
            select.Display(arg2);
        }

        private bool OnSelectCheck(int arg1, object arg2)
        {
            return SelectCheckHandler?.Invoke(arg1, arg2) ?? false;
        }

        private void Start()
        {
            Pop(false);
            SetSelectedIndex(defaultSelectIndex);
        }

        private void OnClick()
        {
            Pop(!_isShowPopup);
        }

        private void Pop(bool show)
        {
            _isShowPopup = show;
            _popUp.gameObject.Display(show);
            if (_arrowOn != null)
                _arrowOn.Display(show);
            if (_arrowOff != null)
                _arrowOff.Display(!show);
            if (_areaListener != null)
                _areaListener.enabled = show;
        }
        
    }
}