using System.Collections.Generic;
using Unity.Mathematics;

namespace ET.Server
{
    [FriendOf(typeof(MoveComponent))]
    [FriendOf(typeof(NumericComponent))]
    public static class UnitHelper
    {
        // 获取看见unit的玩家，主要用于广播
        public static Dictionary<long, AOIEntity> GetBeSeePlayers(this Unit self)
        {
            return self.GetComponent<AOIEntity>().GetBeSeePlayers();
        }

        public static Unit GetUnit(Scene scene, long unitId)
        {
            if (scene.SceneType == SceneType.Map)
            {
                var unitComponent = scene.GetComponent<UnitComponent>();
                return unitComponent.Get(unitId);
            }

            return null;
        }

    }
}