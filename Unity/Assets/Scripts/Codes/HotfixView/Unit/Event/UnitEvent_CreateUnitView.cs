using UnityEngine;
using ET;

namespace ET.Client
{
    [Event(SceneType.Current)]
    public class UnitEvent_CreateUnitView: AEvent<Event_AfterUnitCreate>
    {
        protected override async ETTask Run(Scene scene, Event_AfterUnitCreate args)
        {
            Unit unit = args.Unit;
            var unitShowConfig = UnitShowConfigCategory.Instance.Get(unit.UnitShow);
            var go = await unit.AddComponent<GameObjectComponent, string>(unitShowConfig.Name).Load(unitShowConfig.Model);
            if (go == null)
                return;
            go.transform.position = unit.Position;
            go.layer = LayerMask.NameToLayer("Unit");
            go.tag = "Role";
            var unitInfo = go.AddComponentNotOwns<UnitInfo>();
            unitInfo.Type = (int)unit.Type;
            unitInfo.Id = unit.Id;
            unit.AddComponent<AnimatorComponent>();

            await ETTask.CompletedTask;
        }
    }
}