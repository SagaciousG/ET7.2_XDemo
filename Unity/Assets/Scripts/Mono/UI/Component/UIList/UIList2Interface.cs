﻿using System;
using System.Collections;
 using ET;
 using UnityEngine;

namespace ET
{
    public partial class UIList
    {
        //不通过返回true，通过返回false
        public event Func<int, object, bool> SelectCheckHandler;
        public event Action<int, RectTransform, object> OnData;
        public event Action<RectTransform, bool, object, int> OnSelectedViewRefresh;
        public event Action<object, int> OnSelectedDataRefresh;
        public event Action<RectTransform, int> OnCellFetch; 
        public event Action<int> OnCellCollect; 
        public int SelectedIndex => _selectedIndex;
        public int DataNum => _dataNum;
        public object SelectedData => _data[_selectedIndex];
        public RectTransform Content => _content;
        public RectTransform Viewport => _viewport;
        public RectTransform RenderCell => _templete;
        public bool IsPrefabAsset => _isPrefabAsset;
        public float CellScale => _cellScale;
        public Vector4 Padding => _padding;
        
        public Align Direction => _align;
        
        public int AlignNum
        {
            get => _alignNum;
            set
            {
                if (_alignNum == value)
                    return;
                _alignNum = value;
                UpdateLayout();
            }
        }

        public float SpaceX
        {
            get => _spaceX;
            set
            {
                if (_spaceX == value)
                    return;
                _spaceX = value;
                UpdateLayout();
            }
        }

        public float SpaceY
        {
            get => _spaceY;
            set
            {
                if (_spaceY == value)
                    return;
                _spaceY = value;
                UpdateLayout();
            }
        }

        public ListLayout Layout
        {
            set
            {
                if (_layout == value)
                    return;
                _layout = value;
                UpdateLayout();
            }
            get => _layout;
        }

        public CenterType AutoCenter
        {
            set
            {
                if (_autoCenter == value)
                    return;
                _autoCenter = value;
                UpdateLayout();
            }
            get => _autoCenter;
        }
        
        public void SetData(IList data)
        {
            _data = data;
            _dataNum = data?.Count ?? 0;
            if (_selectedIndex >= _dataNum)
            {
                _selectedIndex = -1;
            }
            _curSelected = -1;
            UpdateLayout();
        }
        
        public void FocusRefresh()
        {
            _curSelected = -1;
            UpdateLayout();
        }

        
        public void ScrollToIndex(int index, float speed = 0.5f)
        {
            if (index < 0)
                return;
            index = Mathf.Clamp(index, 0, int.MaxValue);
            if (Mathf.Clamp(index, _curFirstIndex, _curFirstIndex + _maxViewCellCount - 1) == index)
                return;
            _scrolling = true;
            _scrollSpeed = speed;
            if (!_start)
                return;
            
            Vector2Int point = Vector2Int.zero;
            var size = _templete.rect.size;
            switch (_align)
            {
                case Align.Horizontal:
                    point.x = index % _alignNum;
                    point.y = index / _alignNum;
                    break;
                case Align.Vertical:
                    point.x = index / _alignNum;
                    point.y = index % _alignNum;
                    break;
            }

            var x = point.x * size.x + point.x * _spaceX + _padding.z;
            var y = point.y * size.y + point.y * _spaceY + _padding.x;
            var maxX = Mathf.Clamp(_content.rect.width - _viewport.rect.width, 0, int.MaxValue);
            var maxY = Mathf.Clamp(_content.rect.height - _viewport.rect.height, 0, int.MaxValue);
            x = Mathf.Clamp(x, -maxX, 0);
            y = Mathf.Clamp(y, 0, maxY);
            _toPoint = new Vector2(x, y);
        }
        
        public void SetSelectIndex(int index, bool scrollTo = true)
        {
            if (_data == null)
            {
                _selectedIndex = index;
                if (scrollTo)
                    ScrollToIndex(index);
                return;
            }
            if (index < -1 || index >= _data.Count)
                return;
            if (_selectedIndex == index && _curSelected == _selectedIndex)
                return;
            if (SelectCheckHandler?.Invoke(index, _data[index]) ?? false)
                return;
            var lastIndex = _selectedIndex;
            _selectedIndex = index;

            if (index > -1 && index < _data.Count)
            {
                _curSelected = _selectedIndex;
                OnSelectedDataRefresh?.Invoke(_data[index], _selectedIndex);
            }
            
            if (_dataIndex2Cell.TryGetValue(_selectedIndex, out var cell))
            {
                if (index > -1 && index < _data.Count)
                    OnSelectedViewRefresh?.Invoke(cell, true, _data[index], _selectedIndex);
            }

            if (_dataIndex2Cell.TryGetValue(lastIndex, out var last))
            {
                if (index > -1 && index < _data.Count)
                    OnSelectedViewRefresh?.Invoke(last, lastIndex == index, _data[lastIndex], lastIndex);
                else
                    OnSelectedViewRefresh?.Invoke(last, lastIndex == index, null, lastIndex);
            }
            
            if (scrollTo)
                ScrollToIndex(index);
        }
        
        public void UpdateLayout()
        {
            OnLayoutChange();
            if (_start)
            {
                UpdateContentSize();
                OnCenterTypeChange();
                UpdateMaxFirstCellIndex();
                UpdateMaxViewCellCount();
                UpdateViewFirstCellIndex();
                UpdateCellCount(true);
            }
            SetSelectIndex(_selectedIndex, _scrolling);
        }

        public void UpdateViewCells()
        {
            foreach (var kv in _dataIndex2Cell)
            {
                if (kv.Key < _data.Count)
                    OnData?.Invoke(kv.Key, kv.Value, _data[kv.Key]);
            }
        }

        public void Dispose()
        {
            foreach (var cell in _poolCells)
            {
                Destroy(cell);
            }

            foreach (var cell in _dataIndex2Cell.Values)
            {
                Destroy(cell);
            }
            _poolCells.Clear();
            _dataIndex2Cell.Clear();
            _data.Clear();
            _selectedIndex = -1;
            _curSelected = -1;
        }
    }
    
}