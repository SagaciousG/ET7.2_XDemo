using System;
using System.Net;
using System.Net.Sockets;

namespace ET.Client
{
    public static class LoginHelper
    {
        public static async ETTask<int> Login(Scene clientScene, string account, string password)
        {
            try
            {
                // 创建一个ETModel层的Session
                clientScene.RemoveComponent<RouterAddressComponent>();
                // 获取路由跟realmDispatcher地址
                RouterAddressComponent routerAddressComponent = clientScene.GetComponent<RouterAddressComponent>();
                if (routerAddressComponent == null)
                {
                    routerAddressComponent = clientScene.AddComponent<RouterAddressComponent, string, int>(ConstValue.RouterHttpHost, ConstValue.RouterHttpPort);
                    await routerAddressComponent.Init();
                    
                    clientScene.RemoveComponent<NetClientComponent>();
                    clientScene.AddComponent<NetClientComponent, AddressFamily>(routerAddressComponent.RouterManagerIPAddress.AddressFamily);
                }
                IPEndPoint realmAddress = routerAddressComponent.GetRealmAddress(account);
                
                var session = await RouterHelper.CreateRouterSession(clientScene, realmAddress);
                var r2CLogin = (LoginResponse) await SessionHelper.Call(session, new LoginRequest() { Account = account, Password = password });
                if (r2CLogin.Error > 0)
                {
                    return r2CLogin.Error;
                }
                clientScene.GetComponent<SessionComponent>().Session = session;
                clientScene.GetComponent<PlayerComponent>().Account = account;
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            return 0;
        }

        public static async ETTask<int> Register(Scene clientScene, string account, string password)
        {
            try
            {
                RegistResponse r2CRegist;
                Session session = null;
                try
                {
                    clientScene.RemoveComponent<RouterAddressComponent>();
                    var routerAddressComponent = clientScene.GetComponent<RouterAddressComponent>();
                    if (routerAddressComponent == null)
                    {
                        routerAddressComponent =
                                clientScene.AddComponent<RouterAddressComponent, string, int>(ConstValue.RouterHttpHost, ConstValue.RouterHttpPort);
                        await routerAddressComponent.Init();

                        clientScene.RemoveComponent<NetClientComponent>();
                        clientScene.AddComponent<NetClientComponent, AddressFamily>(routerAddressComponent.RouterManagerIPAddress.AddressFamily);
                    }

                    IPEndPoint realmAddress = routerAddressComponent.GetRealmAddress(account);

                    session = await RouterHelper.CreateRouterSession(clientScene, realmAddress);
                    {
                        r2CRegist = (RegistResponse)await SessionHelper.Call(session, new RegistRequest() { Account = account, Password = password });
                    }
                    return r2CRegist.Error;
                }
                finally
                {
                    session?.Dispose();
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }

            return 0;
        }

        public static async ETTask<int> LoginToTest(Scene clientScene, string account)
        {
            var error = await Login(clientScene, account, "123");
            if (error == ErrorCode.ERR_AccountOrPwNotExist)
            {
                await Register(clientScene, account, "123");
                error = await Login(clientScene, account, "123");
            }
            if (error > 0)
                return error;
            error = await LobbyHelper.EnterZone(clientScene, 999);
            if (error > 0)
                return error;
            await EventSystem.Instance.PublishAsync(clientScene, new LoginToTestFinish());
            return 0;
        }
    }
}