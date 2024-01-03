﻿﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
  using ET;
  using UnityEditor;
using UnityEngine;

namespace ET
{
    public enum FileRenameRule
    {
        Regex,
        Common
    }
        
    public enum RenameOptions
    {
        AddFront,
        AddEnd,
        RenameInOrder,
        AddFolderAtFront,
        ToLower,
    }

    public enum FolderLevel
    {
        First,
        Second,
    }



    public class FileRename : IFileOperator
    {
        
        private List<string> _files;
        private List<string> _showFiles;
        private string _fileRename;
        private string _regex;
        private string _namePreview;

        private bool _validForMeta = true;
        
        private FileRenameRule _rule;
        private RenameOptions _renameOptions;
        private Filter _filter;
        private FolderLevel _folderLevel;

        public string Selected { get; set; }

        public void Init(string[] allFiles, ref List<string> showFiles)
        {
            _files = new();
            _files.AddRange(showFiles);
            _showFiles = new();
            _showFiles.AddRange(showFiles);
            showFiles = _showFiles;
        }
        
        public void Top()
        {
            EditorGUI.BeginChangeCheck();
            GUILayoutHelper.EnumPopup("筛选", ref _filter);
            if (EditorGUI.EndChangeCheck())
            {
                _showFiles.Clear();
                switch (_filter)
                {
                    case Filter.All:
                        _showFiles.AddRange(_files);
                        break;
                    case Filter.SameName:
                    {
                        var map = new MultiMap<string, string>();
                        foreach (var file in _files)
                        {
                            var name = Path.GetFileNameWithoutExtension(file);
                            map.Add(name, file);
                        }

                        foreach (var key in map.Keys)
                        {
                            if (map[key].Count > 1)
                            {
                                _showFiles.AddRange(map[key]);
                            }
                        }
                        break;
                    }
                }
            }
        }
        
        public void Bot()
        {
            EditorGUILayout.BeginHorizontal();
            _validForMeta = EditorGUILayout.Toggle("同时操作meta文件", _validForMeta);
            EditorGUILayout.EndHorizontal();
            DrawRename();
        }
        
        private void DrawRename()
        {
            EditorGUILayout.BeginHorizontal();
            _rule = (FileRenameRule) EditorGUILayout.EnumPopup("使用规则", _rule);
            switch (_rule)
            {
                case FileRenameRule.Common:
                    _renameOptions = (RenameOptions) EditorGUILayout.EnumPopup("选择操作", _renameOptions);
                    break;
            }
            EditorGUILayout.EndHorizontal();
            
            switch (_rule)
            {
                case FileRenameRule.Regex:
                    DrawRegex();
                    break;
                case FileRenameRule.Common:
                    DrawCommonRename();
                    break;
            }
            
        }

        private void DrawRegex()
        {
            EditorGUILayout.BeginHorizontal();
            {
                _regex = EditorGUILayout.TextField("查找", _regex);
                _fileRename = EditorGUILayout.TextField("替换", _fileRename);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.HelpBox("使用正则匹配规则对文件名进行操作(不含扩展名)", MessageType.Info);
            Preview();
            Rename();
        }
        
        private void DrawCommonRename()
        {
            EditorGUILayout.BeginHorizontal();
            string label = "";
            switch (_renameOptions)
            {
                case RenameOptions.AddEnd:
                    label = "附加到末尾";
                    _fileRename = EditorGUILayout.TextField(label, _fileRename);
                    break;
                case RenameOptions.AddFront:
                    label = "附加到开头";
                    _fileRename = EditorGUILayout.TextField(label, _fileRename);
                    break;
                case RenameOptions.RenameInOrder:
                    label = "在尾部添加序号";
                    break;
                case RenameOptions.AddFolderAtFront:
                    label = "在开头添加文件夹名";
                    _folderLevel = (FolderLevel)EditorGUILayout.EnumPopup("文件夹层级", _folderLevel);
                    break;
                case RenameOptions.ToLower:
                    label = "转小写";
                    break;
            }
            EditorGUILayout.EndHorizontal();
            Preview();
            Rename();
        }

        private string GetMatch(string input, string pattern, string replacement)
        {
            try
            {
                if (!string.IsNullOrEmpty(_regex))
                {
                    return Regex.Replace(input, pattern, replacement);
                }
                return input;
            }
            catch (Exception e)
            {

                return input;
            }
        }

        private void Rename()
        {
            if (GUILayout.Button("执行"))
            {
                for (int i = 0; i < _showFiles.Count; i++)
                {
                    var path = _showFiles[i];
                    var fileName = Path.GetFileNameWithoutExtension(path);
                    var nameNew = GetNewName(i, path);
                    var fileInfo = new FileInfo(path);
                    File.Move(path, $"{fileInfo.DirectoryName}/{nameNew}{fileInfo.Extension}");
                    if (_validForMeta)
                    {
                        var metaFile = new FileInfo($"{path}.meta");
                        if (metaFile.Exists)
                        {
                            File.Move($"{path}.meta", $"{metaFile.DirectoryName}/{nameNew}{fileInfo.Extension}.meta");
                        }
                    }
                    Debug.Log($"Rename File [{fileName}] to [{nameNew}]");
                }
                AssetDatabase.Refresh();
            }

        }

        private void Preview()
        {
            EditorGUILayout.BeginHorizontal();
            {
                _namePreview = GetNewName(_showFiles.IndexOf(Selected), Selected);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.LabelField("文件名(修改后)", _namePreview);
        }

        private string GetNewName(int index, string path)
        {
            if (path.IsNullOrEmpty())
                return null;
            var oldName = Path.GetFileNameWithoutExtension(path);

            switch (_rule)
            {
                case FileRenameRule.Regex:
                    return GetMatch(oldName, _regex, _fileRename);
                case FileRenameRule.Common:
                    switch (_renameOptions)
                    {
                        case RenameOptions.AddEnd:
                            return $"{oldName}_{_fileRename}";
                        case RenameOptions.AddFront:
                            return $"{_fileRename}_{oldName}";
                        case RenameOptions.RenameInOrder:
                            return $"{oldName}_{index}";
                        case RenameOptions.AddFolderAtFront:
                        {
                            var directoryInfo = new FileInfo(path).Directory;
                            if (directoryInfo != null)
                            {
                                switch (_folderLevel)
                                {
                                    case FolderLevel.First:
                                        return $"{directoryInfo.Name}_{oldName}";
                                    case FolderLevel.Second:
                                        if (directoryInfo.Parent != null)
                                            return $"{directoryInfo.Parent.Name}_{oldName}";
                                        break;
                                }
                            }
                            break;
                        }
                        case RenameOptions.ToLower:
                            return oldName.ToLower();
                    }
                    break;
            }

            return oldName;
        }
        
        private enum Filter
        {
            All,
            SameName,
        }
    }
}