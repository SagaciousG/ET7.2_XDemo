using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class DamageModelConfigCategory : ConfigSingleton<DamageModelConfigCategory>, IMerge
    {
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, DamageModelConfig> dict = new Dictionary<int, DamageModelConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<DamageModelConfig> list = new List<DamageModelConfig>();
		
        public void Merge(object o)
        {
            DamageModelConfigCategory s = o as DamageModelConfigCategory;
            this.list.AddRange(s.list);
        }
		
		[ProtoAfterDeserialization]        
        public void ProtoEndInit()
        {
            foreach (DamageModelConfig config in list)
            {
                config.AfterEndInit();
                this.dict.Add(config.Id, config);
            }
            this.list.Clear();
            
            this.AfterEndInit();
        }
		
        public DamageModelConfig Get(int id)
        {
            this.dict.TryGetValue(id, out DamageModelConfig item);

            if (item == null)
            {
                Log.Error($"配置找不到，配置表名: {nameof (DamageModelConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, DamageModelConfig> GetAll()
        {
            return this.dict;
        }

        public DamageModelConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class DamageModelConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>伤害类型</summary>
		[ProtoMember(2)]
		public int DamageType { get; set; }
		/// <summary>公式</summary>
		[ProtoMember(3)]
		public int Equation { get; set; }

	}
}
