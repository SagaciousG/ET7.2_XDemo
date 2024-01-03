﻿using System;
using System.IO;
using UnityEngine;

namespace ET
{
    [Serializable]
    public class CurveSaveObj
    {
        public static void Save(CurveRoot curve, string path)
        {
            if (curve.Nodes.Length < 2)
                return;
            var saveObj = Convert(curve);
            saveObj.FileName = Path.GetFileNameWithoutExtension(path);

            var json = JsonUtility.ToJson(saveObj);
            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }
            File.WriteAllText(path, json);
        }
        
        public static CurveSaveObj Convert(CurveRoot curve)
        {
            var saveObj = new CurveSaveObj();
            saveObj.Nodes = new TrackSaveNode[curve.Nodes.Length];
            for (int i = 0; i < curve.Nodes.Length; i++)
            {
                var node = curve.Nodes[i];
                var saveNode = new TrackSaveNode();
                saveNode.Position = node.LocalPosition;
                saveNode.CtrlPoint1 = new TrackSaveCtrlPoint()
                {
                    Position = node.CtrlPoint1.LocalPosition
                };
                saveNode.CtrlPoint2 = new TrackSaveCtrlPoint()
                {
                    Position = node.CtrlPoint2.LocalPosition
                };
                saveObj.Nodes[i] = saveNode;
            }

            saveObj.RotNodes = new TrackRotSaveNode[curve.RotNodes.Length];
            for (int i = 0; i < curve.RotNodes.Length; i++)
            {
                var node = curve.RotNodes[i];
                saveObj.RotNodes[i] = new TrackRotSaveNode()
                {
                    Rotation = node.transform.localRotation.eulerAngles,
                    Percent = node.Percent,
                };
            }

            saveObj.FovNodes = new TrackFovSaveNode[curve.FovNodes.Length];
            for (int i = 0; i < curve.FovNodes.Length; i++)
            {
                saveObj.FovNodes[i] = new TrackFovSaveNode()
                {
                    Fov = curve.FovNodes[i].Fov,
                    Percent = curve.FovNodes[i].Percent
                };
            }
            return saveObj;
        }

        public string FileName { set; get; }

        public TrackSaveNode[] Nodes;
        public TrackRotSaveNode[] RotNodes;
        public TrackFovSaveNode[] FovNodes;
    }

    [Serializable]
    public class TrackSaveNode
    {
        public Vector3 Position;
        public Vector3 Rotation;

        public TrackSaveCtrlPoint CtrlPoint1;
        public TrackSaveCtrlPoint CtrlPoint2;
    }

    [Serializable]
    public class TrackSaveCtrlPoint
    {
        public Vector3 Position;
    }
    
    [Serializable]
    public class TrackFovSaveNode
    {
        public float Percent;
        public float Fov;
    }
    
    [Serializable]
    public class TrackRotSaveNode
    {
        public float Percent;
        public Vector3 Rotation;
    }
    
}