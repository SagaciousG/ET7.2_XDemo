using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class MapBuildConfigCategory : ConfigSingleton<MapBuildConfigCategory>, IMerge
    {
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, MapBuildConfig> dict = new Dictionary<int, MapBuildConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<MapBuildConfig> list = new List<MapBuildConfig>();
		
        public void Merge(object o)
        {
            MapBuildConfigCategory s = o as MapBuildConfigCategory;
            this.list.AddRange(s.list);
        }
		
		[ProtoAfterDeserialization]        
        public void ProtoEndInit()
        {
            foreach (MapBuildConfig config in list)
            {
                config.AfterEndInit();
                this.dict.Add(config.Id, config);
            }
            
            this.AfterEndInit();
            this.list.Clear();
        }
		
        public MapBuildConfig Get(int id)
        {
            this.dict.TryGetValue(id, out MapBuildConfig item);

            if (item == null)
            {
                Log.Error($"配置找不到，配置表名: {nameof (MapBuildConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, MapBuildConfig> GetAll()
        {
            return this.dict;
        }

        public MapBuildConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class MapBuildConfig: ProtoObject, IConfig
	{
		/// <summary>id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>分类</summary>
		[ProtoMember(2)]
		public int Type { get; set; }
		/// <summary>资源</summary>
		[ProtoMember(3)]
		public string Prefab { get; set; }
		/// <summary>图标</summary>
		[ProtoMember(4)]
		public string Icon { get; set; }

	}
}
