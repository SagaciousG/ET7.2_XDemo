using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class DropPoolConfigCategory : ConfigSingleton<DropPoolConfigCategory>, IMerge
    {
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, DropPoolConfig> dict = new Dictionary<int, DropPoolConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<DropPoolConfig> list = new List<DropPoolConfig>();
		
        public void Merge(object o)
        {
            DropPoolConfigCategory s = o as DropPoolConfigCategory;
            this.list.AddRange(s.list);
        }
		
		[ProtoAfterDeserialization]        
        public void ProtoEndInit()
        {
            foreach (DropPoolConfig config in list)
            {
                config.AfterEndInit();
                this.dict.Add(config.Id, config);
            }
            
            this.AfterEndInit();
            this.list.Clear();
        }
		
        public DropPoolConfig Get(int id)
        {
            this.dict.TryGetValue(id, out DropPoolConfig item);

            if (item == null)
            {
                Log.Error($"配置找不到，配置表名: {nameof (DropPoolConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, DropPoolConfig> GetAll()
        {
            return this.dict;
        }

        public DropPoolConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class DropPoolConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>组</summary>
		[ProtoMember(2)]
		public int Group { get; set; }
		/// <summary>物品ID</summary>
		[ProtoMember(3)]
		public int Item { get; set; }
		/// <summary>数量</summary>
		[ProtoMember(4)]
		public int Num { get; set; }
		/// <summary>权重</summary>
		[ProtoMember(5)]
		public int Weight { get; set; }

	}
}
