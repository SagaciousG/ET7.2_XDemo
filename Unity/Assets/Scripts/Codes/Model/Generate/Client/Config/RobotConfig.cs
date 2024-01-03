using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class RobotConfigCategory : ConfigSingleton<RobotConfigCategory>, IMerge
    {
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, RobotConfig> dict = new Dictionary<int, RobotConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<RobotConfig> list = new List<RobotConfig>();
		
        public void Merge(object o)
        {
            RobotConfigCategory s = o as RobotConfigCategory;
            this.list.AddRange(s.list);
        }
		
		[ProtoAfterDeserialization]        
        public void ProtoEndInit()
        {
            foreach (RobotConfig config in list)
            {
                config.AfterEndInit();
                this.dict.Add(config.Id, config);
            }
            this.list.Clear();
            
            this.AfterEndInit();
        }
		
        public RobotConfig Get(int id)
        {
            this.dict.TryGetValue(id, out RobotConfig item);

            if (item == null)
            {
                Log.Error($"配置找不到，配置表名: {nameof (RobotConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, RobotConfig> GetAll()
        {
            return this.dict;
        }

        public RobotConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class RobotConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>外观</summary>
		[ProtoMember(2)]
		public int Skin { get; set; }
		/// <summary>移动速度</summary>
		[ProtoMember(3)]
		public float MoveSpeed { get; set; }

	}
}
