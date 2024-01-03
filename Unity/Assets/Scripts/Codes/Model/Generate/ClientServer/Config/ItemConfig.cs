using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class ItemConfigCategory : ConfigSingleton<ItemConfigCategory>, IMerge
    {
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, ItemConfig> dict = new Dictionary<int, ItemConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<ItemConfig> list = new List<ItemConfig>();
		
        public void Merge(object o)
        {
            ItemConfigCategory s = o as ItemConfigCategory;
            this.list.AddRange(s.list);
        }
		
		[ProtoAfterDeserialization]        
        public void ProtoEndInit()
        {
            foreach (ItemConfig config in list)
            {
                config.AfterEndInit();
                this.dict.Add(config.Id, config);
            }
            
            this.AfterEndInit();
            this.list.Clear();
        }
		
        public ItemConfig Get(int id)
        {
            this.dict.TryGetValue(id, out ItemConfig item);

            if (item == null)
            {
                Log.Error($"配置找不到，配置表名: {nameof (ItemConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, ItemConfig> GetAll()
        {
            return this.dict;
        }

        public ItemConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class ItemConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>类别</summary>
		[ProtoMember(2)]
		public int Type { get; set; }
		/// <summary>名称</summary>
		[ProtoMember(3)]
		public string Name { get; set; }
		/// <summary>描述</summary>
		[ProtoMember(4)]
		public string Desc { get; set; }
		/// <summary>图标</summary>
		[ProtoMember(5)]
		public string Icon { get; set; }
		/// <summary>品质</summary>
		[ProtoMember(6)]
		public int Quality { get; set; }
		/// <summary>叠加</summary>
		[ProtoMember(7)]
		public int Overlay { get; set; }
		/// <summary>出售价值</summary>
		[ProtoMember(8)]
		public int SellPrice { get; set; }
		/// <summary>索引</summary>
		[ProtoMember(9)]
		public int Index { get; set; }
		/// <summary>选项列表</summary>
		[ProtoMember(10)]
		public string OptionBtn { get; set; }
		/// <summary>是否显示在背包</summary>
		[ProtoMember(11)]
		public int HideInBag { get; set; }

	}
}
