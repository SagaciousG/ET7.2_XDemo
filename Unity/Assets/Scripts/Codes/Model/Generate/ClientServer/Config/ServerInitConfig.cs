using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class ServerInitConfigCategory : ConfigSingleton<ServerInitConfigCategory>, IMerge
    {
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, ServerInitConfig> dict = new Dictionary<int, ServerInitConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<ServerInitConfig> list = new List<ServerInitConfig>();
		
        public void Merge(object o)
        {
            ServerInitConfigCategory s = o as ServerInitConfigCategory;
            this.list.AddRange(s.list);
        }
		
		[ProtoAfterDeserialization]        
        public void ProtoEndInit()
        {
            foreach (ServerInitConfig config in list)
            {
                config.AfterEndInit();
                this.dict.Add(config.Id, config);
            }
            
            this.AfterEndInit();
            this.list.Clear();
        }
		
        public ServerInitConfig Get(int id)
        {
            this.dict.TryGetValue(id, out ServerInitConfig item);

            if (item == null)
            {
                Log.Error($"配置找不到，配置表名: {nameof (ServerInitConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, ServerInitConfig> GetAll()
        {
            return this.dict;
        }

        public ServerInitConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class ServerInitConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>stringVal</summary>
		[ProtoMember(2)]
		public string StringVal { get; set; }
		/// <summary>IntVal</summary>
		[ProtoMember(3)]
		public int IntVal { get; set; }

	}
}
