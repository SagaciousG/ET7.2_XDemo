using MongoDB.Bson.Serialization.Attributes;

namespace ET.Server
{
    [ChildOf(typeof(RobotManagerComponent))]
    public class Robot : Entity, ISerializeToEntity, IAwake
    {
        public static implicit operator RobotInfo(Robot robot)
        {
            return new RobotInfo() { Name = robot.Name, Account = robot.Id, Online = robot.Online };
        }
        public string Name { get; set; }
        public int UnitShow { get; set; }
        public long Password { get; set; }
        public long UnitID { get; set; }
        [BsonIgnore]
        public int Online { get; set; }
    }
}