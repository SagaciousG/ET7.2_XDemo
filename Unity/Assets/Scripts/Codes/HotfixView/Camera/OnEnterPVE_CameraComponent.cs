namespace ET.Client
{
    [ActiveEvent(typeof(CameraComponent), ActiveCode.EnterPVE)]
    public class OnEnterPVE_CameraComponent : IActiveEvent
    {
        public void Run(Entity entity, bool active)
        {
            var cameraComponent = entity as CameraComponent;
        }
    }
}