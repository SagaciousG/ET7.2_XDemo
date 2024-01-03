using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace ET.Server
{
    [ComponentOf(typeof(Unit))]
    public class LotteryComponent : Entity, IAwake, IDeserialize, ISerializeToEntity
    {
        [BsonIgnore] public Dictionary<LotteryType, Random> Randoms { get; set; } = new();

        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<LotteryType, int> RandomSeeds { get; set; } = new();

        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<LotteryType, long> LotteryCounts { get; set; } = new();
        
        
    }
}