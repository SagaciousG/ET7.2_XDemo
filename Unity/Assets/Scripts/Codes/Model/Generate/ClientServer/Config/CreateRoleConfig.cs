using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class CreateRoleConfigCategory : ConfigSingleton<CreateRoleConfigCategory>, IMerge
    {
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, CreateRoleConfig> dict = new Dictionary<int, CreateRoleConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<CreateRoleConfig> list = new List<CreateRoleConfig>();
		
        public void Merge(object o)
        {
            CreateRoleConfigCategory s = o as CreateRoleConfigCategory;
            this.list.AddRange(s.list);
        }
		
		[ProtoAfterDeserialization]        
        public void ProtoEndInit()
        {
            foreach (CreateRoleConfig config in list)
            {
                config.AfterEndInit();
                this.dict.Add(config.Id, config);
            }
            
            this.AfterEndInit();
            this.list.Clear();
        }
		
        public CreateRoleConfig Get(int id)
        {
            this.dict.TryGetValue(id, out CreateRoleConfig item);

            if (item == null)
            {
                Log.Error($"配置找不到，配置表名: {nameof (CreateRoleConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, CreateRoleConfig> GetAll()
        {
            return this.dict;
        }

        public CreateRoleConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class CreateRoleConfig: ProtoObject, IConfig
	{
		/// <summary>id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>组</summary>
		[ProtoMember(2)]
		public int Group { get; set; }
		/// <summary>部位类型</summary>
		[ProtoMember(3)]
		public int ArmType { get; set; }
		/// <summary>造型</summary>
		[ProtoMember(4)]
		public string OpType { get; set; }
		/// <summary>造型名称</summary>
		[ProtoMember(5)]
		public string OpName { get; set; }
		/// <summary>颜色</summary>
		[ProtoMember(6)]
		public string Color { get; set; }
		/// <summary>颜色名称</summary>
		[ProtoMember(7)]
		public string ColorName { get; set; }

	}
}
