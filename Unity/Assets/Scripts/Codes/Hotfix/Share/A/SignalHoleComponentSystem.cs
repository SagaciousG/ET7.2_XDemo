using System;

namespace ET
{
    public static class SignalHoleComponentSystem
    {
        
        
        public static void Add(this SignalHoleComponent self, string key, Entity target, Action action)
        {
            var hole = self.GetChild<SignalHole>(key.GetHashCode());
            if (hole == null)
            {
                hole = self.AddChildWithId<SignalHole>(key.GetHashCode());
                hole.Key = key;
            }

            var signal = hole.GetChild<Signal>(target.GetHashCode());
            if (signal == null)
            {
                signal = hole.AddChildWithId<Signal>(target.GetHashCode());
                signal.Target = new WeakReference<Entity>(target);
            }
            signal.Actions.Add(action);
        }

        public static void Add<T>(this SignalHoleComponent self, string key, Entity target, Action<T> action)
        {
            var hole = self.GetChild<SignalHole>(key.GetHashCode());
            if (hole == null)
            {
                hole = self.AddChildWithId<SignalHole>(key.GetHashCode());
                hole.Key = key;
            }

            var signal = hole.GetChild<Signal<T>>(target.GetHashCode());
            if (signal == null)
            {
                signal = hole.AddChildWithId<Signal<T>>(target.GetHashCode());
                signal.Target = new WeakReference<Entity>(target);
            }
            signal.Actions.Add(action);
        }
        
        public static void Add<T1, T2>(this SignalHoleComponent self, string key, Entity target, Action<T1, T2> action)
        {
            var hole = self.GetChild<SignalHole>(key.GetHashCode());
            if (hole == null)
            {
                hole = self.AddChildWithId<SignalHole>(key.GetHashCode());
                hole.Key = key;
            }

            var signal = hole.GetChild<Signal<T1, T2>>(target.GetHashCode());
            if (signal == null)
            {
                signal = hole.AddChildWithId<Signal<T1, T2>>(target.GetHashCode());
                signal.Target = new WeakReference<Entity>(target);
            }
            signal.Actions.Add(action);
        }

        public static void Remove(this SignalHoleComponent self, string key, Entity target, Action action)
        {
            var hole = self.GetChild<SignalHole>(key.GetHashCode());
            if (hole != null)
            {
                var signals = hole.GetChildren<Signal>();
                for (var i = 0; i < signals.Length; i++)
                {
                    var signal = signals[i];
                    if (!signal.Target.TryGetTarget(out var t))
                    {
                        signal.Dispose();
                        return;
                    }

                    if (t == target)
                    {
                        signal.Actions.Remove(action);
                    }
                    if (signal.Actions.Count == 0)
                        signal.Dispose();
                }
            }
        }
        
        public static void Remove<T>(this SignalHoleComponent self, string key, Entity target, Action<T> action)
        {
            var hole = self.GetChild<SignalHole>(key.GetHashCode());
            if (hole != null)
            {
                var signals = hole.GetChildren<Signal<T>>();
                for (var i = 0; i < signals.Length; i++)
                {
                    var signal = signals[i];
                    if (!signal.Target.TryGetTarget(out var t))
                    {
                        signal.Dispose();
                        return;
                    }

                    if (t == target)
                    {
                        signal.Actions.Remove(action);
                    }
                    if (signal.Actions.Count == 0)
                        signal.Dispose();
                }
            }
        }
        
        public static void Remove<T1, T2>(this SignalHoleComponent self, string key, Entity target, Action<T1, T2> action)
        {
            var hole = self.GetChild<SignalHole>(key.GetHashCode());
            if (hole != null)
            {
                var signals = hole.GetChildren<Signal<T1, T2>>();
                for (var i = 0; i < signals.Length; i++)
                {
                    var signal = signals[i];
                    if (!signal.Target.TryGetTarget(out var t))
                    {
                        signal.Dispose();
                        return;
                    }

                    if (t == target)
                    {
                        signal.Actions.Remove(action);
                    }
                    if (signal.Actions.Count == 0)
                        signal.Dispose();
                }
            }
        }
        
        public static void Fire(this SignalHoleComponent self, string key)
        {
            var hole = self.GetChild<SignalHole>(key.GetHashCode());
            if (hole != null)
            {
                var signals = hole.GetChildren<Signal>();
                for (var i = 0; i < signals.Length; i++)
                {
                    var signal = signals[i];
                    if (!signal.Target.TryGetTarget(out var t))
                    {
                        signal.Dispose();
                        return;
                    }

                    foreach (var action in signal.Actions)
                    {
                        action.Invoke();
                    }
                }
            }
        }
        
        public static void Fire<T>(this SignalHoleComponent self, string key, T arg)
        {
            var hole = self.GetChild<SignalHole>(key.GetHashCode());
            if (hole != null)
            {
                var signals = hole.GetChildren<Signal<T>>();
                for (var i = 0; i < signals.Length; i++)
                {
                    var signal = signals[i];
                    if (!signal.Target.TryGetTarget(out var t))
                    {
                        signal.Dispose();
                        return;
                    }

                    foreach (var action in signal.Actions)
                    {
                        action.Invoke(arg);
                    }
                }
            }
        }
        
        public static void Fire<T1, T2>(this SignalHoleComponent self, string key, T1 arg, T2 arg2)
        {
            var hole = self.GetChild<SignalHole>(key.GetHashCode());
            if (hole != null)
            {
                var signals = hole.GetChildren<Signal<T1, T2>>();
                for (var i = 0; i < signals.Length; i++)
                {
                    var signal = signals[i];
                    if (!signal.Target.TryGetTarget(out var t) || t.IsDisposed)
                    {
                        signal.Dispose();
                        return;
                    }

                    foreach (var action in signal.Actions)
                    {
                        action.Invoke(arg, arg2);
                    }
                }
            }
        }
    }
}