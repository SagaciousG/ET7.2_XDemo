using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class GMConfigCategory : ConfigSingleton<GMConfigCategory>, IMerge
    {
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, GMConfig> dict = new Dictionary<int, GMConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<GMConfig> list = new List<GMConfig>();
		
        public void Merge(object o)
        {
            GMConfigCategory s = o as GMConfigCategory;
            this.list.AddRange(s.list);
        }
		
		[ProtoAfterDeserialization]        
        public void ProtoEndInit()
        {
            foreach (GMConfig config in list)
            {
                config.AfterEndInit();
                this.dict.Add(config.Id, config);
            }
            this.list.Clear();
            
            this.AfterEndInit();
        }
		
        public GMConfig Get(int id)
        {
            this.dict.TryGetValue(id, out GMConfig item);

            if (item == null)
            {
                Log.Error($"配置找不到，配置表名: {nameof (GMConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, GMConfig> GetAll()
        {
            return this.dict;
        }

        public GMConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class GMConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Code</summary>
		[ProtoMember(2)]
		public string Code { get; set; }
		/// <summary>title</summary>
		[ProtoMember(3)]
		public string title { get; set; }
		/// <summary>描述</summary>
		[ProtoMember(4)]
		public string Desc { get; set; }
		/// <summary>参数类型</summary>
		[ProtoMember(5)]
		public int ParamType1 { get; set; }
		/// <summary>参数1</summary>
		[ProtoMember(6)]
		public string ParamTitle1 { get; set; }
		/// <summary>参数2</summary>
		[ProtoMember(7)]
		public string ParamTitle2 { get; set; }
		/// <summary>参数3</summary>
		[ProtoMember(8)]
		public string ParamTitle3 { get; set; }

	}
}
