namespace ET.Client
{
    public static class UnitHelper
    {
        public static async ETTask<Unit> GetMyUnitFromClientScene(Scene clientScene)
        {
            PlayerComponent playerComponent = clientScene.GetComponent<PlayerComponent>();
            Scene currentScene = clientScene.GetComponent<CurrentScenesComponent>().Current;
            var error = await currentScene.Wait(WaiterKey.CreateMyUnit);
            if (error > 0)
            {
                Log.Error(ErrorCodeHelper.GetCodeTips(error));
                return null;
            }

            return currentScene.GetComponent<UnitComponent>().Get(playerComponent.MyUnitId);
        }
        
        public static async ETTask<Unit> GetMyUnitFromCurrentScene(Scene currentScene)
        {
            PlayerComponent playerComponent = currentScene.ClientScene().GetComponent<PlayerComponent>();
            var error = await currentScene.Wait(WaiterKey.CreateMyUnit);
            if (error > 0)
            {
                Log.Error(ErrorCodeHelper.GetCodeTips(error));
                return null;
            }
            return currentScene.GetComponent<UnitComponent>().Get(playerComponent.MyUnitId);
        }
        
        public static Unit GetUnitFromClientScene(Scene clientScene, long id)
        {
            Scene currentScene = clientScene.GetComponent<CurrentScenesComponent>().Current;
            return currentScene.GetComponent<UnitComponent>().Get(id);
        }
        
    }
}