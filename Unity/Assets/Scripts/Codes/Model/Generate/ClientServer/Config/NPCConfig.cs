using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class NPCConfigCategory : ConfigSingleton<NPCConfigCategory>, IMerge
    {
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, NPCConfig> dict = new Dictionary<int, NPCConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<NPCConfig> list = new List<NPCConfig>();
		
        public void Merge(object o)
        {
            NPCConfigCategory s = o as NPCConfigCategory;
            this.list.AddRange(s.list);
        }
		
		[ProtoAfterDeserialization]        
        public void ProtoEndInit()
        {
            foreach (NPCConfig config in list)
            {
                config.AfterEndInit();
                this.dict.Add(config.Id, config);
            }
            
            this.AfterEndInit();
            this.list.Clear();
        }
		
        public NPCConfig Get(int id)
        {
            this.dict.TryGetValue(id, out NPCConfig item);

            if (item == null)
            {
                Log.Error($"配置找不到，配置表名: {nameof (NPCConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, NPCConfig> GetAll()
        {
            return this.dict;
        }

        public NPCConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class NPCConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>所属地图</summary>
		[ProtoMember(2)]
		public int Map { get; set; }
		/// <summary>坐标</summary>
		[ProtoMember(3)]
		public string Pos { get; set; }
		/// <summary>名称</summary>
		[ProtoMember(4)]
		public string Name { get; set; }
		/// <summary>模型</summary>
		[ProtoMember(5)]
		public int ModelShow { get; set; }
		/// <summary>交互操作码</summary>
		[ProtoMember(6)]
		public string OptionCode { get; set; }
		/// <summary>初始属性</summary>
		[ProtoMember(7)]
		public int Attribute { get; set; }

	}
}
