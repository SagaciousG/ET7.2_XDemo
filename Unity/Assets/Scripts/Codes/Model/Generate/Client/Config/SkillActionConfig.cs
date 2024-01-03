using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class SkillActionConfigCategory : ConfigSingleton<SkillActionConfigCategory>, IMerge
    {
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, SkillActionConfig> dict = new Dictionary<int, SkillActionConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<SkillActionConfig> list = new List<SkillActionConfig>();
		
        public void Merge(object o)
        {
            SkillActionConfigCategory s = o as SkillActionConfigCategory;
            this.list.AddRange(s.list);
        }
		
		[ProtoAfterDeserialization]        
        public void ProtoEndInit()
        {
            foreach (SkillActionConfig config in list)
            {
                config.AfterEndInit();
                this.dict.Add(config.Id, config);
            }
            this.list.Clear();
            
            this.AfterEndInit();
        }
		
        public SkillActionConfig Get(int id)
        {
            this.dict.TryGetValue(id, out SkillActionConfig item);

            if (item == null)
            {
                Log.Error($"配置找不到，配置表名: {nameof (SkillActionConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, SkillActionConfig> GetAll()
        {
            return this.dict;
        }

        public SkillActionConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class SkillActionConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>行为类型</summary>
		[ProtoMember(2)]
		public int Type { get; set; }
		/// <summary>附加分类</summary>
		[ProtoMember(3)]
		public int SubType { get; set; }
		/// <summary>Param1</summary>
		[ProtoMember(4)]
		public int Param1 { get; set; }
		/// <summary>Param2</summary>
		[ProtoMember(5)]
		public int Param2 { get; set; }
		/// <summary>Param3</summary>
		[ProtoMember(6)]
		public int Param3 { get; set; }
		/// <summary>Param4</summary>
		[ProtoMember(7)]
		public int Param4 { get; set; }

	}
}
