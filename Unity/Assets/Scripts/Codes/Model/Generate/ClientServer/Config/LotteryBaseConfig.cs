using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class LotteryBaseConfigCategory : ConfigSingleton<LotteryBaseConfigCategory>, IMerge
    {
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, LotteryBaseConfig> dict = new Dictionary<int, LotteryBaseConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<LotteryBaseConfig> list = new List<LotteryBaseConfig>();
		
        public void Merge(object o)
        {
            LotteryBaseConfigCategory s = o as LotteryBaseConfigCategory;
            this.list.AddRange(s.list);
        }
		
		[ProtoAfterDeserialization]        
        public void ProtoEndInit()
        {
            foreach (LotteryBaseConfig config in list)
            {
                config.AfterEndInit();
                this.dict.Add(config.Id, config);
            }
            
            this.AfterEndInit();
            this.list.Clear();
        }
		
        public LotteryBaseConfig Get(int id)
        {
            this.dict.TryGetValue(id, out LotteryBaseConfig item);

            if (item == null)
            {
                Log.Error($"配置找不到，配置表名: {nameof (LotteryBaseConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, LotteryBaseConfig> GetAll()
        {
            return this.dict;
        }

        public LotteryBaseConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class LotteryBaseConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>类型</summary>
		[ProtoMember(2)]
		public int Type { get; set; }
		/// <summary>单抽价格</summary>
		[ProtoMember(3)]
		public int Price { get; set; }
		/// <summary>10抽价格</summary>
		[ProtoMember(4)]
		public int Price10 { get; set; }
		/// <summary>奖池</summary>
		[ProtoMember(5)]
		public int DropPool { get; set; }

	}
}
