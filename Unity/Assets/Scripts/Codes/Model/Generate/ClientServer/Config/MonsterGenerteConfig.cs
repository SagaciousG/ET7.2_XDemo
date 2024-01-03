using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class MonsterGenerteConfigCategory : ConfigSingleton<MonsterGenerteConfigCategory>, IMerge
    {
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, MonsterGenerteConfig> dict = new Dictionary<int, MonsterGenerteConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<MonsterGenerteConfig> list = new List<MonsterGenerteConfig>();
		
        public void Merge(object o)
        {
            MonsterGenerteConfigCategory s = o as MonsterGenerteConfigCategory;
            this.list.AddRange(s.list);
        }
		
		[ProtoAfterDeserialization]        
        public void ProtoEndInit()
        {
            foreach (MonsterGenerteConfig config in list)
            {
                config.AfterEndInit();
                this.dict.Add(config.Id, config);
            }
            
            this.AfterEndInit();
            this.list.Clear();
        }
		
        public MonsterGenerteConfig Get(int id)
        {
            this.dict.TryGetValue(id, out MonsterGenerteConfig item);

            if (item == null)
            {
                Log.Error($"配置找不到，配置表名: {nameof (MonsterGenerteConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, MonsterGenerteConfig> GetAll()
        {
            return this.dict;
        }

        public MonsterGenerteConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class MonsterGenerteConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>怪物类型</summary>
		[ProtoMember(2)]
		public int Type { get; set; }
		/// <summary>名字</summary>
		[ProtoMember(3)]
		public string Name { get; set; }
		/// <summary>模型</summary>
		[ProtoMember(4)]
		public string Model { get; set; }
		/// <summary>魔法攻击</summary>
		[ProtoMember(5)]
		public int MagicAttack { get; set; }
		/// <summary>物理攻击</summary>
		[ProtoMember(6)]
		public int Attack { get; set; }
		/// <summary>出手次数</summary>
		[ProtoMember(7)]
		public int AttackNum { get; set; }
		/// <summary>出手速度</summary>
		[ProtoMember(8)]
		public int AttackSpeed { get; set; }
		/// <summary>魔法防御</summary>
		[ProtoMember(9)]
		public int MagicDefense { get; set; }
		/// <summary>物理防御</summary>
		[ProtoMember(10)]
		public int Defense { get; set; }
		/// <summary>命中值</summary>
		[ProtoMember(11)]
		public int HitRate { get; set; }
		/// <summary>闪避值</summary>
		[ProtoMember(12)]
		public int Dodge { get; set; }
		/// <summary>洞察力</summary>
		[ProtoMember(13)]
		public int InsightValue { get; set; }
		/// <summary>血量</summary>
		[ProtoMember(14)]
		public int Heath { get; set; }
		/// <summary>蓝量</summary>
		[ProtoMember(15)]
		public int MagicPower { get; set; }
		/// <summary>魔法穿透</summary>
		[ProtoMember(16)]
		public int MagicPenetration { get; set; }
		/// <summary>护甲穿透</summary>
		[ProtoMember(17)]
		public int DefensePenetration { get; set; }
		/// <summary>格挡</summary>
		[ProtoMember(18)]
		public int Parry { get; set; }
		/// <summary>反震值</summary>
		[ProtoMember(19)]
		public int TheShock { get; set; }
		/// <summary>魔法反噬</summary>
		[ProtoMember(20)]
		public int Backfire { get; set; }
		/// <summary>暴击率</summary>
		[ProtoMember(21)]
		public int Critical { get; set; }
		/// <summary>暴击抵抗</summary>
		[ProtoMember(22)]
		public int CritResistance { get; set; }

	}
}
