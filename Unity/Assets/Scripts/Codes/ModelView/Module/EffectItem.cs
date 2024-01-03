using UnityEngine;

namespace ET.Client
{
    [ChildOf(typeof(EffectComponent))]
    public class EffectItem : Entity, IAwake<string, Transform>, IDestroy
    {
        public string AssetName;
        public GameObject EffGO;
        public Transform ParentTrans;
        public Vector3 Offset;
        public Vector3 Scale;
        public Vector3 Rotation;
        public ETCancellationToken DisposeToken;
        public long LiveTime;
        public bool Loaded;
    }
}