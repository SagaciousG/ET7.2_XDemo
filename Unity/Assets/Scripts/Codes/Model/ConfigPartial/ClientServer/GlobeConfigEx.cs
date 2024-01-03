namespace ET
{
    public partial class GlobeConfigCategory
    {
        public int GetInt(int id)
        {
            return this.Get(id)?.IntValue ?? 0;
        }
        
        public float GetFloat(int id)
        {
            return this.Get(id)?.FloatValue ?? 0;
        }
        
        public string GetString(int id)
        {
            return this.Get(id)?.StringValue ?? "";
        }
    }
    
}