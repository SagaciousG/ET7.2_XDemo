using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class MapConfigCategory : ConfigSingleton<MapConfigCategory>, IMerge
    {
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, MapConfig> dict = new Dictionary<int, MapConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<MapConfig> list = new List<MapConfig>();
		
        public void Merge(object o)
        {
            MapConfigCategory s = o as MapConfigCategory;
            this.list.AddRange(s.list);
        }
		
		[ProtoAfterDeserialization]        
        public void ProtoEndInit()
        {
            foreach (MapConfig config in list)
            {
                config.AfterEndInit();
                this.dict.Add(config.Id, config);
            }
            
            this.AfterEndInit();
            this.list.Clear();
        }
		
        public MapConfig Get(int id)
        {
            this.dict.TryGetValue(id, out MapConfig item);

            if (item == null)
            {
                Log.Error($"配置找不到，配置表名: {nameof (MapConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, MapConfig> GetAll()
        {
            return this.dict;
        }

        public MapConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class MapConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>名字</summary>
		[ProtoMember(2)]
		public string Name { get; set; }
		/// <summary>安全区域</summary>
		[ProtoMember(3)]
		public int IsSecure { get; set; }
		/// <summary>场景名</summary>
		[ProtoMember(4)]
		public string SceneName { get; set; }
		/// <summary>刷怪配置</summary>
		[ProtoMember(5)]
		public int MonsterGroup { get; set; }
		/// <summary>出生坐标</summary>
		[ProtoMember(6)]
		public string BornPoint { get; set; }
		/// <summary>镜头初始坐标</summary>
		[ProtoMember(7)]
		public string BattleCameraPos { get; set; }
		/// <summary>镜头初始旋转</summary>
		[ProtoMember(8)]
		public string BattleCameraRot { get; set; }

	}
}
