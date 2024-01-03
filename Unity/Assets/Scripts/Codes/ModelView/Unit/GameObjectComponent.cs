using UnityEngine;

namespace ET.Client
{
    [ComponentOf]
    public class GameObjectComponent : Entity, IAwake<string>
    {
        public Transform transform => Agent?.transform;
        public GameObject gameObject => Agent?.gameObject;
        public bool Loaded => Agent?.Loaded ?? false;
        public GameObjectAgent Agent { get; set; }
    }
}