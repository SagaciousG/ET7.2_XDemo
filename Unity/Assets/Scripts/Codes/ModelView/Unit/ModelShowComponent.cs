using UnityEngine;

namespace ET.Client
{
    
    [ComponentOf(typeof(Unit))]
    public class ModelShowComponent : Entity, IAwake<int>, IAwake<string>, IDestroy
    {
        public UnitShowConfig Config { get; set; }
        public GameObject Shell { get; set; }
        public GameObject ViewGO { get; set; }
        public bool Loaded { get; set; }
        public EntityWaiter ShellWaiter { get; set; }
        public EntityWaiter ViewGOWaiter { get; set; }
        public QuickOutline Outline { get; set; }
    }
}