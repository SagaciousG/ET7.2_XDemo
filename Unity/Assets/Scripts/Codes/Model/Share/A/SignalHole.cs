using System.Collections.Generic;

namespace ET
{
    [ChildOf(typeof(SignalHoleComponent))]
    public class SignalHole : Entity, IAwake
    {
        public string Key { get; set; }
    }
}