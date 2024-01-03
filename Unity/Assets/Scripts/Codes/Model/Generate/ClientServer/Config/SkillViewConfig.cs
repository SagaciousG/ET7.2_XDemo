using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class SkillViewConfigCategory : ConfigSingleton<SkillViewConfigCategory>, IMerge
    {
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, SkillViewConfig> dict = new Dictionary<int, SkillViewConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<SkillViewConfig> list = new List<SkillViewConfig>();
		
        public void Merge(object o)
        {
            SkillViewConfigCategory s = o as SkillViewConfigCategory;
            this.list.AddRange(s.list);
        }
		
		[ProtoAfterDeserialization]        
        public void ProtoEndInit()
        {
            foreach (SkillViewConfig config in list)
            {
                config.AfterEndInit();
                this.dict.Add(config.Id, config);
            }
            
            this.AfterEndInit();
            this.list.Clear();
        }
		
        public SkillViewConfig Get(int id)
        {
            this.dict.TryGetValue(id, out SkillViewConfig item);

            if (item == null)
            {
                Log.Error($"配置找不到，配置表名: {nameof (SkillViewConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, SkillViewConfig> GetAll()
        {
            return this.dict;
        }

        public SkillViewConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class SkillViewConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>名称</summary>
		[ProtoMember(2)]
		public string Name { get; set; }
		/// <summary>描述</summary>
		[ProtoMember(3)]
		public string Desc { get; set; }
		/// <summary>图标</summary>
		[ProtoMember(4)]
		public string Icon { get; set; }

	}
}
