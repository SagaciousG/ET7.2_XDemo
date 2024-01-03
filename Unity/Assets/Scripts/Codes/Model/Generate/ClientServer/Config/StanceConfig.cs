using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class StanceConfigCategory : ConfigSingleton<StanceConfigCategory>, IMerge
    {
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, StanceConfig> dict = new Dictionary<int, StanceConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<StanceConfig> list = new List<StanceConfig>();
		
        public void Merge(object o)
        {
            StanceConfigCategory s = o as StanceConfigCategory;
            this.list.AddRange(s.list);
        }
		
		[ProtoAfterDeserialization]        
        public void ProtoEndInit()
        {
            foreach (StanceConfig config in list)
            {
                config.AfterEndInit();
                this.dict.Add(config.Id, config);
            }
            
            this.AfterEndInit();
            this.list.Clear();
        }
		
        public StanceConfig Get(int id)
        {
            this.dict.TryGetValue(id, out StanceConfig item);

            if (item == null)
            {
                Log.Error($"配置找不到，配置表名: {nameof (StanceConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, StanceConfig> GetAll()
        {
            return this.dict;
        }

        public StanceConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class StanceConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>地图Id</summary>
		[ProtoMember(2)]
		public int MapId { get; set; }
		/// <summary>位置编号</summary>
		[ProtoMember(3)]
		public int Pos { get; set; }
		/// <summary>坐标</summary>
		[ProtoMember(4)]
		public string Position { get; set; }
		/// <summary>旋转</summary>
		[ProtoMember(5)]
		public string Rotation { get; set; }

	}
}
