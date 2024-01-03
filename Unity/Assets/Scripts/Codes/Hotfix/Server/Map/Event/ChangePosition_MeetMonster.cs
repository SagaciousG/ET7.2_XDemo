using ET.EventType;

namespace ET.Server
{
    [Event(SceneType.Map)]
    public class ChangePosition_MeetMonster : AEvent<ChangePosition>
    {
        protected override async ETTask Run(Scene scene, ChangePosition a)
        {
            var map = scene.GetComponent<MapComponent>().GetMap(a.Unit.Map);
            var mapMonsterComponent = map.GetComponent<MapMonsterComponent>();
            if (mapMonsterComponent == null)
                return;
            if (a.Unit.GetComponent<UnitInfoComponent>().BeInGateWay > 0)
                return;
            TeamComponent teamComponent = a.Unit.GetComponent<TeamComponent>();
            if (!teamComponent.IsLeader && teamComponent.IsTeaming)
                return;
            var meetMonsterComponent = a.Unit.GetComponent<MeetMonsterComponent>();
            if (TimeHelper.ServerNow() - meetMonsterComponent.LastFightTime < mapMonsterComponent.MeetMonsterTimeMin) 
                return;
            if (TimeHelper.ServerNow() - meetMonsterComponent.MeetDetectionTime < mapMonsterComponent.MonsterGetTimeMin) 
                return;
            var res = RandomGenerator.RandomNumber(0, 3);
            if (res > 0)
                return;
            if (meetMonsterComponent.IsInBattle)
                return;
            meetMonsterComponent.IsInBattle = true;
            meetMonsterComponent.MeetDetectionTime = TimeHelper.ServerNow();
            var groupConfig = mapMonsterComponent.RandomGet();
            await EventSystem.Instance.PublishAsync(scene, new Event_OnEnterBattle(){Unit = a.Unit});
            MessageHelper.SendToClient(a.Unit, new MeetMonsterAMessage(){MonsterGroupId = groupConfig.Id});
        }
    }
}