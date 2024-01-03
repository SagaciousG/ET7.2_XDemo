namespace ET.Client
{
    public abstract class AUIEvent
    {
        public abstract ETTask OnAwake(UI ui);
        public abstract void OnCreate(UI ui);
        public abstract void SetData(UI ui, params object[] args);
        public abstract void OnRemove(UI ui);
    }
}