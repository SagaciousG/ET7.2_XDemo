using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.Mathematics;

namespace ET.Server
{
    [FriendOf(typeof(Unit))]
    public class MapAwakeSystem : AwakeSystem<Map, int>
    {
        protected override async void Awake(Map self, int id)
        {
            self.ConfigId = id;
            self.AddComponent<MapUnitComponent>();
            self.AddComponent<AOIManagerComponent>();
            InitNPC(self, id);
            
            var cfg = MapConfigCategory.Instance.Get(id);
            if (cfg.IsSecure == 0)
            {
                self.AddComponent<MapMonsterComponent, int>(cfg.MonsterGroup);
            }
        }

        private async void InitNPC(Map self, int id)
        {
            var unitComponent = self.DomainScene().GetComponent<UnitComponent>();
            var dbComponent = self.DomainScene().GetComponent<DBComponent>();
            var npcs = await dbComponent.Query<Unit>(u => u.Type == UnitType.NPC && u.map == id);
            var npcMap = new Dictionary<int, Unit>();
            foreach (var npc in npcs)
            {
                unitComponent.Add(npc);
                npcMap.Add(npc.GetComponent<UnitNPCComponent>().NPCID, npc);
                npc.AddComponent<AOIEntity, int, float3>(9 * 1000, npc.Position);
            }
            
            var npcCfgs = NPCConfigCategory.Instance.GetNPCByMap(id);
            foreach (var npcConfig in npcCfgs)
            {
                if (!npcMap.TryGetValue(npcConfig.Id, out var u))
                {
                    u = UnitFactory.CreateNPC(self.DomainScene(), npcConfig.Id);
                    unitComponent.Add(u);
                    u.AddComponent<AOIEntity, int, float3>(9 * 1000, u.Position);
                }
            }
            
            
        }
    }

    [FriendOf(typeof(MapUnitComponent))]
    public static class MapSystem
    {
        public static void AddUnit(this Map self, long unitId)
        {
            var mapUnitComponent = self.GetComponent<MapUnitComponent>();
            mapUnitComponent.MapUnits[unitId] = self.DomainScene().GetComponent<UnitComponent>().Get(unitId);
        }
        
        public static void RemoveUnit(this Map self, long unitId)
        {
            var mapUnitComponent = self.GetComponent<MapUnitComponent>();

            mapUnitComponent.MapUnits.Remove(unitId);
        }
    }
}
