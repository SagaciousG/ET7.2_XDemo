using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class GateWayConfigCategory : ConfigSingleton<GateWayConfigCategory>, IMerge
    {
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, GateWayConfig> dict = new Dictionary<int, GateWayConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<GateWayConfig> list = new List<GateWayConfig>();
		
        public void Merge(object o)
        {
            GateWayConfigCategory s = o as GateWayConfigCategory;
            this.list.AddRange(s.list);
        }
		
		[ProtoAfterDeserialization]        
        public void ProtoEndInit()
        {
            foreach (GateWayConfig config in list)
            {
                config.AfterEndInit();
                this.dict.Add(config.Id, config);
            }
            this.list.Clear();
            
            this.AfterEndInit();
        }
		
        public GateWayConfig Get(int id)
        {
            this.dict.TryGetValue(id, out GateWayConfig item);

            if (item == null)
            {
                Log.Error($"配置找不到，配置表名: {nameof (GateWayConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, GateWayConfig> GetAll()
        {
            return this.dict;
        }

        public GateWayConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class GateWayConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>名称</summary>
		[ProtoMember(2)]
		public string Name { get; set; }
		/// <summary>坐标</summary>
		[ProtoMember(3)]
		public string Position { get; set; }
		/// <summary>旋转</summary>
		[ProtoMember(4)]
		public string Rotation { get; set; }
		/// <summary>所属地图Id</summary>
		[ProtoMember(5)]
		public int Map { get; set; }
		/// <summary>外观</summary>
		[ProtoMember(6)]
		public string Show { get; set; }
		/// <summary>传送至</summary>
		[ProtoMember(7)]
		public int ToGateWay { get; set; }

	}
}
