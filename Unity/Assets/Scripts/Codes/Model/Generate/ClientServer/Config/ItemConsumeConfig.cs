using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class ItemConsumeConfigCategory : ConfigSingleton<ItemConsumeConfigCategory>, IMerge
    {
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, ItemConsumeConfig> dict = new Dictionary<int, ItemConsumeConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<ItemConsumeConfig> list = new List<ItemConsumeConfig>();
		
        public void Merge(object o)
        {
            ItemConsumeConfigCategory s = o as ItemConsumeConfigCategory;
            this.list.AddRange(s.list);
        }
		
		[ProtoAfterDeserialization]        
        public void ProtoEndInit()
        {
            foreach (ItemConsumeConfig config in list)
            {
                config.AfterEndInit();
                this.dict.Add(config.Id, config);
            }
            
            this.AfterEndInit();
            this.list.Clear();
        }
		
        public ItemConsumeConfig Get(int id)
        {
            this.dict.TryGetValue(id, out ItemConsumeConfig item);

            if (item == null)
            {
                Log.Error($"配置找不到，配置表名: {nameof (ItemConsumeConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, ItemConsumeConfig> GetAll()
        {
            return this.dict;
        }

        public ItemConsumeConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class ItemConsumeConfig: ProtoObject, IConfig
	{
		/// <summary>id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>类别</summary>
		[ProtoMember(2)]
		public int Type { get; set; }
		/// <summary>参数1</summary>
		[ProtoMember(3)]
		public int Param1 { get; set; }
		/// <summary>参数2</summary>
		[ProtoMember(4)]
		public int Param2 { get; set; }

	}
}
