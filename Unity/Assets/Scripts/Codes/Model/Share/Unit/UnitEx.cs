using System.Diagnostics;
using MongoDB.Bson.Serialization.Attributes;
using Unity.Mathematics;

namespace ET
{
    public partial class Unit
    {
        [BsonIgnore]
        public int Level { get; set; }


        [BsonIgnore]
        public int UnitShow
        {
            get => unitShow;
            set
            {
                IsDirty = true;
                unitShow = value;
            }
        }


        [BsonIgnore]
        public string Name
        {
            get => name;
            set
            {
                IsDirty = true;
                name = value;
            }
        }

        [BsonIgnore]
        public float3 Position
        {
            get => this.position;
            set
            {
                IsDirty = true;
                float3 oldPos = this.position;
                this.position = value;
                EventSystem.Instance.Publish(this.DomainScene(), new ChangePosition() { Unit = this, OldPos = oldPos });
            }
        }

        [BsonIgnore]
        public float3 Forward
        {
            get => math.mul(this.Rotation, math.forward());
            set
            {
                IsDirty = true;
                this.Rotation = quaternion.LookRotation(value, math.up());
            }
        }

        [BsonIgnore]
        public quaternion Rotation
        {
            get => this.rotation;
            set
            {
                IsDirty = true;
                this.rotation = value;
                EventSystem.Instance.Publish(this.DomainScene(), new ChangeRotation() { Unit = this });
            }
            
            
        }
        
        [BsonIgnore]
        public int Map
        {
            get => this.map;
            set
            {
                IsDirty = true;
                this.map = value;
            }
        }

        [BsonIgnore]
        public int MoveSpeed
        {
            get => this.moveSpeed;
            set => this.moveSpeed = value;
        }

        [BsonIgnore]
        public int AOI
        {
            get => this.aoi;
            set => this.aoi = value;
        }

        [BsonIgnore]
        public bool IsDirty { get; set; }
        
    }
}