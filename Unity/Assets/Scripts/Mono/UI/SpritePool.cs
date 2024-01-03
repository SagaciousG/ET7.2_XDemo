using System.Collections.Generic;
using ET;
using UnityEngine;

namespace ET
{
    public class SpritePool : MonoBehaviour
    {
        private const int DestoryTime = 20;
        public static SpritePool Instance
        {
            get
            {
                if (_instance == null)
                {
                    var obj = new GameObject("[SpritePool]");
                    DontDestroyOnLoad(obj);
                    _instance = obj.AddComponent<SpritePool>();
                }
                return _instance;
            }
        }

        private static SpritePool _instance;
        
        private Dictionary<string, Stack<Sprite>> _namePool = new Dictionary<string, Stack<Sprite>>();

        private Dictionary<string, int> _lastCallTime = new Dictionary<string, int>();

        private void OnDestroy()
        {
            if (_instance == null)
                return;
            Destroy(_instance.gameObject);
            _instance = null;
        }

        private void Update()
        {
            if (Mathf.FloorToInt(Time.time % DestoryTime) == 0)
                this.LoopClear();
        }

        private void LoopClear()
        {
            var list = new List<string>();
            var cur = Time.time;
            foreach (var key in _namePool.Keys)
            {
                var t = _lastCallTime[key];
                if (cur - t >= DestoryTime)
                {
                    list.Add(key);
                }
            }

            foreach (var key in list)
            {
                // foreach (var sprite in _namePool[key])
                // {
                //     Destroy(sprite);
                // }
                _namePool.Remove(key);
            }
            YooAssetHelper.UnloadAssets();
        }

        public void Collect(string assetName, Sprite sprite)
        {
            if (!_namePool.TryGetValue(assetName, out var stack))
            {
                _namePool[assetName] = new Stack<Sprite>();
                stack = _namePool[assetName];
            }
            stack.Push(sprite);
            SetTime(assetName);
        }

        public async ETTask<Sprite> Fetch(string spritName)
        {
            if (!_namePool.TryGetValue(spritName, out var stack))
            {
                _namePool[spritName] = new Stack<Sprite>();
                stack = _namePool[spritName];
            }
            SetTime(spritName);
            if (stack.Count > 0)
                return stack.Pop();
            var sprite = await YooAssetHelper.LoadAssetAsync<Sprite>(spritName);
            return sprite;
        }
        
        private void SetTime(string spritPath)
        {
            _lastCallTime[spritPath] = (int) Time.time;
        }
    }
    
}