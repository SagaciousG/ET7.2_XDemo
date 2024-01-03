using System;
using System.Data;

namespace ET
{
    public static class MathHelper
    {
        private static DataTable dt => _dt ??= new DataTable();
        [StaticField]
        private static DataTable _dt;
        public static float RadToDeg(float radians)
        {
            return (float)(radians * 180 / System.Math.PI);
        }
        
        public static float DegToRad(float degrees)
        {
            return (float)(degrees * System.Math.PI / 180);
        }

        public static string ByteFormat(long value)
        {
            if (value > 1024 * 1024 * 1024)
            {
                return (value / (1024f * 1024 * 1024)).ToString("0.00") + " GB";
            }else if (value > 1024 * 1024)
            {
                return (value / (1024f * 1024)).ToString("0.00") + " MB";
            }
            return (value / 1024f).ToString("0.00") + " KB";
        }

        public static string ComputeStr(string expression)
        {
            return dt.Compute(expression, "").ToString();
        }
        public static T ComputeStr<T>(string expression)
        {
            return (T)dt.Compute(expression, "");
        }
        
        
    }
}