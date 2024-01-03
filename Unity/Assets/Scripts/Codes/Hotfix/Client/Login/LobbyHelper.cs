using System;

namespace ET.Client
{
    public static class LobbyHelper
    {
        public static async ETTask<int> EnterZone(Scene scene, int zone)
        {
            var realmSession = scene.GetComponent<SessionComponent>().Session;
            var gateKey = (GetLoginGateKeyResponse) await realmSession.Call(new GetLoginGateKeyRequest(){Zone = zone, Account = scene.GetComponent<PlayerComponent>().Account});
            
            Session gateSession = await RouterHelper.CreateRouterSession(scene, NetworkHelper.ToIPEndPoint(gateKey.Address));
            var loginGateResponse = (LoginGateResponse) await SessionHelper.Call(gateSession,
                new LoginGateRequest() { Key = gateKey.Key});
            if (loginGateResponse.Error > 0)
            {
                return loginGateResponse.Error;
            }
            realmSession.Dispose();
            scene.GetComponent<SessionComponent>().Session = gateSession;
            scene.GetComponent<PlayerComponent>().MyId = loginGateResponse.PlayerId;
            scene.GetComponent<PlayerComponent>().GetComponent<PlayerZoneListComponent>().Dispose();
            return 0;
        }
        
        public static async ETTask<int> CreateRole(Scene scene, int showId, string name)
        {
            var nameExistResponse = await SessionHelper.Call<NameExistResponse>(scene, new NameExistRequest() { Name = name });
            if (nameExistResponse.Error > 0)
            {
                return nameExistResponse.Error;
            }
            var response = await SessionHelper.Call<CreateRoleResponse>(scene,
                new CreateRoleRequest()
                {
                    ShowID = showId,
                    Name = name
                });
            return response.Error;
        }

        public static async ETTask<string> RandomName(Scene scene)
        {
            var nameResponse = await SessionHelper.Call<GetNameResponse>(scene, new GetNameRequest());
            return nameResponse.Name;
        }
        
        public static async ETTask SelectRole(Scene scene, long unitId)
        {
            await SessionHelper.Call<SelectRoleResponse>(scene, new SelectRoleRequest() { UnitId = unitId });
            scene.GetComponent<PlayerComponent>().MyUnitId = unitId;
        }
    }
}