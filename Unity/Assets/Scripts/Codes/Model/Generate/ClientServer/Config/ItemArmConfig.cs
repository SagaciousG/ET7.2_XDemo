using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class ItemArmConfigCategory : ConfigSingleton<ItemArmConfigCategory>, IMerge
    {
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, ItemArmConfig> dict = new Dictionary<int, ItemArmConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<ItemArmConfig> list = new List<ItemArmConfig>();
		
        public void Merge(object o)
        {
            ItemArmConfigCategory s = o as ItemArmConfigCategory;
            this.list.AddRange(s.list);
        }
		
		[ProtoAfterDeserialization]        
        public void ProtoEndInit()
        {
            foreach (ItemArmConfig config in list)
            {
                config.AfterEndInit();
                this.dict.Add(config.Id, config);
            }
            
            this.AfterEndInit();
            this.list.Clear();
        }
		
        public ItemArmConfig Get(int id)
        {
            this.dict.TryGetValue(id, out ItemArmConfig item);

            if (item == null)
            {
                Log.Error($"配置找不到，配置表名: {nameof (ItemArmConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, ItemArmConfig> GetAll()
        {
            return this.dict;
        }

        public ItemArmConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class ItemArmConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>部位</summary>
		[ProtoMember(2)]
		public int Part { get; set; }
		/// <summary>基础词条</summary>
		[ProtoMember(3)]
		public string BaseWord { get; set; }
		/// <summary>基础词条参数</summary>
		[ProtoMember(4)]
		public string BaseWordVal { get; set; }
		/// <summary>特殊词条</summary>
		[ProtoMember(5)]
		public string SpecialWord { get; set; }
		/// <summary>词条参数</summary>
		[ProtoMember(6)]
		public string SpecialWordArgs { get; set; }
		/// <summary>套装</summary>
		[ProtoMember(7)]
		public int Outfit { get; set; }

	}
}
