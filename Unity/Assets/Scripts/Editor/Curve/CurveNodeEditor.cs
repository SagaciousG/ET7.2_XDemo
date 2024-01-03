﻿﻿using System;
 using UnityEditor;
using UnityEngine;
 using Model;

 namespace ET
{
    [CustomEditor(typeof(CurveNode))]
    public class CurveNodeEditor : UnityEditor.Editor
    {
        private CurveNode Node => (CurveNode) target;
        
        private void OnEnable()
        {
            var node = (CurveNode) target;
            if (node.CtrlPoint1 == null)
            {
                node.CtrlPoint1 = new GameObject("node_1").AddComponent<CurveCtrlPoint>();
                node.CtrlPoint1.transform.localPosition = new Vector3(0, 1, 0);
            }

            if (node.CtrlPoint2 == null)
            {
                node.CtrlPoint2 = new GameObject("node_2").AddComponent<CurveCtrlPoint>();
                node.CtrlPoint2.transform.localPosition = new Vector3(0, -1, 0);
            }
        }
    }
}