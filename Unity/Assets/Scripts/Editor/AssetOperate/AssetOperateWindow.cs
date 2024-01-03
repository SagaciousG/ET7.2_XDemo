﻿﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace ET
{
    public class AssetOperateWindow : EditorWindow
    {
        [MenuItem("Tools/文件操作工具")]
        static void ShowWindow()
        {
            var win = GetWindow<AssetOperateWindow>();
            win.minSize = new Vector2(500, 500);
            win.Show();
        }
        
        private List<string> _files = new List<string>();
        private List<string> _showFiles = new List<string>();
        
        private Vector2 _scrollPos;
        private string _filter = ".meta";

        private FileOperatorType _operatorType;

        private IFileOperator _current;

        private bool _requireInit;

        private bool _showAll;

        private string _selected;
        private void OnGUI() {
            if (mouseOverWindow == this)
            {
                //鼠标位于当前窗口
                if (Event.current.type == UnityEngine.EventType.DragUpdated)
                {
                    //拖入窗口未松开鼠标
                    DragAndDrop.visualMode = DragAndDropVisualMode.Generic; //改变鼠标外观
                }
                else if (Event.current.type == UnityEngine.EventType.DragExited)
                {
                    //拖入窗口并松开鼠标
                    Focus(); //获取焦点，使unity置顶(在其他窗口的前面)
                    //Rect rect=EditorGUILayout.GetControlRect();
                    //rect.Contains(Event.current.mousePosition);//可以使用鼠标位置判断进入指定区域
                    if (DragAndDrop.paths != null)
                    {
                        int len = DragAndDrop.paths.Length;
                        for (int i = 0; i < len; i++)
                        {
                            var path = DragAndDrop.paths[i];
                            if (Directory.Exists(path))
                            {
                                var files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
                                foreach (var f in files)
                                {
                                    if (f.EndsWith(".meta")) continue;
                                    _files.Add(f.Replace("\\", "/"));
                                }
                            }
                            else
                            {
                                if (path.EndsWith(".meta")) continue;
                                _files.Add(path.Replace("\\", "/"));
                            }
                        }

                        _requireInit = true;
                        _showFiles = _files.GetRange(0, _files.Count);
                    }
                }
            }

            if (_files.Count == 0)
            {
                EditorGUILayout.LabelField("拖入【文件】或【文件夹】");
                return;
            } 
            
            switch (_operatorType)
            {
                case FileOperatorType.Rename:
                    if (!(_current is FileRename))
                    {
                        _current = new FileRename();
                        _showFiles.Clear();
                        _showFiles.AddRange(_files);
                        _current.Init(_files.ToArray(), ref _showFiles);
                    }
                    break;
                case FileOperatorType.AssetsOperator:
                    if (!(_current is AssetsOperator))
                    {
                        _current = new AssetsOperator();
                        _showFiles.Clear();
                        _showFiles.AddRange(_files);
                        _current.Init(_files.ToArray(), ref _showFiles);
                    }
                    break;
            }

            if (_requireInit)
            {
                _requireInit = false;
                _current.Init(_files.ToArray(), ref _showFiles);
            }

            _current.Selected = _selected;
            
            var space = 5;
            var areaHeight = new[] {25, 25, 200};
            
            
            GUILayout.BeginArea(new Rect(0, 0, position.width, areaHeight[0]), "", "box");
            {
                TopArea();
            }
            GUILayout.EndArea();
            
            GUILayout.BeginArea(new Rect(0, areaHeight[0] + space, position.width, areaHeight[1]), "", "box");
            {
                TopArea2();
            }
            GUILayout.EndArea();
            
            GUILayout.BeginArea(new Rect(0, areaHeight[0] + areaHeight[1] + space * 2, position.width, 
                position.height - (areaHeight[0] + areaHeight[1] + areaHeight[2] + space * 3)), "", "box");
            {
                CenterArea();
            }
            GUILayout.EndArea();
            
            GUILayout.BeginArea(new Rect(0, position.height - areaHeight[2], position.width, areaHeight[2]), "", "box");
            {
                BotArea();
            }
            GUILayout.EndArea();
        }
        
        private void TopArea()
        {
            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("清除", GUILayout.Width(100)))
                {
                    _files.Clear();
                }
                _operatorType = (FileOperatorType)EditorGUILayout.EnumPopup("选择操作", _operatorType);
                
            }
            GUILayout.EndHorizontal();
        }

        private void TopArea2()
        {
            GUILayout.BeginHorizontal();
            {
                _current.Top();
            }
            GUILayout.EndHorizontal();
        }
        
        private void CenterArea()
        {
            EditorGUILayout.BeginHorizontal();
            
            if (GUILayout.Button("列表", _showAll ? "AnimClipToolbarButton" : "minibuttonmid", GUILayout.Width(100)))
            {
                _showAll = false;
            }

            if (GUILayout.Button("所有文件", !_showAll ? "AnimClipToolbarButton" : "minibuttonmid", GUILayout.Width(100)))
            {
                _showAll = true;
            }

            List<string> list = _showAll ? _files : _showFiles;
            EditorGUILayout.EndHorizontal();
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
            {
                for (int i = 0; i < list.Count; i++)
                {
                    DrawItem(list[i], i);
                }
            }
            EditorGUILayout.EndScrollView();
        }

        private void DrawItem(string path, int index)
        {
            EditorGUILayout.BeginHorizontal();
            {
                GUI.color = _selected == path ? Color.green : Color.white;
                if (GUILayout.Button(path, "LargeButtonLeft", GUILayout.Width(position.width * 0.9f)))
                {
                    _selected = _selected == path ? "" : path;
                }
                if (GUILayout.Button("X", GUILayout.Width(25), GUILayout.Height(25)))
                {
                    _showFiles.Remove(path);
                    _files.Remove(path);
                    _requireInit = true;
                }

                GUI.color = Color.white;
            }
            EditorGUILayout.EndHorizontal();
        }
        
        public enum FileOperatorType
        {
            Rename,
            AssetsOperator,
        }
        
        private void BotArea()
        {
            EditorGUILayout.BeginVertical();
            _current.Bot();
            EditorGUILayout.EndVertical();
        }
        
    }
}