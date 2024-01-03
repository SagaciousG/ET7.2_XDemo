using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class GlobeConfigCategory : ConfigSingleton<GlobeConfigCategory>, IMerge
    {
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, GlobeConfig> dict = new Dictionary<int, GlobeConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<GlobeConfig> list = new List<GlobeConfig>();
		
        public void Merge(object o)
        {
            GlobeConfigCategory s = o as GlobeConfigCategory;
            this.list.AddRange(s.list);
        }
		
		[ProtoAfterDeserialization]        
        public void ProtoEndInit()
        {
            foreach (GlobeConfig config in list)
            {
                config.AfterEndInit();
                this.dict.Add(config.Id, config);
            }
            this.list.Clear();
            
            this.AfterEndInit();
        }
		
        public GlobeConfig Get(int id)
        {
            this.dict.TryGetValue(id, out GlobeConfig item);

            if (item == null)
            {
                Log.Error($"配置找不到，配置表名: {nameof (GlobeConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, GlobeConfig> GetAll()
        {
            return this.dict;
        }

        public GlobeConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class GlobeConfig: ProtoObject, IConfig
	{
		/// <summary>id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>FloatValue</summary>
		[ProtoMember(2)]
		public float FloatValue { get; set; }
		/// <summary>IntValue</summary>
		[ProtoMember(3)]
		public int IntValue { get; set; }
		/// <summary>StringValue</summary>
		[ProtoMember(4)]
		public string StringValue { get; set; }
		/// <summary>注释</summary>
		[ProtoMember(5)]
		public string Note { get; set; }

	}
}
