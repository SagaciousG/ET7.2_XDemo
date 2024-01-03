namespace ET.Client
{
    public static class SessionHelper
    {
        public static async ETTask Call(Scene clientScene, IRequest request)
        {
            await Call(clientScene.GetComponent<SessionComponent>().Session, request);
        }
        
        public static async ETTask<T> Call<T>(Scene clientScene, IRequest request) where T : IResponse
        {
            return (T) await Call(clientScene.GetComponent<SessionComponent>().Session, request);
        }

        public static void Send(Scene clientScene, IMessage message)
        {
            clientScene.GetComponent<SessionComponent>().Session.Send(message);
            OpcodeHelper.LogMsg(clientScene.Zone, 
                0, message, true, true
            );
        }

        public static ETTask<IResponse> Call(Session session, IRequest request)
        {
            OpcodeHelper.LogMsg(session.DomainZone(), 
                0, request, true, true
            );
            return session.Call(request);
        }

    }
}