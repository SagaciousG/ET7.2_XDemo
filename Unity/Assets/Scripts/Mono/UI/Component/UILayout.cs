using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    [ExecuteAlways]
    public class UILayout : MonoBehaviour
    {
        public RectTransform.Axis Direction;
        public float Space;
        public bool AutoSizeX;
        public bool AutoSizeY;

        public Align AlignX;
        public Align AlignY;
        public Vector4 Padding;
        public Vector2 Min;
        private RectTransform _rectTransform;
        private bool _dirty;
        private UILayout _pNode;
        private List<UILayout> _nodes = new();
        private int _childCount;
        private void Awake()
        {
            _rectTransform = (RectTransform)transform;
            ReInit();
        }

        private void ReInit()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                var uiLayout = child.GetComponent<UILayout>();
                if (uiLayout != null)
                {
                    _nodes.Add(uiLayout);
                    uiLayout._pNode = this;
                }
            }

            _childCount = transform.childCount;
        }

        private void Layout()
        {
            foreach (var node in _nodes)
            {
                if (node == null)
                    continue;
                if (!node.enabled)
                    continue;
                node.Layout();
            }

            if (Direction == RectTransform.Axis.Vertical)
            {
                var y = -Padding.x;
                var x = Padding.z;
                var height = 0f;
                var width = 0f;
                for (int i = 0; i < transform.childCount; i++)
                {
                    var child = transform.GetChild(i);
                    if (!child.gameObject.activeSelf)
                        continue;
                    var rt = (RectTransform)child;
                    rt.anchorMin = Vector2.up;
                    rt.anchorMax = Vector2.up;
                    rt.anchoredPosition = new Vector2(x, y);
                    y -= rt.rect.height + Space;
                    height += rt.rect.height + Space;
                    width = Mathf.Max(width, rt.rect.width);
                }

                height -= Space;
                height = Mathf.Max(Min.y, height);
                if (AutoSizeY)
                {
                    height += Padding.y + Padding.x;
                    _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Max(Min.y, height));
                }
                else
                {
                    var y0 = 0f;
                    switch (AlignY)
                    {
                        case Align.Center:
                        {
                            var h = _rectTransform.rect.height - height;
                            y0 = h / 2;
                            break;
                        }
                        case Align.Right:
                        {
                            y0 = _rectTransform.rect.height - height - Padding.y;
                            break;
                        }
                    }

                    if (AlignY != Align.Left)
                    {
                        for (int i = 0; i < transform.childCount; i++)
                        {
                            var child = transform.GetChild(i);
                            var rt = (RectTransform)child;
                            if (!child.gameObject.activeSelf)
                                continue;
                            rt.anchoredPosition += new Vector2(0, -y0);
                        }
                    }
                }

                if (AutoSizeX)
                {
                    width += Padding.w + Padding.z;
                    width = Mathf.Max(width, Min.x);
                    _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
                }
                switch (AlignX)
                {
                    case Align.Center:
                    {
                        for (int i = 0; i < transform.childCount; i++)
                        {
                            var child = transform.GetChild(i);
                            if (!child.gameObject.activeSelf)
                                continue;
                            var rt = (RectTransform)child;
                            var w = rt.rect.width - _rectTransform.rect.width;
                            var x0 = w / 2;
                            rt.anchoredPosition += new Vector2(-x0, 0);
                        }
                        break;
                    }
                    case Align.Right:
                    {
                        for (int i = 0; i < transform.childCount; i++)
                        {
                            var child = transform.GetChild(i);
                            if (!child.gameObject.activeSelf)
                                continue;
                            var rt = (RectTransform)child;
                            var w = rt.rect.width - _rectTransform.rect.width;
                            var x0 = w + Padding.w;
                            rt.anchoredPosition += new Vector2(-x0, 0);
                        }
                        break;
                    }
                }
            }
            else
            {
                var y = -Padding.x;
                var x = Padding.z;
                var width = 0f;
                var height = 0f;
                for (int i = 0; i < transform.childCount; i++)
                {
                    var child = transform.GetChild(i);
                    if (!child.gameObject.activeSelf)
                        continue;
                    var rt = (RectTransform)child;
                    rt.anchorMin = Vector2.up;
                    rt.anchorMax = Vector2.up;
                    rt.anchoredPosition = new Vector2(x, y);
                    x += rt.rect.width + Space;
                    width += rt.rect.width + Space;
                    height = Mathf.Max(height, rt.rect.height);
                }

                width -= Space;
                width = Mathf.Max(width, Min.x);
                if (AutoSizeX)
                {
                    width += Padding.w + Padding.z;
                    width = Mathf.Max(width, Min.x);
                    _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
                }

                if (AutoSizeY)
                {
                    height += Padding.y + Padding.x;
                    height = Mathf.Max(height, Min.x);
                    _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
                }
            }
        }

        private void Update()
        {
            if (_childCount != transform.childCount)
            {
                ReInit();
            }
            if (_pNode != null)
                return;

            Layout();
        }
        
        public enum Align
        {
            Left,
            Center,
            Right,
        }
    }
}