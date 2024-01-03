using System;
using System.Collections.Generic;

namespace ET
{
    [ChildOf(typeof(SignalHole))]
    public class Signal : Entity, IAwake
    {
        public WeakReference<Entity> Target { get; set; }
        public List<Action> Actions { get; set; } = new();
    }
    
    [ChildOf(typeof(SignalHole))]
    public class Signal<T> : Entity, IAwake
    {
        public WeakReference<Entity> Target { get; set; }
        public List<Action<T>> Actions { get; set; } = new();
    }
    
    [ChildOf(typeof(SignalHole))]
    public class Signal<T1, T2> : Entity, IAwake
    {
        public WeakReference<Entity> Target { get; set; }
        public List<Action<T1, T2>> Actions { get; set; } = new();
    }
}