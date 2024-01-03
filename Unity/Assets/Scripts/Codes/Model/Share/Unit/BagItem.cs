using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    [ChildOf(typeof(BagComponent))]
    public class BagItem : Entity, IAwake<int>, ISerializeToEntity
    {
        public ItemConfig Config
        {
            get
            {
                _config ??= ItemConfigCategory.Instance.Get(ItemID);
                return _config;
            }
        }

        [BsonIgnore]
        private ItemConfig _config;
        public long Num { get; set; }
        public int ItemID { get; set; }
    }
}