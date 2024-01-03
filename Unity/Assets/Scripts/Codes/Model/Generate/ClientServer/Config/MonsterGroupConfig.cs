using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class MonsterGroupConfigCategory : ConfigSingleton<MonsterGroupConfigCategory>, IMerge
    {
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, MonsterGroupConfig> dict = new Dictionary<int, MonsterGroupConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<MonsterGroupConfig> list = new List<MonsterGroupConfig>();
		
        public void Merge(object o)
        {
            MonsterGroupConfigCategory s = o as MonsterGroupConfigCategory;
            this.list.AddRange(s.list);
        }
		
		[ProtoAfterDeserialization]        
        public void ProtoEndInit()
        {
            foreach (MonsterGroupConfig config in list)
            {
                config.AfterEndInit();
                this.dict.Add(config.Id, config);
            }
            
            this.AfterEndInit();
            this.list.Clear();
        }
		
        public MonsterGroupConfig Get(int id)
        {
            this.dict.TryGetValue(id, out MonsterGroupConfig item);

            if (item == null)
            {
                Log.Error($"配置找不到，配置表名: {nameof (MonsterGroupConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, MonsterGroupConfig> GetAll()
        {
            return this.dict;
        }

        public MonsterGroupConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class MonsterGroupConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>分组</summary>
		[ProtoMember(2)]
		public int Group { get; set; }
		/// <summary>权重</summary>
		[ProtoMember(3)]
		public int Weight { get; set; }
		/// <summary>站位</summary>
		[ProtoMember(4)]
		public string Pos { get; set; }
		/// <summary>怪物Id（和站位一对一）</summary>
		[ProtoMember(5)]
		public string MonsterId { get; set; }

	}
}
