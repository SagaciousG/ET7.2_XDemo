

using System.Collections.Generic;
using System.IO;

namespace ET.Server
{
    public static class MessageHelper
    {
        public static void NoticeUnitAdd(Unit unit, Unit sendUnit)
        {
            if (unit.Type == UnitType.Player)
            {
                var createUnits = new CreateUnitsAMessage(){Units = new()};
                createUnits.Units.Add(ET.UnitHelper.CreateUnitInfo(sendUnit));
                SendToClient(unit, createUnits);
            }
        }
        
        public static void NoticeUnitRemove(Unit unit, Unit sendUnit)
        {
            if (unit.Type == UnitType.Player)
            {
                var removeUnits = new RemoveUnitsAMessage
                {
                    Units = new() { sendUnit.Id }
                };
                SendToClient(unit, removeUnits);
            }
        }
        
        public static void Broadcast(Unit unit, IActorMessage message)
        {
            Dictionary<long, AOIEntity> dict = unit.GetBeSeePlayers();
            // 网络底层做了优化，同一个消息不会多次序列化
            foreach (AOIEntity u in dict.Values)
            {
                SendToClient(u.Unit, message);
            }
        }
        
        public static void SendToClient(Unit unit, IActorMessage message)
        {
            if (unit.Type != UnitType.Player)
                return;
            if (!unit.GetComponent<UnitInfoComponent>().IsOnline)
                return;
            var gateComponent = unit.GetComponent<UnitGateComponent>();
            if (gateComponent != null)
                SendActor(gateComponent.GateSessionActorID, message);
        }
        
        public static void SendToClient(Unit unit, IActorRequest message)
        {
            var gateComponent = unit.GetComponent<UnitGateComponent>();
            if (gateComponent != null)
                ActorMessageSenderComponent.Instance.Send(gateComponent.GateSessionActorID, message);
        }
        
        
        /// <summary>
        /// 发送协议给ActorLocation
        /// </summary>
        /// <param name="id">注册Actor的EntityId</param>
        /// <param name="message"></param>
        public static void SendToLocationActor(long id, IActorLocationMessage message)
        {
            ActorLocationSenderComponent.Instance.Send(id, message);
        }
        
        /// <summary>
        /// 发送协议给Actor
        /// </summary>
        /// <param name="actorId">注册Actor的InstanceId</param>
        /// <param name="message"></param>
        public static void SendActor(long actorId, IActorMessage message)
        {
            ActorMessageSenderComponent.Instance.Send(actorId, message);
        }
        
        /// <summary>
        /// 发送RPC协议给Actor
        /// </summary>
        /// <param name="actorId">注册Actor的InstanceId</param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async ETTask<IActorResponse> CallActor(long actorId, IActorRequest message)
        {
            return await ActorMessageSenderComponent.Instance.Call(actorId, message);
        }

        
        /// <summary>
        /// 发送RPC协议给ActorLocation
        /// </summary>
        /// <param name="id">注册Actor的Id</param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async ETTask<IActorResponse> CallLocationActor(long id, IActorLocationRequest message)
        {
            return await ActorLocationSenderComponent.Instance.Call(id, message);
        }
        
        public static async ETTask<T> CallCenter<T>(ICenterRequest request) where T : ICenterResponse
        {
            var response = (T) await CallCenter(request);
            return response;
        }
        
        
        public static async ETTask<ICenterResponse> CallCenter(ICenterRequest request)
        {
            var center = StartSceneConfigCategory.Instance.GetByCenterName("Center");
            var response = await CallActor(center.InstanceId, request);
            return (ICenterResponse) response;
        }
        
        public static void SendCenter(ICenterMessage message) 
        {
            var center = StartSceneConfigCategory.Instance.GetByCenterName("Center");
            SendActor(center.InstanceId, message);
        }
        
    }
}