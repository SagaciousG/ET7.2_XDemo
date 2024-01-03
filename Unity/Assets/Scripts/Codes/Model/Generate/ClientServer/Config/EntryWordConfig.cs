using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class EntryWordConfigCategory : ConfigSingleton<EntryWordConfigCategory>, IMerge
    {
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, EntryWordConfig> dict = new Dictionary<int, EntryWordConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<EntryWordConfig> list = new List<EntryWordConfig>();
		
        public void Merge(object o)
        {
            EntryWordConfigCategory s = o as EntryWordConfigCategory;
            this.list.AddRange(s.list);
        }
		
		[ProtoAfterDeserialization]        
        public void ProtoEndInit()
        {
            foreach (EntryWordConfig config in list)
            {
                config.AfterEndInit();
                this.dict.Add(config.Id, config);
            }
            
            this.AfterEndInit();
            this.list.Clear();
        }
		
        public EntryWordConfig Get(int id)
        {
            this.dict.TryGetValue(id, out EntryWordConfig item);

            if (item == null)
            {
                Log.Error($"配置找不到，配置表名: {nameof (EntryWordConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, EntryWordConfig> GetAll()
        {
            return this.dict;
        }

        public EntryWordConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class EntryWordConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>类型</summary>
		[ProtoMember(2)]
		public int Type { get; set; }
		/// <summary>描述</summary>
		[ProtoMember(3)]
		public string Desc { get; set; }
		/// <summary>参数1</summary>
		[ProtoMember(4)]
		public string Param1 { get; set; }
		/// <summary>参数2</summary>
		[ProtoMember(5)]
		public string Param2 { get; set; }

	}
}
