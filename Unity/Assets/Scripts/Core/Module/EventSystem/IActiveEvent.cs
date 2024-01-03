namespace ET
{
    public interface IActiveEvent
    {
        void Run(Entity entity, bool active);
    }
}