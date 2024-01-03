using UnityEditor;

namespace ET
{
    [CustomEditor(typeof(CurveFovNode))]
    public class CurveFovNodeEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var node = (CurveFovNode) target;
            node.Percent = EditorGUILayout.Slider("位置", node.Percent, 0, 1);
            node.transform.position = node.Root.GetPosition(node.Percent);
            node.Fov = EditorGUILayout.FloatField("Fov", node.Fov);
        }
    }
}