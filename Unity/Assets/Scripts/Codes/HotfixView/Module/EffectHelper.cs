using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof(EffectItem))]
    public static class EffectHelper
    {
        public static long AddEffect(string assetName, Transform parent, long liveTime, Vector3 offset, Vector3 scale,
            Vector3 rotation)
        {
            var item = EffectComponent.Instance.AddChild<EffectItem, string, Transform>(assetName, parent);
            item.Offset = offset;
            item.Scale = scale;
            item.Rotation = rotation;
            item.LiveTime = liveTime;
            item.Load().Coroutine();
            return item.Id;
        }
        
        public static long AddEffect(string assetName, Transform parent, long liveTime, Vector3 offset, Vector3 scale)
        {
            return AddEffect(assetName, parent, liveTime, offset, scale, Vector3.zero);
        }
        
        public static long AddEffect(string assetName, Transform parent, long liveTime, Vector3 offset)
        {
            return AddEffect(assetName, parent, liveTime, offset, Vector3.one, Vector3.zero);
        }
        
        public static long AddEffect(string assetName, Transform parent, long liveTime)
        {
            return AddEffect(assetName, parent, liveTime, Vector3.zero, Vector3.one, Vector3.zero);
        }

        public static void Remove(ref long id)
        {
            var effectItem = EffectComponent.Instance.GetChild<EffectItem>(id);
            if (effectItem != null)
            {
                effectItem.DisposeToken?.Cancel();
                effectItem.Dispose();
                id = 0;
            }
        }
    }
}