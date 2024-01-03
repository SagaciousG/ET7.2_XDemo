using UnityEngine;

namespace ET.Client
{
    [ChildOf]
    public class GameObjectAgent: Entity, IAwake<string>, IDestroy
    {
        public string ObjName { get; set; }
        public Transform ParentTrans { get; set; }
        public string AssetPath;
        public Transform transform => gameObject.transform;
        public GameObject gameObject { get; set; }
        public bool IsEmptyObj { get; set; }
        
        public bool Loaded { get; set; }
    }
}