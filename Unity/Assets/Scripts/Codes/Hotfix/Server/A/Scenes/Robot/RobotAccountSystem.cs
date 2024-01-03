namespace ET.Server
{
    public class RobotAccountAwakeSystem : AwakeSystem<RobotAccount, string, string>
    {
        protected override void Awake(RobotAccount self, string a, string b)
        {
            self.Account = a;
            self.Password = b;
        }
    }
}