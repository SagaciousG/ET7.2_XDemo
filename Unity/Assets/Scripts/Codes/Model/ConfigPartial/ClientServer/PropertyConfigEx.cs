using System;
using System.Collections.Generic;

namespace ET
{
    public partial class PropertyConfigCategory
    {
        private MultiMap<int, PropertyConfig> _group = new MultiMap<int, PropertyConfig>();
        private Dictionary<int, PropertyConfig> _keyMap = new ();
        public override void AfterEndInit()
        {
            foreach (var kv in this.dict)
            {
                this._group.Add(kv.Value.Sort, kv.Value);
                _keyMap.Add(kv.Value.Key, kv.Value);
            }
        }

        public PropertyConfig[] GetByGroup(int group)
        {
            this._group.TryGetValue(group, out var res);
            return res?.ToArray() ?? Array.Empty<PropertyConfig>();
        }
        
        public PropertyConfig GetByKey(int key)
        {
            if (!this._keyMap.TryGetValue(key, out var res))
            {
                Log.Error("PropertyConfig is Null, Key = " + key);
            }
            return res;
        }
    }
}