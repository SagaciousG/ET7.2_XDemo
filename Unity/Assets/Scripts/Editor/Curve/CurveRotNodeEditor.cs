using UnityEditor;

namespace ET
{
    [CustomEditor(typeof(CurveRotNode))]
    public class CurveRotNodeEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var node = (CurveRotNode) target;
            node.Percent = EditorGUILayout.Slider("位置", node.Percent, 0, 1);
            node.transform.position = node.Root.GetPosition(node.Percent);
        }
    }
}