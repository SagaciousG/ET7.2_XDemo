namespace ET.Server
{
    [Invoke(TimerInvokeType.UnitSaveData)]
    public class UnitTimer_SaveUnit : ATimer<Unit>
    {
        protected override void Run(Unit t)
        {
            if (t.IsDirty)
            {
                t.SaveAsync();
            }
        }
    }
}