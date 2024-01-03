using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class UnitShowConfigCategory : ConfigSingleton<UnitShowConfigCategory>, IMerge
    {
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, UnitShowConfig> dict = new Dictionary<int, UnitShowConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<UnitShowConfig> list = new List<UnitShowConfig>();
		
        public void Merge(object o)
        {
            UnitShowConfigCategory s = o as UnitShowConfigCategory;
            this.list.AddRange(s.list);
        }
		
		[ProtoAfterDeserialization]        
        public void ProtoEndInit()
        {
            foreach (UnitShowConfig config in list)
            {
                config.AfterEndInit();
                this.dict.Add(config.Id, config);
            }
            
            this.AfterEndInit();
            this.list.Clear();
        }
		
        public UnitShowConfig Get(int id)
        {
            this.dict.TryGetValue(id, out UnitShowConfig item);

            if (item == null)
            {
                Log.Error($"配置找不到，配置表名: {nameof (UnitShowConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, UnitShowConfig> GetAll()
        {
            return this.dict;
        }

        public UnitShowConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class UnitShowConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>组</summary>
		[ProtoMember(2)]
		public int Group { get; set; }
		/// <summary>模型</summary>
		[ProtoMember(3)]
		public string Model { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(4)]
		public string Name { get; set; }
		/// <summary>Desc</summary>
		[ProtoMember(5)]
		public string Desc { get; set; }
		/// <summary>偏移</summary>
		[ProtoMember(6)]
		public string Offset { get; set; }
		/// <summary>旋转</summary>
		[ProtoMember(7)]
		public string Rotation { get; set; }
		/// <summary>缩放</summary>
		[ProtoMember(8)]
		public float Scale { get; set; }
		/// <summary>外壳</summary>
		[ProtoMember(9)]
		public string Shell { get; set; }

	}
}
