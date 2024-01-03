using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class LevelUpConfigCategory : ConfigSingleton<LevelUpConfigCategory>, IMerge
    {
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, LevelUpConfig> dict = new Dictionary<int, LevelUpConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<LevelUpConfig> list = new List<LevelUpConfig>();
		
        public void Merge(object o)
        {
            LevelUpConfigCategory s = o as LevelUpConfigCategory;
            this.list.AddRange(s.list);
        }
		
		[ProtoAfterDeserialization]        
        public void ProtoEndInit()
        {
            foreach (LevelUpConfig config in list)
            {
                config.AfterEndInit();
                this.dict.Add(config.Id, config);
            }
            
            this.AfterEndInit();
            this.list.Clear();
        }
		
        public LevelUpConfig Get(int id)
        {
            this.dict.TryGetValue(id, out LevelUpConfig item);

            if (item == null)
            {
                Log.Error($"配置找不到，配置表名: {nameof (LevelUpConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, LevelUpConfig> GetAll()
        {
            return this.dict;
        }

        public LevelUpConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class LevelUpConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>等级</summary>
		[ProtoMember(2)]
		public int Level { get; set; }
		/// <summary>升级经验</summary>
		[ProtoMember(3)]
		public int Exp { get; set; }
		/// <summary>力量</summary>
		[ProtoMember(4)]
		public int Power { get; set; }
		/// <summary>智力</summary>
		[ProtoMember(5)]
		public int Intellect { get; set; }
		/// <summary>体质</summary>
		[ProtoMember(6)]
		public int Physique { get; set; }
		/// <summary>敏捷</summary>
		[ProtoMember(7)]
		public int Agile { get; set; }
		/// <summary>洞察</summary>
		[ProtoMember(8)]
		public int Insight { get; set; }
		/// <summary>血量</summary>
		[ProtoMember(9)]
		public int Hp { get; set; }
		/// <summary>物理攻击</summary>
		[ProtoMember(10)]
		public int Atk { get; set; }
		/// <summary>魔法攻击</summary>
		[ProtoMember(11)]
		public int MAtk { get; set; }
		/// <summary>物理防御</summary>
		[ProtoMember(12)]
		public int Def { get; set; }
		/// <summary>魔法防御</summary>
		[ProtoMember(13)]
		public int MDef { get; set; }

	}
}
