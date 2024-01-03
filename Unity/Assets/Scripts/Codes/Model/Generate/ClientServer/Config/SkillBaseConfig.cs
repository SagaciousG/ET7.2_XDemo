using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class SkillBaseConfigCategory : ConfigSingleton<SkillBaseConfigCategory>, IMerge
    {
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, SkillBaseConfig> dict = new Dictionary<int, SkillBaseConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<SkillBaseConfig> list = new List<SkillBaseConfig>();
		
        public void Merge(object o)
        {
            SkillBaseConfigCategory s = o as SkillBaseConfigCategory;
            this.list.AddRange(s.list);
        }
		
		[ProtoAfterDeserialization]        
        public void ProtoEndInit()
        {
            foreach (SkillBaseConfig config in list)
            {
                config.AfterEndInit();
                this.dict.Add(config.Id, config);
            }
            
            this.AfterEndInit();
            this.list.Clear();
        }
		
        public SkillBaseConfig Get(int id)
        {
            this.dict.TryGetValue(id, out SkillBaseConfig item);

            if (item == null)
            {
                Log.Error($"配置找不到，配置表名: {nameof (SkillBaseConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, SkillBaseConfig> GetAll()
        {
            return this.dict;
        }

        public SkillBaseConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class SkillBaseConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>组</summary>
		[ProtoMember(2)]
		public int Group { get; set; }
		/// <summary>等级</summary>
		[ProtoMember(3)]
		public int Level { get; set; }
		/// <summary>消耗</summary>
		[ProtoMember(4)]
		public int ConsumeType { get; set; }
		/// <summary>消耗值</summary>
		[ProtoMember(5)]
		public int ConsumeVal { get; set; }
		/// <summary>目标选择</summary>
		[ProtoMember(6)]
		public int SelectTarget { get; set; }
		/// <summary>词条</summary>
		[ProtoMember(7)]
		public string EntryWord { get; set; }
		/// <summary>词条参数</summary>
		[ProtoMember(8)]
		public string EntryWordArg { get; set; }
		/// <summary>学习消耗</summary>
		[ProtoMember(9)]
		public int TakeSP { get; set; }

	}
}
