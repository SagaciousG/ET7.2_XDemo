using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class SkillTestModelCategory : ConfigSingleton<SkillTestModelCategory>, IMerge
    {
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, SkillTestModel> dict = new Dictionary<int, SkillTestModel>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<SkillTestModel> list = new List<SkillTestModel>();
		
        public void Merge(object o)
        {
            SkillTestModelCategory s = o as SkillTestModelCategory;
            this.list.AddRange(s.list);
        }
		
		[ProtoAfterDeserialization]        
        public void ProtoEndInit()
        {
            foreach (SkillTestModel config in list)
            {
                config.AfterEndInit();
                this.dict.Add(config.Id, config);
            }
            this.list.Clear();
            
            this.AfterEndInit();
        }
		
        public SkillTestModel Get(int id)
        {
            this.dict.TryGetValue(id, out SkillTestModel item);

            if (item == null)
            {
                Log.Error($"配置找不到，配置表名: {nameof (SkillTestModel)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, SkillTestModel> GetAll()
        {
            return this.dict;
        }

        public SkillTestModel GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class SkillTestModel: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>分类</summary>
		[ProtoMember(2)]
		public int Group { get; set; }
		/// <summary>模型</summary>
		[ProtoMember(3)]
		public string Model { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(4)]
		public string Name { get; set; }

	}
}
