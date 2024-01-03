using System;

namespace ET
{
    public static class SignalHelper
    {
        public static void Add(Scene scene, string key, Entity target, Action action)
        {
            scene.GetComponent<SignalHoleComponent>().Add(key, target, action);
        }

        public static void Add<T>(Scene scene, string key, Entity target, Action<T> action)
        {
            scene.GetComponent<SignalHoleComponent>().Add(key, target, action);
        }
        
        public static void Add<T1, T2>(Scene scene, string key, Entity target, Action<T1, T2> action)
        {
            scene.GetComponent<SignalHoleComponent>().Add(key, target, action);
        }

        public static void Remove(Scene scene, string key, Entity target, Action action)
        {
            scene.GetComponent<SignalHoleComponent>().Remove(key, target, action);
        }

        public static void Remove<T>(Scene scene, string key, Entity target, Action<T> action)
        {
            scene.GetComponent<SignalHoleComponent>().Remove(key, target, action);
        }
        public static void Remove<T1, T2>(Scene scene, string key, Entity target, Action<T1, T2> action)
        {
            scene.GetComponent<SignalHoleComponent>().Remove(key, target, action);
        }
        
        public static void Fire(Scene scene, string key)
        {
            scene.GetComponent<SignalHoleComponent>().Fire(key);
        }
        
        public static void Fire<T>(Scene scene, string key, T arg)
        {
            scene.GetComponent<SignalHoleComponent>().Fire<T>(key, arg);
        }
        
        public static void Fire<T1, T2>(Scene scene, string key, T1 arg, T2 arg2)
        {
            scene.GetComponent<SignalHoleComponent>().Fire(key, arg, arg2);
        }
    }
}