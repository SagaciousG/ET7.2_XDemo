using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class SkillTargetFilterConfigCategory : ConfigSingleton<SkillTargetFilterConfigCategory>, IMerge
    {
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, SkillTargetFilterConfig> dict = new Dictionary<int, SkillTargetFilterConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<SkillTargetFilterConfig> list = new List<SkillTargetFilterConfig>();
		
        public void Merge(object o)
        {
            SkillTargetFilterConfigCategory s = o as SkillTargetFilterConfigCategory;
            this.list.AddRange(s.list);
        }
		
		[ProtoAfterDeserialization]        
        public void ProtoEndInit()
        {
            foreach (SkillTargetFilterConfig config in list)
            {
                config.AfterEndInit();
                this.dict.Add(config.Id, config);
            }
            
            this.AfterEndInit();
            this.list.Clear();
        }
		
        public SkillTargetFilterConfig Get(int id)
        {
            this.dict.TryGetValue(id, out SkillTargetFilterConfig item);

            if (item == null)
            {
                Log.Error($"配置找不到，配置表名: {nameof (SkillTargetFilterConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, SkillTargetFilterConfig> GetAll()
        {
            return this.dict;
        }

        public SkillTargetFilterConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class SkillTargetFilterConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>描述</summary>
		[ProtoMember(2)]
		public string Desc { get; set; }
		/// <summary>目标阵营</summary>
		[ProtoMember(3)]
		public int Faction { get; set; }
		/// <summary>目标角色类型</summary>
		[ProtoMember(4)]
		public int TargetRole { get; set; }
		/// <summary>目标状态</summary>
		[ProtoMember(5)]
		public int TargetStatue { get; set; }
		/// <summary>选择目标</summary>
		[ProtoMember(6)]
		public int SelectType { get; set; }
		/// <summary>目标数量</summary>
		[ProtoMember(7)]
		public int TargetCount { get; set; }

	}
}
