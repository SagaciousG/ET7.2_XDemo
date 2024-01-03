namespace ET
{
    public static class NumericHelper
    {
        public static long PrcessMDef(long damage, long mDef)
        {
            return damage * (1 - mDef / (100 + mDef));
        }
    
        public static long PrcessDef(long damage, long def)
        {
            return damage * (1 - def / (100 + def));
        }
    }
}