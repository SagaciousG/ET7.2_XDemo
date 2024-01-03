using System.Collections.Generic;
using System.Numerics;
using ET.EventType;

namespace ET.Client
{
    [Event(SceneType.Client)]
    public class UIMainEvent_OnBagUpdate : AEvent<Event_OnBagUpdate>
    {
        protected override async ETTask Run(Scene scene, Event_OnBagUpdate a)
        {
            var uiBagComponent = UIHelper.GetUIComponent<UIBagComponent>(scene.CurrentScene(), UIType.UIBag);
            uiBagComponent?.RefreshShowList();
            await ETTask.CompletedTask;
        }
    }
    
    [Event(SceneType.Current)]
    public class UIMainEvent_UpdateDialog : AEvent<ChangePosition>
    {
        protected override async ETTask Run(Scene scene, ChangePosition a)
        {
            var myUnit = await UnitHelper.GetMyUnitFromCurrentScene(scene);
            if (a.Unit != myUnit)            
                return;
            var unitComponent = myUnit.GetParent<UnitComponent>();
            var list = new List<Unit>();
            foreach (var unit in unitComponent.Units)
            {
                if (unit == myUnit)
                    continue;
                if (unit.Position.Distance(myUnit.Position) < 2)
                {
                    list.Add(unit);
                }
            }
            var uiMainComponent = UIHelper.GetUIComponent<UIMainComponent>(scene, UIType.UIMain);
            uiMainComponent.SetDialog(list);
            await ETTask.CompletedTask;
        }
    }
}