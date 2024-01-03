using System.Collections.Generic;
using ET;

namespace ET.Client
{
    [ChildOf(typeof(UI))]
    public class ETUIList : Entity, IAwake<UIList>, IDestroy, IUIEntity
    {
        public UIList UIList { get; set; }

        public Dictionary<int, UI> IndexUIs { get; set; } = new();
        
        public Stack<UI> UnUsedUIs { get; set; } = new();
    }
}