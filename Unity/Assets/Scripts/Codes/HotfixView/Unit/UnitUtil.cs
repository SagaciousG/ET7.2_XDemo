using UnityEngine;
using ET;

namespace ET.Client
{
    public static class UnitUtil
    {
        public static async ETTask<GameObject> LoadModel(this XModelImage self, UnitShowConfig cfg)
        {
            var path = $"{cfg.Model}";
            var obj = await self.Load(path);
            obj.transform.localScale = cfg.Scale * Vector3.one;
            obj.transform.localPosition = cfg.Offset.ToVector3();
            obj.transform.localRotation = Quaternion.Euler(cfg.Rotation.ToVector3());
            return obj;
        }
    }
}