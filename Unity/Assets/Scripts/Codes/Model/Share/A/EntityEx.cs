using System;
using System.Reflection;

namespace ET
{
    public static class EntitySystem
    {
        [StaticField]
        private static MethodInfo _entityCreateMethod;
        [StaticField]
        private static PropertyInfo _entityParentProperty;
        
        public static T FindChild<T>(this Entity self, Func<T, bool> match) where T : Entity
        {
            foreach (var child in self.Children.Values)
            {
                if (child is T t)
                {
                    if (match.Invoke(t))
                        return t;
                };
            }

            return null;
        }

        public static T GetParentDepth<T>(this Entity self) where T : Entity
        {
            var p = self.Parent;
            while (p != null)
            {
                if (p is T t)
                    return t;
                p = p.Parent;
            }

            return null;
        }

        public static T[] GetChildren<T>(this Entity self) where T : Entity
        {
            if (self?.Children == null)
                return Array.Empty<T>();

            var list = ListComponent<T>.Create();
            foreach (Entity entity in self.Children.Values)
            {
                if (entity is T t)
                    list.Add(t);
            }

            var res = list.ToArray();
            list.Dispose();
            return res;
        }
        
        public static Entity[] GetChildren(this Entity self, Type type)
        {
            if (self?.Children == null)
                return Array.Empty<Entity>();

            var list = ListComponent<Entity>.Create();
            foreach (Entity entity in self.Children.Values)
            {
                if (entity.GetType() == type)
                    list.Add(entity);
            }

            var res = list.ToArray();
            list.Dispose();
            return res;
        }
        
        public static Entity AddChild(this Entity self, Type type, bool isFromPool = false)
        {
            if (_entityCreateMethod == null)
            {
                _entityCreateMethod = typeof (Entity).GetMethod("Create", BindingFlags.Static | BindingFlags.NonPublic);
                _entityParentProperty = typeof (Entity).GetProperty("Parent", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.SetProperty | BindingFlags.GetProperty);
            }

            if (type.GetInterface(typeof (IAwake).FullName) == null)
            {
                throw new Exception($"{type.Name}必须实现接口IAwake");
            }
            
            var component = (Entity) _entityCreateMethod.Invoke(null, new object[]{type, isFromPool});
            component.Id = IdGenerater.Instance.GenerateId();
            _entityParentProperty.SetValue(component, self);

            EventSystem.Instance.Awake(component);
            return component;
        }

        public static void ClearId(this Entity self)
        {
            foreach (var kv in self.Components)
            {
                kv.Value.Id = 0;
                kv.Value.ClearId();
            }
            foreach (var kv in self.Children)
            {
                kv.Value.Id = 0;
                kv.Value.ClearId();
            }
        }
        
        public static void ReGenerateID(this Entity self)
        {
            foreach (var kv in self.Components)
            {
                kv.Value.Id = IdGenerater.Instance.GenerateId();
                kv.Value.ClearId();
            }
            foreach (var kv in self.Children)
            {
                kv.Value.Id = IdGenerater.Instance.GenerateId();
                kv.Value.ClearId();
            }
        }
    }
}