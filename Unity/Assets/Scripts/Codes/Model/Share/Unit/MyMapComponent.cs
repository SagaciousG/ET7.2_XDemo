using System.Collections.Generic;

namespace ET
{
    [ComponentOf(typeof(Unit))]
    public class MyMapComponent : Entity, IAwake, ISerializeToEntity
    {
        public List<long> MyMaps { get; set; } = new();
        public List<long> MyCollections { get; set; } = new();
    }
}