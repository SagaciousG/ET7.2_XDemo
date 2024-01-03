using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class StringMapConfigCategory : ConfigSingleton<StringMapConfigCategory>, IMerge
    {
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, StringMapConfig> dict = new Dictionary<int, StringMapConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<StringMapConfig> list = new List<StringMapConfig>();
		
        public void Merge(object o)
        {
            StringMapConfigCategory s = o as StringMapConfigCategory;
            this.list.AddRange(s.list);
        }
		
		[ProtoAfterDeserialization]        
        public void ProtoEndInit()
        {
            foreach (StringMapConfig config in list)
            {
                config.AfterEndInit();
                this.dict.Add(config.Id, config);
            }
            
            this.AfterEndInit();
            this.list.Clear();
        }
		
        public StringMapConfig Get(int id)
        {
            this.dict.TryGetValue(id, out StringMapConfig item);

            if (item == null)
            {
                Log.Error($"配置找不到，配置表名: {nameof (StringMapConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, StringMapConfig> GetAll()
        {
            return this.dict;
        }

        public StringMapConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class StringMapConfig: ProtoObject, IConfig
	{
		/// <summary>id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>分组</summary>
		[ProtoMember(2)]
		public int Group { get; set; }
		/// <summary>key</summary>
		[ProtoMember(3)]
		public int Key { get; set; }
		/// <summary>val</summary>
		[ProtoMember(4)]
		public string Val { get; set; }

	}
}
