using UnityEditor;
using UnityEngine;

namespace ET
{
    public class CurveNode : MonoBehaviour
    {
        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }
        
        public Vector3 LocalPosition
        {
            get => transform.localPosition;
            set => transform.localPosition = value;
        }

        public float Fov = 60;

        // public Quaternion Rotation
        // {
        //     get => transform.rotation;
        //     set => transform.rotation = value;
        // }
        //
        // public Quaternion LocalRotation
        // {
        //     get => transform.localRotation;
        //     set => transform.localRotation = value;
        // }
        
        [HideInInspector]
        public CurveNode FrontNode;
        [HideInInspector]
        public CurveNode NextNode;

        public CurveCtrlPoint CtrlPoint1
        {
            get => _ctrlPoint1;
            set
            {
                if (value != null)
                {
                    _ctrlPoint1 = value;
                    _ctrlPoint1.transform.SetParent(this.transform, false);
                }
            }
        }

        public CurveCtrlPoint CtrlPoint2
        {
            get => _ctrlPoint2;
            set
            {
                if (value != null)
                {
                    _ctrlPoint2 = value;
                    _ctrlPoint2.transform.SetParent(this.transform, false);
                }
            }
        }

        public void Clear()
        {
            FrontNode = null;
            NextNode = null;
        }

        [SerializeField]
        private CurveCtrlPoint _ctrlPoint1;
        [SerializeField]
        private CurveCtrlPoint _ctrlPoint2;

        [SerializeField]
        private Vector3 _ctrlPos1;
        [SerializeField]
        private Vector3 _ctrlPos2;

#if UNITY_EDITOR

        public void UpdateCtrlPosition(bool focusUpdate = false)
        {
            if (_ctrlPos1 != _ctrlPoint1.LocalPosition || focusUpdate)
            {
                var dir = _ctrlPoint1.LocalPosition.normalized;
                var dis = _ctrlPoint2.LocalPosition.magnitude;
                var pos2 = dir * -1 * dis;
                _ctrlPoint2.LocalPosition = pos2;
            }else if (_ctrlPos2 != _ctrlPoint2.LocalPosition)
            {
                var dir = _ctrlPoint2.LocalPosition.normalized;
                var dis = _ctrlPoint1.LocalPosition.magnitude;
                var pos1 = dir * -1 * dis;
                _ctrlPoint1.LocalPosition = pos1;
            }

            _ctrlPos1 = _ctrlPoint1.LocalPosition;
            _ctrlPos2 = _ctrlPoint2.LocalPosition;
        }

        public bool HideGizmos;
        
        public void DrawGizmos()
        {
            UpdateCtrlPosition();
            if (_ctrlPoint1 != null && _ctrlPoint2 != null)
            {
                if (NextNode != null)
                {
                    _ctrlPoint1.gameObject.SetActive(true);
                    Handles.DrawLine(CtrlPoint1.Position, Position);
                    _ctrlPoint1.DrawGizmos();
                }
                else
                {
                    _ctrlPoint1.gameObject.SetActive(false);
                }

                if (FrontNode != null)
                {
                    _ctrlPoint2.gameObject.SetActive(true);
                    Handles.DrawLine(CtrlPoint2.Position, Position);
                    _ctrlPoint2.DrawGizmos();
                }
                else
                {
                    _ctrlPoint2.gameObject.SetActive(false);
                }
            }

            if (!HideGizmos)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(Position, 0.2f);
                Gizmos.color = Color.white;
            }
            
            if (NextNode != null && CtrlPoint2 !=null)
            {
                Handles.DrawBezier(Position, NextNode.Position, 
                    CtrlPoint1.Position, NextNode.CtrlPoint2.Position, Color.green, null, 3);
            }
            
        }
#endif
    }
}