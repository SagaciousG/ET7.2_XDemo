﻿using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public class CurveManager : MonoBehaviour
    {
        public static CurveManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GameObject("trackRoot").AddComponent<CurveManager>();
                return _instance;
            }
        }
        private static CurveManager _instance;
        
        private Stack<CurveNode> _nodePool = new Stack<CurveNode>();
        private Stack<CurveFovNode> _fovNodePool = new Stack<CurveFovNode>();
        private Stack<CurveRotNode> _rotNodePool = new Stack<CurveRotNode>();
        
        
        private Stack<CurveCtrlPoint> _ctrlPointPool = new Stack<CurveCtrlPoint>();
        private Stack<CurveRoot> _trackRootPool = new Stack<CurveRoot>();

        private void Awake()
        {
            _instance = this;
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _instance = null;
        }
        
        // public TrackRoot Create(string id)
        // {
        //     var obj = AssetBundleHelper.LoadUI<TextAsset>("bezier", $"{id}.bezier");
        //     var data = JsonUtility.FromJson<TrackSaveObj>(obj.text);
        //     data.FileName = id;
        //     var track = Fetch(data);
        //     return track;
        // }

        public CurveRoot Fetch(CurveSaveObj data)
        {
            CurveRoot curveRoot = GetRoot(data.FileName);
            curveRoot.Nodes = new CurveNode[data.Nodes.Length];
            for (int i = 0; i < data.Nodes.Length; i++)
            {
                var nodeData = data.Nodes[i];
                var node = GetNode($"node_{i}");
                node.transform.SetParent(curveRoot.transform, false);
                curveRoot.Nodes[i] = node;
                node.LocalPosition = nodeData.Position;
                node.CtrlPoint1 = GetCtrlPoint("ctrl_1");
                node.CtrlPoint2 = GetCtrlPoint("ctrl_2");
                node.CtrlPoint1.LocalPosition = nodeData.CtrlPoint1.Position;
                node.CtrlPoint2.LocalPosition = nodeData.CtrlPoint2.Position;
            }
            curveRoot.ReCalLength();

            for (int i = 0; i < data.FovNodes.Length; i++)
            {
                var nodeData = data.FovNodes[i];
                var node = GetFovNode($"fov_{i}");
                node.transform.SetParent(curveRoot.transform, false);
                curveRoot.FovNodes[i] = node;
                node.transform.position = curveRoot.GetPosition(nodeData.Percent);
                node.Percent = nodeData.Percent;
                node.Fov = nodeData.Fov;
            }

            for (int i = 0; i < data.RotNodes.Length; i++)
            {
                var nodeData = data.RotNodes[i];
                var node = GetRotNode($"rot_{i}");
                node.transform.SetParent(curveRoot.transform, false);
                curveRoot.RotNodes[i] = node;
                node.transform.position = curveRoot.GetPosition(nodeData.Percent);
                node.Percent = nodeData.Percent;
                node.transform.localRotation = Quaternion.Euler(nodeData.Rotation);
            }

            return curveRoot;
        }

        private CurveRoot GetRoot(string rootName)
        {
            if (_trackRootPool.Count > 0)
            {
                var root = _trackRootPool.Pop();
                root.gameObject.name = rootName;
                return root;
            }
            return new GameObject(rootName).AddComponent<CurveRoot>();
        }
        
        private CurveNode GetNode(string nodeName)
        {
            if (_nodePool.Count > 0)
            {
                var node = _nodePool.Pop();
                node.gameObject.name = nodeName;
                return node;
            }
            return new GameObject(nodeName).AddComponent<CurveNode>();
        }

        private CurveFovNode GetFovNode(string nodeName)
        {
            if (_nodePool.Count > 0)
            {
                var node = _fovNodePool.Pop();
                node.gameObject.name = nodeName;
                return node;
            }
            return new GameObject(nodeName).AddComponent<CurveFovNode>();
        }
        
        private CurveRotNode GetRotNode(string nodeName)
        {
            if (_nodePool.Count > 0)
            {
                var node = _rotNodePool.Pop();
                node.gameObject.name = nodeName;
                return node;
            }
            return new GameObject(nodeName).AddComponent<CurveRotNode>();
        }

        private CurveCtrlPoint GetCtrlPoint(string nodeName)
        {
            if (_ctrlPointPool.Count > 0)
            {
                var node = _ctrlPointPool.Pop();
                node.gameObject.name = nodeName;
                return node;
            }
            return new GameObject(nodeName).AddComponent<CurveCtrlPoint>();
        }

        public void Collect(CurveRoot root)
        {
            foreach (var node in root.Nodes)
            {
                _ctrlPointPool.Push(node.CtrlPoint1);
                _ctrlPointPool.Push(node.CtrlPoint2);
                _nodePool.Push(node);
                node.CtrlPoint1.transform.SetParent(transform, false);
                node.CtrlPoint2.transform.SetParent(transform, false);
                node.transform.SetParent(transform, false);
            }

            foreach (var node in root.FovNodes)
            {
                node.transform.SetParent(transform, false);
                _fovNodePool.Push(node);
            }

            foreach (var node in root.RotNodes)
            {
                node.transform.SetParent(transform, false);
                _rotNodePool.Push(node);
            }
            
            _trackRootPool.Push(root);
            root.transform.SetParent(transform, false);
            root.Nodes = null;
        }
    }
}