using System.Collections.Generic;

namespace ET.Server
{
    [FriendOf(typeof(UnitInfoComponent))]
    public static class UnitSystem
    {
        public class UnitDestroySystem : AwakeSystem<Unit>
        {
            protected override void Awake(Unit self)
            {
                if (self.Type == UnitType.Player)
                {
                    var timer = self.GetComponent<UnitInfoComponent>().LoopSave;
                    TimerComponent.Instance.Remove(ref timer);
                    self.GetComponent<UnitInfoComponent>().LoopSave = timer;
                }
            }
        }

        public static void StartSaveTimer(this Unit self)
        {
            if (self.Type == UnitType.Player)
            {
                var unitInfoComponent = self.GetComponent<UnitInfoComponent>();
                unitInfoComponent.LoopSave = TimerComponent.Instance.NewRepeatedTimer(60 * 1000, TimerInvokeType.UnitSaveData, self);
            }
        }

        public static void SetDirty(this Unit unit)
        {
            unit.IsDirty = true;
        }
        
        public static void SaveAsync(this Unit unit)
        {
            var dbComponent = unit.DomainScene().GetComponent<DBComponent>();
            dbComponent.Save(unit).Coroutine();
            unit.IsDirty = false;
        }
    }
}