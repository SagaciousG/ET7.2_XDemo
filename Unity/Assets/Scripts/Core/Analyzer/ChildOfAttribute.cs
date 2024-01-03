using System;

namespace ET
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ChildOfAttribute : Attribute
    {
        public Type type;

        public ChildOfAttribute(Type type = null)
        {
            this.type = type;
        }
    }
}