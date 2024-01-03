using System;
using System.Linq;

namespace ET.Server
{
    [FriendOf(typeof(RobotManagerComponent))]
    public static class RobotManagerComponentSystem
    {
        public class RobotManagerComponentAwakeSystem : AwakeSystem<RobotManagerComponent>
        {
            protected override async void Awake(RobotManagerComponent self)
            {
                var dbComponent = self.DomainScene().GetComponent<DBComponent>();
                var all = await dbComponent.Query<Robot>(a => true, nameof(RobotManagerComponent));
                foreach (var info in all)
                {
                    self.AddChild(info);
                }
            }
        }
        
        
        public static async ETTask<Robot> NewRobot(this RobotManagerComponent self)
        {
            var account = RandomGenerator.RandInt64();
            var password = RandomGenerator.RandInt64();
            var dbComponent = self.DomainScene().GetComponent<DBComponent>();
            var robot = self.AddChildWithId<Robot>(account);
            robot.Password = password;
            var nameComponent = self.DomainScene().GetComponent<UnitNameComponent>();
            robot.Name = nameComponent.RandomGet();
            robot.UnitShow = RandomGenerator.RandomArray(UnitShowConfigCategory.Instance.GetAll().Keys.ToArray());
            robot.UnitID = IdGenerater.Instance.GenerateUnitId(self.DomainScene().Zone);
            nameComponent.Use(robot.Name);
            nameComponent.Save().Coroutine();
            await dbComponent.Save(robot);
            var login = await MessageHelper.CallActor(self.DomainScene().GetComponent<GateComponent>().MapActorID,
                new G2M_LoginRobotToMapARequest() { 
                    Robot = new SimpleUnit()
                    {
                        UnitId = robot.UnitID,
                        Name = robot.Name,
                        UnitShow = robot.UnitShow,
                        UnitType = (int)UnitType.Robot
                    },
                });
            robot.Online = 1;
            return robot;
        }

        public static async ETTask LoginRobot(this RobotManagerComponent self, long account)
        {
            var robot = self.GetChild<Robot>(account);
            robot.Online = 1;
            var login = await MessageHelper.CallActor(self.DomainScene().GetComponent<GateComponent>().MapActorID,
                new G2M_LoginRobotToMapARequest() { 
                    Robot = new SimpleUnit()
                    {
                        UnitId = robot.UnitID,
                        Name = robot.Name,
                        UnitShow = robot.UnitShow,
                        UnitType = (int)UnitType.Robot
                    },
                });
        }
    }
}