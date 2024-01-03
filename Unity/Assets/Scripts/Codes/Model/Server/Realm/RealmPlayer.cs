namespace ET.Server
{

    [ComponentOf(typeof(Session))]
    public class RealmPlayer : Entity, IAwake, IDestroy
    {
        public string Account { get; set; }
    }

}