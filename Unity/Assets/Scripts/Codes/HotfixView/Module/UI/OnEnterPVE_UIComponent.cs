namespace ET.Client
{
    [ActiveEvent(typeof(UIComponent), ActiveCode.EnterPVE)]
    [FriendOf(typeof(UIComponent))]
    public class OnEnterPVE_UIComponent : IActiveEvent
    {
        public void Run(Entity entity, bool active)
        {
            var uiComponent = entity as UIComponent;
            foreach (UI ui in uiComponent.UIs.Values)
            {
                ui.GameObject.SetActive(active);   
            }
        }
    }
}