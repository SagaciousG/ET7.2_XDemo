﻿using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

 namespace ET
{
    [CustomEditor(typeof(CurveRoot))]
    public class CurveRootEditor : Editor
    {
        [DrawGizmo(GizmoType.Active | GizmoType.InSelectionHierarchy | GizmoType.NonSelected, typeof(CurveRoot))]
        static void MyGizmo(CurveRoot root, GizmoType gizmoType) //参数1 为 组件类型，随意选， 
        {
            root.Nodes = root.GetComponentsInChildren<CurveNode>();
            if (root.Nodes == null) return;
            var nodes = root.Nodes;
            for (int i = 0; i < nodes.Length; i++)
            {
                var node = nodes[i];
                node.name = i == 0 ? "start" : (i == nodes.Length - 1 ? "end" : $"node_{i}");
                node.Clear();
            }
            for (int i = 0; i < nodes.Length - 1; i++)
            {
                var n1 = nodes[i];
                var n2 = nodes[i + 1];
                n1.NextNode = n2;
                n2.FrontNode = n1;
            }
            
            foreach (var node in root.Nodes)
            {
                node.DrawGizmos();
            }

            root.RotNodes = root.GetComponentsInChildren<CurveRotNode>();
            root.FovNodes = root.GetComponentsInChildren<CurveFovNode>();
            
            root.ReCalLength();

            for (int i = 0; i < root.RotNodes.Length - 1; i++)
            {
                var n1 = root.RotNodes[i];
                var n2 = root.RotNodes[i + 1];
                n1.name = $"rot_{i}";
                n2.name = $"rot_{i + 1}";
                
                var len = root.GetLength(n1.Percent, n2.Percent);
                var sections = Mathf.CeilToInt(len / 3) * 10;
                for (int j = 0; j < sections; j++)
                {
                    var t = j * 1f / sections;
                    var p = root.GetPosition(t * (n2.Percent - n1.Percent) + n1.Percent);
                    var up = Quaternion.SlerpUnclamped(Quaternion.Euler(n1.transform.up), Quaternion.Euler(n2.transform.up), t);
                    if (j % 10 == 0)
                    {
                        GizmosHelper.DrawLine(p, p + up.eulerAngles.normalized * 0.6f, Color.green);
                    }
                    else
                    {
                        GizmosHelper.DrawLine(p, p + up.eulerAngles.normalized * 0.3f, Color.green);
                    }
                }
            }

            for (int i = 0; i < root.FovNodes.Length - 1; i++)
            {
                var n1 = root.FovNodes[i];
                var n2 = root.FovNodes[i + 1];
                n1.name = $"fov_{i}";
                n2.name = $"fov_{i + 1}";
            }

            if (_target != null)
            {
                var forward = _target.forward;
                var points = new Vector3[5];
                points[0] = _target.transform.position;
                if (_isCamera)
                {
                    var fov = _target.GetComponent<Camera>().fieldOfView;
                    var v1 = forward.Rotate(Vector3.one, fov * 0.5f);
                    var v2 = forward.Rotate(Vector3.one, fov * -0.5f);
                    var v3 = forward.Rotate(new Vector3(1, -1, -1), fov * 0.5f);
                    var v4 = forward.Rotate(new Vector3(1, -1, -1), fov * -0.5f);
                    points[1] = v1;
                    points[2] = v3;
                    points[3] = v2;
                    points[4] = v4;
                }
                else
                {
                    var v1 = forward.Rotate(Vector3.one, 45);
                    var v2 = forward.Rotate(Vector3.one, 45 * -1);
                    var v3 = forward.Rotate(new Vector3(1, -1, -1), 45);
                    var v4 = forward.Rotate(new Vector3(1, -1, -1), 45 * -1);
                    points[1] = v1;
                    points[2] = v3;
                    points[3] = v2;
                    points[4] = v4;
                }

                for (int i = 1; i < 5; i++)
                {
                    GizmosHelper.DrawLine(points[0], points[i] + points[0], Color.white);
                    if (i + 1 < 5)
                    {
                        GizmosHelper.DrawLine(points[i] + points[0], points[i + 1] + points[0], Color.white);
                    }
                    else
                    {
                        GizmosHelper.DrawLine(points[4] + points[0], points[1] + points[0], Color.white);
                    }
                }
            }
        }
        
        private CurveRoot CurveRoot => (CurveRoot) target;

        private static List<string> _saveFileNames = new List<string>();
        private static List<string> _saveFiles = new List<string>();
        private string _saveDir = "Assets/EditorJsons/BezierData";
        private string _saveName = "";
        private int _selectedFile;
        private Transform _lookAtTarget;

        private static float _percent;
        private static Transform _target;
        private static bool _isCamera;
        private void RefreshSaveFiles()
        {
            var files = Directory.GetFiles(_saveDir, "*.bezier.txt");
            _saveFileNames.Clear();
            _saveFiles.Clear();
            foreach (var file in files)
            {
                _saveFileNames.Add(Path.GetFileNameWithoutExtension(file));
                _saveFiles.Add(file);
            }
        }
        
        private void Save()
        {
            if (string.IsNullOrEmpty(_saveName))
            {
                _saveName = System.DateTime.Now.ToString("MMddhhmmss");
            }
            
            CurveSaveObj.Save(this.CurveRoot,  $"{_saveDir}/{_saveName}.bezier.txt");
            AssetDatabase.Refresh();
        }
        


        private void Read()
        {
            var json = File.ReadAllText(_saveFiles[_selectedFile]);
            var data = JsonUtility.FromJson<CurveSaveObj>(json);
            foreach (var node in this.CurveRoot.Nodes)
            {
                DestroyImmediate(node.gameObject);
            }

            this.CurveRoot.Nodes = new CurveNode[data.Nodes.Length];
            for (int i = 0; i < data.Nodes.Length; i++)
            {
                var nodeData = data.Nodes[i];
                var node = new GameObject($"node_{i}").AddComponent<CurveNode>();
                node.transform.SetParent(this.CurveRoot.transform, false);
                this.CurveRoot.Nodes[i] = node;
                // node.LocalRotation = Quaternion.Euler(nodeData.Rotation);
                node.LocalPosition = nodeData.Position;
                node.CtrlPoint1 = new GameObject("ctrl_1").AddComponent<CurveCtrlPoint>();
                node.CtrlPoint2 = new GameObject("ctrl_2").AddComponent<CurveCtrlPoint>();
                node.CtrlPoint1.LocalPosition = nodeData.CtrlPoint1.Position;
                node.CtrlPoint2.LocalPosition = nodeData.CtrlPoint2.Position;
            }

        }

        private void LookAtPoint()
        {
            foreach (var n in this.CurveRoot.Nodes)
            {
                n.transform.LookAt(_lookAtTarget);
            }
        }
        
        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginVertical();
            _saveDir = EditorGUILayout.TextField("文件夹路径",_saveDir);
            _saveName = EditorGUILayout.TextField("文件名", _saveName);
            if (GUILayout.Button("保存"))
            {
                Save();
            }
            
            EditorGUILayout.EndVertical();
            if (GUILayout.Button("刷新数据"))
            {
                RefreshSaveFiles();
            }

            EditorGUILayout.BeginHorizontal();
            _selectedFile = EditorGUILayout.Popup(_selectedFile, _saveFileNames.ToArray());
            if (GUILayout.Button("读取"))
            {
                Read();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            _lookAtTarget = (Transform) EditorGUILayout.ObjectField("", _lookAtTarget, typeof(Transform), true);
            if (GUILayout.Button("LookAtPoint"))
            {
                LookAtPoint();
            }
            EditorGUILayout.EndHorizontal();

            _target = (Transform)EditorGUILayout.ObjectField("Target", _target, typeof(Transform), true);
            _percent = EditorGUILayout.Slider("Percent", _percent, 0, 1);
            _isCamera = EditorGUILayout.Toggle("IsCamera", _isCamera);
            if (_target != null)
            {
                _target.position = this.CurveRoot.GetPosition(_percent);
                if (this.CurveRoot.TryGetRot(_percent, out var rot))
                {
                    _target.rotation = rot;
                }
                if (_isCamera)
                {
                    if (this.CurveRoot.TryGetFov(_percent, out var fov))
                    {
                        _target.GetComponent<Camera>().fieldOfView = fov;
                    }
                }
            }
        }
    }
}