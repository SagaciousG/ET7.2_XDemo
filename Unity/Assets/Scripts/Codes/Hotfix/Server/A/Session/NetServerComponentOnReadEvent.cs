namespace ET.Server
{
    [Event(SceneType.Process)]
    public class NetServerComponentOnReadEvent: AEvent<NetServerComponentOnRead>
    {
        protected override async ETTask Run(Scene scene, NetServerComponentOnRead args)
        {
            Session session = args.Session;
            object message = args.Message;

            if (message is IResponse response)
            {
                session.OnResponse(response);
                return;
            }
			
            // 根据消息接口判断是不是Actor消息，不同的接口做不同的处理,比如需要转发给Chat Scene，可以做一个IChatMessage接口
            switch (message)
            {
                case IUnitRequest unitRequest: // gate session收到actor rpc消息，先向actor 发送rpc请求，再将请求结果返回客户端
                {
                    long unitId = session.GetComponent<SessionPlayerComponent>().GetMyPlayer().UnitId;
                    int rpcId = unitRequest.RpcId; // 这里要保存客户端的rpcId
                    long instanceId = session.InstanceId;
                    IResponse iResponse = await MessageHelper.CallLocationActor(unitId, unitRequest);
                    iResponse.RpcId = rpcId;
                    // session可能已经断开了，所以这里需要判断
                    if (session.InstanceId == instanceId)
                    {
                        session.Send(iResponse);
                    }
                    break;
                }
                case IUnitMessage unitMessage:
                {
                    long unitId = session.GetComponent<SessionPlayerComponent>().GetMyPlayer().UnitId;
                    MessageHelper.SendToLocationActor(unitId, unitMessage);
                    break;
                }
                case ICenterMessage centerMessage:
                {
                    MessageHelper.SendCenter(centerMessage);
                    break;
                }
                case ICenterRequest centerRequest:
                {
                    int rpcId = centerRequest.RpcId; // 这里要保存客户端的rpcId
                    long instanceId = session.InstanceId;
                    var centerResponse = await MessageHelper.CallCenter(centerRequest);
                    centerResponse.RpcId = rpcId;
                    if (session.InstanceId == instanceId)
                    {
                        session.Send(centerResponse);
                    }
                    break;
                }
                case IActorRequest actorRequest:  // 分发IActorRequest消息，目前没有用到，需要的自己添加
                {
                    break;
                }
                case IActorMessage actorMessage:  // 分发IActorMessage消息，目前没有用到，需要的自己添加
                {
                    break;
                }
				
                default:
                {
                    // 非Actor消息
                    MessageDispatcherComponent.Instance.Handle(session, message);
                    break;
                }
            }
        }
    }
}