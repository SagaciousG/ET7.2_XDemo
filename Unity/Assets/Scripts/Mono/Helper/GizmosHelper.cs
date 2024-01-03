﻿using UnityEngine;

namespace ET
{
    public static class GizmosHelper
    {
        public static void DrawLine(Vector3 start, Vector3 to, Color color)
        {
#if UNITY_EDITOR
            
            Gizmos.color = color;
            Gizmos.DrawLine(start, to);
            Gizmos.color = Color.white;
#endif
        }
        
        public static void DrawRect(Rect rect, Color color, float height = 0)
        {
#if UNITY_EDITOR
            Gizmos.color = color;
            var p0 = new Vector3(rect.xMin, height, rect.yMin);
            var p1 = new Vector3(rect.xMin, height, rect.yMax);
            var p2 = new Vector3(rect.xMax, height, rect.yMax);
            var p3 = new Vector3(rect.xMax, height, rect.yMin);
            Gizmos.DrawLine(p0, p1);
            Gizmos.DrawLine(p1, p2);
            Gizmos.DrawLine(p2, p3);
            Gizmos.DrawLine(p3, p0);
            Gizmos.color = Color.white;
#endif       
        }

        public static void Draw2DCube(Rect rect, Color color)
        {
#if UNITY_EDITOR
            Gizmos.color = color;
            Gizmos.DrawCube(new Vector3(rect.center.x, 0, rect.center.y), 
                new Vector3(rect.width, 0.01f, rect.height));
            Gizmos.color = Color.white;
#endif
        }
    }
}