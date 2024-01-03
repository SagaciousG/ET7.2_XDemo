using System;
using UnityEngine;

namespace ET
{
    public class UIClickInRect : MonoBehaviour
    {
        public event Action<UIClickInRect, bool> OnClickInArea;
        public event Action<UIClickInRect> OnHide;
        public Vector4 Expand;
        public bool ClickOutsideHidden;
        public bool ClickUIOnly;
        
        private Canvas _canvas => gameObject.GetComponentInParent<Canvas>();

        private void OnEnable()
        {
            if (ClickUIOnly)
                InputComponent.Instance.AddUIOnlyListener(OnClick);
            else
                InputComponent.Instance.AddListener(OnClick);
        }

        private void OnDisable()
        {
            InputComponent.Instance.RemoveListener(OnClick);
        }

        private void OnClick(InputData data, object args)
        {
            if (data.eventType == InputEventType.Click)
            {
                var rectTransform = GetComponent<RectTransform>();
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    rectTransform, data.position,
                    _canvas.worldCamera, out var localPoint);
                var inArea = rectTransform.rect.Resize(-Expand.z, -Expand.y, Expand.z + Expand.w, Expand.x + Expand.y).Contains(localPoint);
                if (inArea)
                {
                    
                }
                else
                {
                    if (this.ClickOutsideHidden)
                    {
                        gameObject.SetActive(false);
                        OnHide?.Invoke(this);
                    }
                }
                OnClickInArea?.Invoke(this, inArea);
            }
        }
        
    }
}