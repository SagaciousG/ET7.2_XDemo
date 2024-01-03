using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using ET;
using UnityEditor.AnimatedValues;
using UnityEngine.UI;

[CustomEditor(typeof(XImage))]
[CanEditMultipleObjects]
public class XImageEditor : ImageEditor
{
    private SerializedProperty _spriteAsset;
    private SerializedProperty _flip;
    
    SerializedProperty _mType;
    SerializedProperty _mSprite;
    SerializedProperty _mPreserveAspect;
    SerializedProperty _mUseSpriteMesh;
    AnimBool _mShowSlicedOrTiled;
    AnimBool _mShowSliced;
    AnimBool _mShowTiled;
    AnimBool _mShowFilled;
    AnimBool _mShowType;
    
    protected override void OnEnable()
    {
        base.OnEnable();
        
        _flip = serializedObject.FindProperty("_flip");
        
        _mSprite                = serializedObject.FindProperty("m_Sprite");
        _mType                  = serializedObject.FindProperty("m_Type");
        _mPreserveAspect        = serializedObject.FindProperty("m_PreserveAspect");
        _mUseSpriteMesh         = serializedObject.FindProperty("m_UseSpriteMesh");
        
        var typeEnum = (Image.Type)_mType.enumValueIndex;
        
        _mShowSlicedOrTiled = new AnimBool(!_mType.hasMultipleDifferentValues && typeEnum == Image.Type.Sliced);
        _mShowSliced = new AnimBool(!_mType.hasMultipleDifferentValues && typeEnum == Image.Type.Sliced);
        _mShowTiled = new AnimBool(!_mType.hasMultipleDifferentValues && typeEnum == Image.Type.Tiled);
        _mShowFilled = new AnimBool(!_mType.hasMultipleDifferentValues && typeEnum == Image.Type.Filled);
        _mShowType = new AnimBool(_mSprite.objectReferenceValue != null);
        _mShowSlicedOrTiled.valueChanged.AddListener(Repaint);
        _mShowSliced.valueChanged.AddListener(Repaint);
        _mShowTiled.valueChanged.AddListener(Repaint);
        _mShowFilled.valueChanged.AddListener(Repaint);
        _mShowType.valueChanged.AddListener(Repaint);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(_flip);
        
        SpriteGUI();
        AppearanceControlsGUI();
        RaycastControlsGUI();
        MaskableControlsGUI();

        _mShowType.target = _mSprite.objectReferenceValue != null;
        if (EditorGUILayout.BeginFadeGroup(_mShowType.faded))
            TypeGUI();
        EditorGUILayout.EndFadeGroup();

        SetShowNativeSize(false);
        if (EditorGUILayout.BeginFadeGroup(m_ShowNativeSize.faded))
        {
            EditorGUI.indentLevel++;

            if ((Image.Type)_mType.enumValueIndex == Image.Type.Simple)
                EditorGUILayout.PropertyField(_mUseSpriteMesh);

            EditorGUILayout.PropertyField(_mPreserveAspect);
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFadeGroup();
        NativeSizeButtonGUI();

        if (serializedObject.hasModifiedProperties)
        {
            serializedObject.ApplyModifiedProperties();
        }
        // base.OnInspectorGUI();

    }
    
    void SetShowNativeSize(bool instant)
    {
        Image.Type type = (Image.Type)_mType.enumValueIndex;
        bool showNativeSize = (type == Image.Type.Simple || type == Image.Type.Filled) && _mSprite.objectReferenceValue != null;
        base.SetShowNativeSize(showNativeSize, instant);
    }

    public override bool HasPreviewGUI()
    {
        return base.HasPreviewGUI();
    }

    public override void OnPreviewGUI(Rect rect, GUIStyle background)
    {
        base.OnPreviewGUI(rect, background);
    }
}
