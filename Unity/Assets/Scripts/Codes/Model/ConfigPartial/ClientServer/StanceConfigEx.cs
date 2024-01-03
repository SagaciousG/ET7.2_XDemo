namespace ET
{
    public partial class StanceConfigCategory
    {
        public StanceConfig Get(int map, int pos)
        {
            var id = map * 10000 + pos;
            return Get(id);
        }
    }
}