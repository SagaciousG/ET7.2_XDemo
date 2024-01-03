using System;
using UnityEngine;

namespace ET
{
    public class CurveRoot : MonoBehaviour
    {
        public CurveNode Final => Nodes[Nodes.Length - 1];
        
        [HideInInspector]
        public CurveNode[] Nodes;

        public CurveFovNode[] FovNodes;
        public CurveRotNode[] RotNodes;
        
        
        private float[] _sectionLength;
        private float _trackTotal;

        private CurveFovNode[] _sortedFovNodes;
        private CurveRotNode[] _sortedRotNodes;
        
        public void ReCalLength()
        {
            _trackTotal = 0;
            if (Nodes.Length == 0)
                return;
            _sectionLength = new float[Nodes.Length - 1];
            for (int i = 0; i < Nodes.Length - 1; i++)
            {
                var node1 = Nodes[i];
                var node2 = Nodes[i + 1];
                _sectionLength[i] = BezierUtils.BezierLength(node1.Position, node1.CtrlPoint1.Position, 
                    node2.CtrlPoint2.Position, node2.Position);
                _trackTotal += _sectionLength[i];
            }

            _sortedFovNodes = new CurveFovNode[FovNodes.Length];
            _sortedRotNodes = new CurveRotNode[RotNodes.Length];
            FovNodes.CopyTo(_sortedFovNodes, 0);
            RotNodes.CopyTo(_sortedRotNodes, 0);
            Array.Sort(_sortedFovNodes, (a, b) => Mathf.CeilToInt(a.Percent * 100 - b.Percent * 100));
            Array.Sort(_sortedRotNodes, (a, b) => Mathf.CeilToInt(a.Percent * 100 - b.Percent * 100));
        }

        public float GetLength(float from, float to)
        {
            var length = _trackTotal * to - _trackTotal * from;
            return Mathf.Clamp(length, 0, _trackTotal);
        }
        
        //路径百分比转路径段索引
        private int Percent2Index(float t)
        {
            var track = t * _trackTotal;
            var cur = 0f;
            for (int i = 0; i < _sectionLength.Length; i++)
            {
                cur += _sectionLength[i];
                if (cur >= track)
                {
                    return i;
                }
            }

            return _sectionLength.Length - 1;
        }

        //总百分比转所在分段百分比
        private float Percent2SectionPercent(float t)
        {
            var track = t * _trackTotal;
            var cur = 0f;
            var section = 0f;
            foreach (var t1 in _sectionLength)
            {
                section = t1;
                if (cur + section >= track - 0.0001)
                {
                    break;    
                }

                cur += section;
            }

            return (track - cur) / section;
        }
        
        /// <summary>
        /// t: 0 - 1
        /// t为在全路程中的比
        /// </summary>
        /// <param name="t"></param>
        /// <returns>返回t所在的世界坐标</returns>
        public Vector3 GetPosition(float t)
        {
            if (Nodes.Length < 2)
                return Vector3.zero;
            t = Mathf.Clamp01(t);
            var index = Percent2Index(t);
            var node1 = Nodes[index];
            var node2 = Nodes[index + 1];
            var t0 = Percent2SectionPercent(t);
            return BezierUtils.CalculateThreePowerBezierPoint(t0, 
                node1.Position,
                node1.CtrlPoint1.Position, 
                node2.CtrlPoint2.Position,
                node2.Position);
        }

        /// <summary>
        /// t: 0 - 1
        /// t为在全路程中的比
        /// </summary>
        /// <param name="t"></param>
        /// <param name="rot"></param>
        /// <returns>返回当前旋转四元数</returns>
        public bool TryGetRot(float t, out Quaternion rot)
        {
            t = Mathf.Clamp01(t);
            CurveRotNode cur = null;
            CurveRotNode last = null;
            foreach (var node in _sortedRotNodes)
            {
                if (node.Percent >= t)
                {
                    cur = node;
                    break;
                }

                last = node;
            }

            if (cur != null && last != null)
            {
                rot = Quaternion.Slerp(last.transform.rotation, cur.transform.rotation,
                    (t - last.Percent) / (cur.Percent - last.Percent));
                return true;
            }
            rot = Quaternion.identity;
            return false;
        }

        public bool TryGetFov(float t, out float fov)
        {
            t = Mathf.Clamp01(t);
            CurveFovNode cur = null;
            CurveFovNode last = null;
            foreach (var node in _sortedFovNodes)
            {
                if (node.Percent >= t)
                {
                    cur = node;
                    break;
                }

                last = node;
            }

            if (cur != null && last != null)
            {
                fov = (cur.Fov - last.Fov) * 
                    ((t - last.Percent) / (cur.Percent - last.Percent)) + cur.Fov;
                return true;
            }

            fov = 0;
            return false;
        }

        public void Dispose()
        {
            CurveManager.Instance.Collect(this);
        }
    }
}