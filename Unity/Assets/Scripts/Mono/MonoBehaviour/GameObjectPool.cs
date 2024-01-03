using System;
using System.Collections.Generic;
using ET;
using UnityEngine;

namespace ET
{
    public class GameObjectPool : MonoBehaviour
    {
        public static GameObjectPool Instance
        {
            get
            {
                if (_instance == null)
                {
                    var obj = new GameObject("[GameObjectPool]");
                    UnityEngine.Object.DontDestroyOnLoad(obj);
                    _instance = obj.AddComponent<GameObjectPool>();
                }
                return _instance;
            }
        }

        private static GameObjectPool _instance;
        
        private static Dictionary<string, ItemInfo> _poolObjs = new ();
        private const int DestroyTime = 20;
        private class ItemInfo
        {
            public float TagTime;
            public Stack<GameObject> Pool = new();
        }
        
        private void Update()
        {
            if (Mathf.FloorToInt(Time.time % DestroyTime) == 0)
                this.LoopClear();
        }

        private void LoopClear()
        {
            var removeKeys = new List<string>();
            var cur = Time.time;
            foreach (var kv in _poolObjs)
            {
                if (cur - kv.Value.TagTime >= DestroyTime)
                {
                    removeKeys.Add(kv.Key);
                    foreach (GameObject o in kv.Value.Pool)
                    {
                        Destroy(o);
                    }
                    kv.Value.Pool.Clear();
                }
            }

            foreach (string key in removeKeys)
            {
                _poolObjs.Remove(key);
            }
        }

        private void OnDestroy()
        {
            Destroy(_instance.gameObject);
            _instance = null;
        }

        public void Collect(string key, GameObject obj)
        {
            if (!_poolObjs.TryGetValue(key, out var item))
            {
                item = new ItemInfo();
                _poolObjs[key] = item;
            }

            item.TagTime = Time.time;
            obj.transform.SetParent(this.transform, false);
            obj.SetActive(false);
            item.Pool.Push(obj);
        }
        
        public bool Fetch(string key, out GameObject obj)
        {
            if (_poolObjs.TryGetValue(key, out var item))
            {
                if (item.Pool.Count > 0)
                {
                    item.TagTime = Time.time;
                    obj = item.Pool.Pop();
                    obj.SetActive(true);
                    obj.transform.SetParent(null, false);
                    return true;
                }
            }
            obj = null;
            return false;
        }
    }
    
}