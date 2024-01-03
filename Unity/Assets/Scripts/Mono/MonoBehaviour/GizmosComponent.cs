using System;
using UnityEngine;

namespace ET
{
    public interface IGizmos
    {
        void OnDrawGizmos();
    }
    public class GizmosComponent : MonoBehaviour
    {
        private IGizmos target;
        
        public void Init(IGizmos gizmos)
        {
            target = gizmos;
        }

        public void Clear()
        {
            target = null;
        }
        
        private void OnDrawGizmos()
        {
            target?.OnDrawGizmos();
        }
    }
}