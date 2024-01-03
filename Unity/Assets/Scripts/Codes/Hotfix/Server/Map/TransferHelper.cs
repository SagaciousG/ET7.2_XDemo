using Unity.Mathematics;

namespace ET.Server
{
    public static class TransferHelper
    {
        public static async ETTask Transfer(Unit unit, int mapId, float3 position, quaternion rotation)
        {
            // 通知客户端开始切场景
            MessageHelper.SendToClient(unit, new StartSceneChangeAMessage(){MapId = mapId, MapActorId = unit.DomainScene().InstanceId});
            var oldMap = unit.DomainScene().GetComponent<MapComponent>().GetMap(unit.Map);
            oldMap.RemoveUnit(unit.Id);
            unit.Stop(0);
            unit.RemoveComponent<AOIEntity>();
            unit.RemoveComponent<PathfindingComponent>();
            
            var newMap = unit.DomainScene().GetComponent<MapComponent>().GetMap(mapId);
            newMap.AddUnit(unit.Id);
            unit.Map = mapId;
            unit.Position = position;
            unit.Rotation = rotation;
            unit.AddComponent<AOIEntity, int, float3>(9 * 1000, position);
            unit.AddComponent<PathfindingComponent, string>(newMap.MapConfig.SceneName);
            unit.GetComponent<MeetMonsterComponent>().MeetDetectionTime = TimeHelper.ServerNow();
            
            // 通知客户端创建My Unit
            CreateMyUnitAMessage m2CCreateUnits = new CreateMyUnitAMessage();
            m2CCreateUnits.Unit = ET.UnitHelper.CreateUnitInfo(unit);
            MessageHelper.SendToClient(unit, m2CCreateUnits);
            await ETTask.CompletedTask;
        }
        
        public static async ETTask RobotTransfer(Unit unit, int mapId, float3 position, quaternion rotation)
        {
            var oldMap = unit.DomainScene().GetComponent<MapComponent>().GetMap(unit.Map);
            oldMap.RemoveUnit(unit.Id);
            unit.Stop(0);
            unit.RemoveComponent<AOIEntity>();
            unit.RemoveComponent<PathfindingComponent>();
            
            var newMap = unit.DomainScene().GetComponent<MapComponent>().GetMap(mapId);
            newMap.AddUnit(unit.Id);
            unit.Map = mapId;
            unit.Position = position;
            unit.Rotation = rotation;
            unit.AddComponent<AOIEntity, int, float3>(9 * 1000, position);
            unit.AddComponent<PathfindingComponent, string>(newMap.MapConfig.SceneName);
            unit.GetComponent<MeetMonsterComponent>().MeetDetectionTime = TimeHelper.ServerNow();
            await ETTask.CompletedTask;
        }
    }
}