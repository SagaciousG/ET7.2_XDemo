using System;
using UnityEditor;
using UnityEngine;
using ET;

[CustomEditor(typeof (UIList))]
public class UIListEditor: UnityEditor.Editor
{
    private SerializedProperty _padding;
    private SerializedProperty _demoNum;
    private SerializedProperty _content;
    private SerializedProperty _viewport;
    private SerializedProperty _templete;
    private SerializedProperty _align;
    private SerializedProperty _alignNum;
    private SerializedProperty _spaceX;
    private SerializedProperty _spaceY;
    private SerializedProperty _layout;
    private SerializedProperty _autoCenter;
    private SerializedProperty _cellScale;
    private SerializedProperty _isPrefabAsset;

    private bool _hideGizmos;

    private void OnEnable()
    {
        var list = (UIList) target;
        _padding = serializedObject.FindProperty("_padding");
        _demoNum = serializedObject.FindProperty("_demoNum");
        _content = serializedObject.FindProperty("_content");
        _viewport = serializedObject.FindProperty("_viewport");
        _templete = serializedObject.FindProperty("_templete");
        _align = serializedObject.FindProperty("_align");
        _alignNum = serializedObject.FindProperty("_alignNum");
        _spaceX = serializedObject.FindProperty("_spaceX");
        _spaceY = serializedObject.FindProperty("_spaceY");
        _layout = serializedObject.FindProperty("_layout");
        _autoCenter = serializedObject.FindProperty("_autoCenter");
        _cellScale = serializedObject.FindProperty("_cellScale");
        _isPrefabAsset = serializedObject.FindProperty("_isPrefabAsset");
    }


    public override void OnInspectorGUI()
    {
        var list = (UIList) target;
        EditorGUILayout.PropertyField(_demoNum);
        EditorGUILayout.LabelField("DataNum", list.DataNum.ToString());
        EditorGUILayout.PropertyField(_autoCenter);
        EditorGUILayout.PropertyField(_content);
        EditorGUILayout.PropertyField(_viewport);
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(_templete, new GUIContent("CellRender"));
        if (EditorGUI.EndChangeCheck())
        {
            if (_templete.objectReferenceValue == null)
                _isPrefabAsset.boolValue = false;
            else
            {
                var obj = ((RectTransform)_templete.objectReferenceValue).gameObject;
                var assetType = PrefabUtility.GetPrefabAssetType(obj);
                _isPrefabAsset.boolValue = assetType != PrefabAssetType.NotAPrefab;
            }
        }

        GUI.enabled = false;
        if (_templete.objectReferenceValue != null)
            EditorGUILayout.Vector2Field("CellSize", ((RectTransform)_templete.objectReferenceValue).sizeDelta);
        GUI.enabled = true;
        EditorGUILayout.PropertyField(_cellScale, new GUIContent("CellScale"));
        
        EditorGUILayout.PropertyField(_padding, new GUIContent("Padding", "上下左右"));
        EditorGUILayout.PropertyField(_layout);

        switch (list.Layout)
        {
            case UIList.ListLayout.Grid:
                EditorGUILayout.PropertyField(_align);
                switch (_align.enumValueIndex)
                {
                    case 0:
                        EditorGUILayout.PropertyField(_alignNum, new GUIContent("ColCount"));
                        break;
                    case 1:
                        EditorGUILayout.PropertyField(_alignNum, new GUIContent("RawCount"));
                        break;
                }

                EditorGUILayout.PropertyField(_spaceX);
                EditorGUILayout.PropertyField(_spaceY);
                break;
            case UIList.ListLayout.Horizontal:
                EditorGUILayout.PropertyField(_spaceX);
                break;
            case UIList.ListLayout.Vertical:
                EditorGUILayout.PropertyField(_spaceY);
                break;
        }

        if (serializedObject.hasModifiedProperties)
        {
            serializedObject.ApplyModifiedProperties();
            list.UpdateLayout();
        }

        _hideGizmos = EditorGUILayout.Toggle("HideGizmos", _hideGizmos);
    }
    
    private void OnSceneGUI()
    {
        if (Application.isPlaying)
            return;
        if (_hideGizmos)
            return;
        var list = (UIList) target;
        if (list.Content == null)
            return;
        if (list.RenderCell == null)
            return;
        if (list.AlignNum == 0)
            return;
        var size = list.CellScale * list.RenderCell.sizeDelta;
        var pos = list.Content.position;
        Handles.color = Color.green;
        var size3d = new Vector3(size.x, size.y, 0.01f);
        size3d = Vector3.Scale(size3d, list.transform.lossyScale);
        var startPos = new Vector3(pos.x + size3d.x / 2, pos.y - size3d.y / 2, 0) + new Vector3(list.Padding.z, list.Padding.x * -1);
        for (int i = 0; i < this._demoNum.intValue; i++)
        {
            var col = i % list.AlignNum; //行
            var raw = Mathf.FloorToInt(i * 1f / list.AlignNum);
            if (list.Direction == UIList.Align.Vertical)
            {
                col = Mathf.FloorToInt(i * 1f / list.AlignNum);
                raw = i % list.AlignNum;
            }
            var realPos = startPos + new Vector3(col * size3d.x + col * list.SpaceX, -1 * raw * size3d.y - raw * list.SpaceY, 0);
            Handles.DrawWireCube(realPos, size3d);
            var half = new Vector3(size3d.x / 2, size3d.y / 2);
            Handles.DrawLine(realPos - half, realPos + size3d - half);
            Handles.DrawLine(new Vector3(realPos.x, (realPos + size3d).y) - half, new Vector3((realPos + size3d).x, realPos.y) - half);
        }

        var c = Mathf.FloorToInt(_demoNum.intValue * 1f / list.AlignNum);
        var r = list.AlignNum;
        if (list.Direction == UIList.Align.Horizontal)
        {
            (c, r) = (r, c);
        }
        var w = c * size3d.x + c * list.SpaceX;
        var h = r * size3d.y * -1 - r * list.SpaceY;
        
        var realStart = startPos + new Vector3((w - size3d.x + list.Padding.w - list.SpaceX - list.Padding.z) / 2, (h + size3d.y - list.Padding.y + list.SpaceY + list.Padding.x) / 2, 0);
        Handles.color = Color.yellow;
        Handles.DrawWireCube(realStart, new Vector3(w + list.Padding.w + list.Padding.z - list.SpaceX, h - list.Padding.y - list.Padding.x + list.SpaceY));
        Handles.color = Color.white;
    }
}
