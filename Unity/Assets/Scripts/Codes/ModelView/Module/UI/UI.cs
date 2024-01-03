using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    [ChildOf()]
    public sealed class UI: Entity, IAwake<string, GameObject>, IDestroy, IUIEntity
    {
        public GameObject GameObject { get; set; }
        public Entity Component { get; set; }
		
        public string UIType { get; set; }
        
        public bool IsSingleUI { get; set; }

        public List<ETTask> UICloseTasks;
        public List<ETTask<object>> UICloseTasksWithParam;
        public object TaskParam;
    }
}