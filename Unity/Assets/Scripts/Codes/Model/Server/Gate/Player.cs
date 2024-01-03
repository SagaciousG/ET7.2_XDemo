using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace ET.Server
{
    [ChildOf(typeof(PlayerComponent))]
    public sealed class Player : Entity, IAwake<string>
    {
        public string Account { get; set; }
        [BsonIgnore]
        public long UnitId { get; set; } //当前登录的角色
        [BsonIgnore]
        public Session Session { get; set; }
    }
}