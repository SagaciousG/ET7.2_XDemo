using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class GlobalComponent: Entity, IAwake
    {
        [StaticField]
        public static GlobalComponent Instance;
        
        public Transform Global;
        public Transform UI;
    }
}