using System;
using UnityEditor;
using UnityEngine;

namespace ET
{
    [CustomEditor(typeof(UIClickInRect))]
    public class UIClickInRectEditor : Editor
    {
        private void OnSceneGUI()
        {
            var self = (UIClickInRect)target;
            var pos = self.transform.position;
            var size = self.GetComponent<RectTransform>().sizeDelta;
            size += new Vector2(self.Expand.z + self.Expand.w, self.Expand.x + self.Expand.y);
            var size3d = new Vector3(size.x, size.y, 0.01f);
            var lossyScale = self.transform.lossyScale;
            size3d = Vector3.Scale(size3d, lossyScale);
            var startPos = new Vector3(pos.x + (self.Expand.w -self.Expand.z) / 2 * lossyScale.x, pos.y + (self.Expand.x - self.Expand.y) / 2 * lossyScale.y, 0);
            
            Handles.color = Color.yellow;
            Handles.DrawWireCube(startPos, size3d);
            Handles.color = Color.white;
        }
    }
}