using UnityEngine;

namespace ET.Client
{
    public static class RectTransformHelper
    {
        public static Vector2 TransferLocalPos(RectTransform self, RectTransform content)
        {
            var screenPoint = RectTransformUtility.WorldToScreenPoint(Init.Instance.UICamera, self.position);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(content, screenPoint, Init.Instance.UICamera,
                out var localPoint);
            return localPoint;
        }
    }
}