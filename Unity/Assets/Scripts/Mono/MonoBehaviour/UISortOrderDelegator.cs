using System;
using UnityEngine;

namespace ET
{
    [RequireComponent(typeof(Canvas))]
    public class UISortOrderDelegator : MonoBehaviour
    {
        public int relativeOrder;

        private void Start()
        {
            gameObject.layer = LayerMask.NameToLayer("UI");
            var pC = transform.parent.GetComponentInParent<Canvas>();
            GetComponent<Canvas>().overrideSorting = true;
            GetComponent<Canvas>().sortingOrder = pC.sortingOrder + relativeOrder;
        }
    }
}