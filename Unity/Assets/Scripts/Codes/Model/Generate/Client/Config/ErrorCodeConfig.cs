using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class ErrorCodeConfigCategory : ConfigSingleton<ErrorCodeConfigCategory>, IMerge
    {
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, ErrorCodeConfig> dict = new Dictionary<int, ErrorCodeConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<ErrorCodeConfig> list = new List<ErrorCodeConfig>();
		
        public void Merge(object o)
        {
            ErrorCodeConfigCategory s = o as ErrorCodeConfigCategory;
            this.list.AddRange(s.list);
        }
		
		[ProtoAfterDeserialization]        
        public void ProtoEndInit()
        {
            foreach (ErrorCodeConfig config in list)
            {
                config.AfterEndInit();
                this.dict.Add(config.Id, config);
            }
            this.list.Clear();
            
            this.AfterEndInit();
        }
		
        public ErrorCodeConfig Get(int id)
        {
            this.dict.TryGetValue(id, out ErrorCodeConfig item);

            if (item == null)
            {
                Log.Error($"配置找不到，配置表名: {nameof (ErrorCodeConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, ErrorCodeConfig> GetAll()
        {
            return this.dict;
        }

        public ErrorCodeConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class ErrorCodeConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>字段名</summary>
		[ProtoMember(2)]
		public string Header { get; set; }
		/// <summary>Log文本</summary>
		[ProtoMember(3)]
		public string Log { get; set; }
		/// <summary>客户端提示语</summary>
		[ProtoMember(4)]
		public string ClientTips { get; set; }

	}
}
