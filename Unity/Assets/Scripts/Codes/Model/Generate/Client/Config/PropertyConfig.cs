using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class PropertyConfigCategory : ConfigSingleton<PropertyConfigCategory>, IMerge
    {
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, PropertyConfig> dict = new Dictionary<int, PropertyConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<PropertyConfig> list = new List<PropertyConfig>();
		
        public void Merge(object o)
        {
            PropertyConfigCategory s = o as PropertyConfigCategory;
            this.list.AddRange(s.list);
        }
		
		[ProtoAfterDeserialization]        
        public void ProtoEndInit()
        {
            foreach (PropertyConfig config in list)
            {
                config.AfterEndInit();
                this.dict.Add(config.Id, config);
            }
            this.list.Clear();
            
            this.AfterEndInit();
        }
		
        public PropertyConfig Get(int id)
        {
            this.dict.TryGetValue(id, out PropertyConfig item);

            if (item == null)
            {
                Log.Error($"配置找不到，配置表名: {nameof (PropertyConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, PropertyConfig> GetAll()
        {
            return this.dict;
        }

        public PropertyConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class PropertyConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>字段名</summary>
		[ProtoMember(2)]
		public string FieldName { get; set; }
		/// <summary>分类</summary>
		[ProtoMember(3)]
		public int Sort { get; set; }
		/// <summary>中文名</summary>
		[ProtoMember(4)]
		public string Name { get; set; }
		/// <summary>默认值</summary>
		[ProtoMember(5)]
		public int DefaultVal { get; set; }
		/// <summary>Key</summary>
		[ProtoMember(6)]
		public int Key { get; set; }
		/// <summary>固定增加值</summary>
		[ProtoMember(7)]
		public int Add { get; set; }
		/// <summary>百分比增加</summary>
		[ProtoMember(8)]
		public int Pct { get; set; }
		/// <summary>固定再增加</summary>
		[ProtoMember(9)]
		public int FinalAdd { get; set; }
		/// <summary>百分比再增加</summary>
		[ProtoMember(10)]
		public int FinalPct { get; set; }

	}
}
