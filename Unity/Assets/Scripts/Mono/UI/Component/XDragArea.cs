using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ET
{
    public class XDragArea : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        public event Action<Vector3> OnBegin;
        public event Action<Vector3> OnDraging;
        public event Action<Vector3> OnEnd;

        public void OnBeginDrag(PointerEventData eventData)
        {
            this.OnBegin?.Invoke(eventData.position);
        }

        public void OnDrag(PointerEventData eventData)
        {
            this.OnDraging?.Invoke(eventData.position);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            this.OnEnd?.Invoke(eventData.position);
        }

    }
}