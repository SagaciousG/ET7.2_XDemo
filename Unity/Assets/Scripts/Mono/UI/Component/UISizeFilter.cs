using UnityEngine;

namespace ET
{
    [ExecuteAlways]
    public class UISizeFilter : MonoBehaviour
    {
        public float horizontal;
        public float vertical;

        public RectTransform target;

        private RectTransform _self;

        private void Awake()
        {
            _self = gameObject.GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            UpdateImmediately();
        }

        public void UpdateImmediately()
        {
            if (target == null)
                return;
            LateUpdate();
        }
        
        private void LateUpdate()
        {
            if (target != null)
            {
                _self.sizeDelta = new Vector2(target.sizeDelta.x + horizontal, target.sizeDelta.y + vertical);
            }
        }
    }
}