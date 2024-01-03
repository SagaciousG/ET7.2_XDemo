using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace ET
{
    public class XText : TextMeshProUGUI
    {
        [SerializeField]
        private AutoSize _sizeFitter;

        public override void Rebuild(CanvasUpdate update)
        {
            switch (_sizeFitter)
            {
                case AutoSize.Horizontal:
                {
                    var size = GetPreferredValues(float.MaxValue, rectTransform.rect.height);
                    rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
                    break;
                }
                case AutoSize.Vertical:
                {
                    var size = GetPreferredValues(rectTransform.rect.width, float.MaxValue);
                    rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
                    break;
                }
            }    
            base.Rebuild(update);
        }
        
        public enum AutoSize
        {
            None,
            Vertical,
            Horizontal,
        }
    }
}