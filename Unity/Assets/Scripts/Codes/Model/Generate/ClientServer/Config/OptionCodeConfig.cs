using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class OptionCodeConfigCategory : ConfigSingleton<OptionCodeConfigCategory>, IMerge
    {
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, OptionCodeConfig> dict = new Dictionary<int, OptionCodeConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<OptionCodeConfig> list = new List<OptionCodeConfig>();
		
        public void Merge(object o)
        {
            OptionCodeConfigCategory s = o as OptionCodeConfigCategory;
            this.list.AddRange(s.list);
        }
		
		[ProtoAfterDeserialization]        
        public void ProtoEndInit()
        {
            foreach (OptionCodeConfig config in list)
            {
                config.AfterEndInit();
                this.dict.Add(config.Id, config);
            }
            
            this.AfterEndInit();
            this.list.Clear();
        }
		
        public OptionCodeConfig Get(int id)
        {
            this.dict.TryGetValue(id, out OptionCodeConfig item);

            if (item == null)
            {
                Log.Error($"配置找不到，配置表名: {nameof (OptionCodeConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, OptionCodeConfig> GetAll()
        {
            return this.dict;
        }

        public OptionCodeConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class OptionCodeConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(2)]
		public string Name { get; set; }
		/// <summary>字段名</summary>
		[ProtoMember(3)]
		public string FieldName { get; set; }
		/// <summary>说明</summary>
		[ProtoMember(4)]
		public string Tips { get; set; }
		/// <summary>参数</summary>
		[ProtoMember(5)]
		public string Param { get; set; }
		/// <summary>显示条件</summary>
		[ProtoMember(6)]
		public int ShowCondition { get; set; }

	}
}
