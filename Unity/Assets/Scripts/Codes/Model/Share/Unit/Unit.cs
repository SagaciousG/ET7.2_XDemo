using System.Diagnostics;
using MongoDB.Bson.Serialization.Attributes;
using Unity.Mathematics;

namespace ET
{
    [ChildOf(typeof(UnitComponent))]
    [DebuggerDisplay("ViewName")]
    public partial class Unit: Entity, IAwake
    {
        public UnitType Type { get; set; }
        public ProfessionNum ProfessionNum { get; set; }

        [BsonElement]
        public float3 position; //坐标
        [BsonElement]
        public string name;
        [BsonElement]
        public int unitShow;
        [BsonElement]
        public quaternion rotation;
        [BsonElement]
        public int map;    
        [BsonElement]
        public int moveSpeed;
        [BsonElement]
        public int aoi;
       
        protected override string ViewName
        {
            get
            {
                return $"{this.GetType().Name} ({this.Id})";
            }
        }
    }
}