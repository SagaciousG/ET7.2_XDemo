using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace ET
{
    public class CurveFovNode : MonoBehaviour
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

        public float Fov;

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            
           GUI.color = Color.green;
           Handles.Label(transform.position, $"Fov:{Fov}");
           GUI.color = Color.white;
#endif
        }
    }
}