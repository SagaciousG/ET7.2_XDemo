using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class PanelConfigCategory : ConfigSingleton<PanelConfigCategory>, IMerge
    {
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, PanelConfig> dict = new Dictionary<int, PanelConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<PanelConfig> list = new List<PanelConfig>();
		
        public void Merge(object o)
        {
            PanelConfigCategory s = o as PanelConfigCategory;
            this.list.AddRange(s.list);
        }
		
		[ProtoAfterDeserialization]        
        public void ProtoEndInit()
        {
            foreach (PanelConfig config in list)
            {
                config.AfterEndInit();
                this.dict.Add(config.Id, config);
            }
            
            this.AfterEndInit();
            this.list.Clear();
        }
		
        public PanelConfig Get(int id)
        {
            this.dict.TryGetValue(id, out PanelConfig item);

            if (item == null)
            {
                Log.Error($"配置找不到，配置表名: {nameof (PanelConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, PanelConfig> GetAll()
        {
            return this.dict;
        }

        public PanelConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class PanelConfig: ProtoObject, IConfig
	{
		/// <summary>id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }

	}
}
