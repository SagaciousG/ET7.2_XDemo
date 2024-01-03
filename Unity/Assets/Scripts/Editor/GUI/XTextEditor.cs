using TMPro;
using TMPro.EditorUtilities;
using UnityEditor;

namespace ET
{
    [CustomEditor(typeof(XText))]
    public class XTextEditor : TMP_EditorPanelUI
    {
        private SerializedProperty _autoSize;
        protected override void OnEnable()
        {
            base.OnEnable();
            _autoSize = serializedObject.FindProperty("_sizeFitter");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(_autoSize);
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
            base.OnInspectorGUI();
        }
    }
}