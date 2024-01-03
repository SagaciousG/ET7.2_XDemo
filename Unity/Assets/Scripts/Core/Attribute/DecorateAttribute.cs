using System;

namespace ET
{
    [AttributeUsage(AttributeTargets.All)]
    public class DecorateAttribute : Attribute
    {
        public string Image;
        public string Color;
    }
}