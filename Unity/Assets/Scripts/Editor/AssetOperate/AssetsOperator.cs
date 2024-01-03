﻿﻿using System;
using System.Collections.Generic;
using System.IO;
using ET;
using UnityEditor;
using UnityEngine;

namespace ET
{
    public class AssetsOperator : IFileOperator
    {
        private List<UnityEngine.Object> _objs;
        private List<string> _showFiles;
        private List<string> _showFilesBasic;
        private Dictionary<string, Type> _assetTypes;
        private List<string> _typeNames;
        private List<int> _showIndex;

        private string _extension;

        private int _selectIndex;
        private int _selectIndex2;

        private Vector2 _scrollRect;
        private AssetOperatorType _operatorType;
        private string _toFolder;
        private MoveFileOption _moveFileOption;
        private bool _moveMeta;
        public string Selected { get; set; }

        public void Init(string[] allFiles, ref List<string> showFiles)
        {
            _objs = new List<UnityEngine.Object>();
            _showFiles = showFiles;
            _showFilesBasic = showFiles.GetRange(0, showFiles.Count);
            _assetTypes = new Dictionary<string, Type>();
            _typeNames = new List<string>(){"All", "ByExtension"};
            _showIndex = new List<int>();
            for (int i = 0; i < showFiles.Count; i++)
            {
                var p = showFiles[i];
                var o = AssetDatabase.LoadAssetAtPath(p, typeof(Object));
                if (o == null)
                {
                    showFiles.Remove(p);
                    continue;
                }
                _objs.Add(o);
                var oType = o.GetType();
                if (!_assetTypes.ContainsKey(oType.Name))
                {
                    _assetTypes.Add(oType.Name, oType);
                    _typeNames.Add(oType.Name);
                }
            }
        }

        public void Top()
        {
            bool showIndexChanged = false;
            EditorGUI.BeginChangeCheck();
            _selectIndex = EditorGUILayout.Popup("类型筛选", _selectIndex, _typeNames.ToArray());
            if (_selectIndex >= _typeNames.Count)
                _selectIndex = 0;
            if (EditorGUI.EndChangeCheck())
            {
                _showIndex.Clear();
                if (_selectIndex == 0)
                {
                    for (int i = 0; i < _objs.Count; i++)
                    {
                        _showIndex.Add(i);
                    }
                }
                else if (_selectIndex == 1)
                {
                    for (int i = 0; i < _objs.Count; i++)
                    {
                        if (_extension.IsNullOrEmpty())
                        {
                            _showIndex.Add(i);
                        }
                        else
                        {
                            if (Path.GetExtension(_showFilesBasic[i]).Trim('.').ToLower().Contains(_extension.Trim('.').ToLower()))
                            {
                                _showIndex.Add(i);
                            }
                        }
                    }
                }
                else
                {
                    var selectType = _assetTypes[_typeNames[_selectIndex]];
                    
                    for (int i = 0; i < _objs.Count; i++)
                    {
                        if (_objs[i].GetType() == selectType)
                        {
                            _showIndex.Add(i);
                        }
                    }
                }

                showIndexChanged = true;

            }

            if (_selectIndex == 1)
            {
                EditorGUI.BeginChangeCheck();
                _extension = EditorGUILayout.TextField("扩展名", _extension);
                if (EditorGUI.EndChangeCheck())
                {
                    _showIndex.Clear();
                    for (int i = 0; i < _objs.Count; i++)
                    {
                        if (Path.GetExtension(_showFilesBasic[i]).Trim('.').ToLower().Contains(_extension.Trim('.').ToLower()))
                        {
                            _showIndex.Add(i);
                        }
                    }

                    showIndexChanged = true;
                }
                
            }
            
            if (_selectIndex == 0)
            {
                
            }
            else if (_selectIndex == 1)
            {
                
            }
            else
            {
                var selectType = _assetTypes[_typeNames[_selectIndex]];
                if (selectType == typeof(GameObject))
                {
                    var componentTypes = new List<Type>();
                    foreach (var i in _showIndex)
                    {
                        var obj = (GameObject)_objs[i];
                        
                    }
                }
            }

            if (showIndexChanged)
            {
                _showFiles.Clear();
                foreach (var i in _showIndex)
                {
                    _showFiles.Add(_showFilesBasic[i]);
                }
            }
        }

        public void Bot()
        {
            _operatorType = (AssetOperatorType) EditorGUILayout.EnumPopup("操作类型", _operatorType);
            switch (_operatorType)
            {
                case AssetOperatorType.GameObject:
                    DrawGameObject();
                    break;
                case AssetOperatorType.MoveFile:
                    DrawMoveFile();
                    break;
            }
        }

        private void DrawGameObject()
        {
            EditorGUILayout.LabelField("修改游戏物体");
            if (string.IsNullOrEmpty(Selected))
                return;
            var selectIndex= _showFilesBasic.IndexOf(Selected);
            if (selectIndex == -1) return;
            var selectObj = _objs[selectIndex];
            GUI.enabled = false;
            EditorGUILayout.ObjectField("资源", selectObj, typeof(Object), false);
            GUI.enabled = true;
            if (selectObj is GameObject gameObject)
            {
                _scrollRect = EditorGUILayout.BeginScrollView(_scrollRect, GUILayout.Height(130));
                AssetTypeDrawer.Draw(gameObject);
                EditorGUILayout.EndScrollView();
                EditorGUILayout.BeginHorizontal();
                var types = gameObject.GetComponentsInChildren<Component>();
                var typeNames = new List<string>();
                var typeList = new List<Type>();
                foreach (var component in types)
                {
                    if (!typeList.Contains(component.GetType()))
                    {
                        typeList.Add(component.GetType());
                        typeNames.Add(component.GetType().Name);
                    }
                }

                _selectIndex2 = EditorGUILayout.Popup("组件类型", _selectIndex2, typeNames.ToArray());
                Type selectType = null;
                if (typeList.ContainKey(_selectIndex2))
                {
                    selectType = typeList[_selectIndex2];
                }

                if (GUILayout.Button("移除组件"))
                {
                    if (selectType == null) return;
                    var children = gameObject.FindAllChildren();
                    foreach (var child in children)
                    {
                        
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        private void DrawMoveFile()
        {
            EditorGUILayout.LabelField("移动文件");
            GUILayoutHelper.EnumPopup("移动操作", ref _moveFileOption);
            _toFolder = EditorGUILayout.TextField("移动至", _toFolder);
            _moveMeta = EditorGUILayout.Toggle("移动Meta", _moveMeta);
            if (!string.IsNullOrEmpty(Selected))
            {
                var selectIndex= _showFilesBasic.IndexOf(Selected);
                if (selectIndex != -1)
                {
                    EditorGUILayout.LabelField("预览");
                    EditorGUILayout.LabelField(_showFilesBasic[selectIndex]);
                    EditorGUILayout.LabelField("移动后");
                    EditorGUILayout.LabelField(GetMovedPath(_showFilesBasic[selectIndex]));
                }
            }
            
            if (GUILayout.Button("Done"))
            {
                foreach (var showFile in _showFiles)
                {
                    if (!Directory.Exists(Path.GetDirectoryName(GetMovedPath(showFile))))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(GetMovedPath(showFile)));
                    }
                    File.Move(showFile, GetMovedPath(showFile));
                    if (_moveMeta)
                    {
                        File.Move($"{showFile}.meta", GetMovedPath($"{showFile}.meta"));
                    }
                }
                _showFiles.Clear();
                AssetDatabase.Refresh();
            }
        }

        private string GetMovedPath(string file)
        {
            _toFolder ??= "";
            switch (_moveFileOption)
            {
                case MoveFileOption.KeepTree:
                {
                    var relative = file.Substring(file.IndexOf("Assets", StringComparison.Ordinal) + 6);
                    return _toFolder.Trim('/').Trim('\\') + relative;
                }
                case MoveFileOption.OneFolder:
                {
                    return _toFolder + "/" + Path.GetFileName(file);
                    break;
                }
            }

            return file;
        }
        
        private enum AssetOperatorType
        {
            GameObject,
            MoveFile,
        }
        
        private enum MoveFileOption
        {
            [Name("移动至同一文件夹")]
            OneFolder,
            [Name("保留目录结构")]
            KeepTree,
        }
    }
}