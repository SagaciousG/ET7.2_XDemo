using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class UnitConfigCategory : ConfigSingleton<UnitConfigCategory>, IMerge
    {
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, UnitConfig> dict = new Dictionary<int, UnitConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<UnitConfig> list = new List<UnitConfig>();
		
        public void Merge(object o)
        {
            UnitConfigCategory s = o as UnitConfigCategory;
            this.list.AddRange(s.list);
        }
		
		[ProtoAfterDeserialization]        
        public void ProtoEndInit()
        {
            foreach (UnitConfig config in list)
            {
                config.AfterEndInit();
                this.dict.Add(config.Id, config);
            }
            this.list.Clear();
            
            this.AfterEndInit();
        }
		
        public UnitConfig Get(int id)
        {
            this.dict.TryGetValue(id, out UnitConfig item);

            if (item == null)
            {
                Log.Error($"配置找不到，配置表名: {nameof (UnitConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, UnitConfig> GetAll()
        {
            return this.dict;
        }

        public UnitConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class UnitConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Unit类型</summary>
		[ProtoMember(2)]
		public int Type { get; set; }
		/// <summary>外观</summary>
		[ProtoMember(3)]
		public int Skin { get; set; }
		/// <summary>移动速度</summary>
		[ProtoMember(4)]
		public float MoveSpeed { get; set; }
		/// <summary>力量</summary>
		[ProtoMember(5)]
		public int Power { get; set; }
		/// <summary>智力</summary>
		[ProtoMember(6)]
		public int Intellect { get; set; }
		/// <summary>体质</summary>
		[ProtoMember(7)]
		public int Physique { get; set; }
		/// <summary>敏捷</summary>
		[ProtoMember(8)]
		public int Agile { get; set; }
		/// <summary>洞察</summary>
		[ProtoMember(9)]
		public int Insight { get; set; }

	}
}
