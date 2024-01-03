namespace ET.Server
{
    [ChildOf(typeof(RobotManagerComponent))]
    public class RobotAccount: Entity, IAwake<string, string>
    {
        public string Account { get; set; }
        public string Password { get; set; }
    }
}