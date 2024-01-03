using ET.EventType;
using UnityEngine;

namespace ET.Client
{
    [Event(SceneType.Current)]
    public class OnUnitMove_SyncObjPos: AEvent<ChangePosition>
    {
        protected override async ETTask Run(Scene scene, ChangePosition args)
        {
            Unit unit = args.Unit;
            var gameObjectComponent = unit.GetComponent<GameObjectComponent>();
            Transform transform = gameObjectComponent?.gameObject.transform;
            if (transform == null)
                return;
            transform.position = unit.Position;
            await ETTask.CompletedTask;
        }
    }
    
    [Event(SceneType.Current)]
    public class OnUnitMove_SyncObjRot: AEvent<ChangeRotation>
    {
        protected override async ETTask Run(Scene scene, ChangeRotation args)
        {
            Unit unit = args.Unit;
            var gameObjectComponent = unit.GetComponent<GameObjectComponent>();
            Transform transform = gameObjectComponent?.gameObject.transform;
            if (transform == null)
                return;
            transform.rotation = unit.Rotation;
            await ETTask.CompletedTask;
        }
    }
    
    [Event(SceneType.Current)]
    public class OnUnitMove_MoveStart : AEvent<MoveStart>
    {
        protected override async ETTask Run(Scene scene, MoveStart a)
        {
            var animatorComponent = a.Unit.GetComponent<AnimatorComponent>();
            // animatorComponent?.SetBoolValue(ParametersType.run, true);
            await ETTask.CompletedTask;
        }
    }
    
    [Event(SceneType.Current)]
    public class OnUnitMove_MoveStop : AEvent<MoveStop>
    {
        protected override async ETTask Run(Scene scene, MoveStop a)
        {
            var animatorComponent = a.Unit.GetComponent<AnimatorComponent>();
            // animatorComponent?.SetBoolValue(ParametersType.run, false);
            await ETTask.CompletedTask;
        }
    }

}