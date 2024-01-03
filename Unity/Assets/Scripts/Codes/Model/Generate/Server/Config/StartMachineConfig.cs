using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class StartMachineConfigCategory : ConfigSingleton<StartMachineConfigCategory>, IMerge
    {
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, StartMachineConfig> dict = new Dictionary<int, StartMachineConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<StartMachineConfig> list = new List<StartMachineConfig>();
		
        public void Merge(object o)
        {
            StartMachineConfigCategory s = o as StartMachineConfigCategory;
            this.list.AddRange(s.list);
        }
		
		[ProtoAfterDeserialization]        
        public void ProtoEndInit()
        {
            foreach (StartMachineConfig config in list)
            {
                config.AfterEndInit();
                this.dict.Add(config.Id, config);
            }
            
            this.AfterEndInit();
            this.list.Clear();
        }
		
        public StartMachineConfig Get(int id)
        {
            this.dict.TryGetValue(id, out StartMachineConfig item);

            if (item == null)
            {
                Log.Error($"配置找不到，配置表名: {nameof (StartMachineConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, StartMachineConfig> GetAll()
        {
            return this.dict;
        }

        public StartMachineConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class StartMachineConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>内网地址</summary>
		[ProtoMember(2)]
		public string InnerIP { get; set; }
		/// <summary>外网地址</summary>
		[ProtoMember(3)]
		public string OuterIP { get; set; }
		/// <summary>守护进程端口</summary>
		[ProtoMember(4)]
		public string WatcherPort { get; set; }

	}
}
