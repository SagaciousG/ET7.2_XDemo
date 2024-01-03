using UnityEngine;
using ET;

namespace ET.Client
{
    [Event(SceneType.Current)]
    public class UnitEvent_AfterCreateMyUnit: AEvent<Event_AfterCreateMyUnit>
    {
        protected override async ETTask Run(Scene scene, Event_AfterCreateMyUnit args)
        {
            Unit unit = await UnitHelper.GetMyUnitFromCurrentScene(scene);

            var cameraComponent = scene.GetComponent<CameraComponent>();
            cameraComponent.Unit = unit;
            
            await ETTask.CompletedTask;
        }
    }
}