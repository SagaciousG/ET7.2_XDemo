﻿using UnityEditor;
using UnityEngine;

namespace ET
{
    public class CurveCtrlPoint : MonoBehaviour
    {
        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        public Vector3 LocalPosition
        {
            get => transform.localPosition;
            set => transform.localPosition = value;
        }

        public void DrawGizmos()
        {
#if UNITY_EDITOR
            Gizmos.DrawWireCube(transform.position, Vector3.one * 0.3f); 
#endif
        }
    }
}