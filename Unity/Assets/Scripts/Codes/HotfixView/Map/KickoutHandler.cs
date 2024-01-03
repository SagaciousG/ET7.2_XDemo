using ET;

namespace ET.Client
{
    [MessageHandler(SceneType.Client)]
    public class KickoutHandler : AMHandler<KickOutMessage>
    {
        protected override async ETTask Run(Session session, KickOutMessage message)
        {
            await UIDialog.Show(ErrorCodeHelper.GetCodeTips(message.Code));
        }
    }
}