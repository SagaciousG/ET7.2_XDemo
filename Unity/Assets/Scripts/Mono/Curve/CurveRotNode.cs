using System;
using UnityEditor;
using UnityEngine;

namespace ET
{
    public class CurveRotNode : MonoBehaviour
    {
        public CurveRoot Root
        {
            get
            {
                if (_root == null)
                    _root = GetComponentInParent<CurveRoot>();
                return _root;
            }
        }
        private CurveRoot _root;
        
        public float Percent;

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            
            Handles.color = Color.blue;
            Handles.DrawLine(transform.position, transform.position + transform.forward);
            Handles.color = Color.green;
            Handles.DrawLine(transform.position, transform.position + transform.up);
            Handles.color = Color.red;
            Handles.DrawLine(transform.position, transform.position + transform.right);
            Handles.color = Color.white;
#endif
        }
    }
}