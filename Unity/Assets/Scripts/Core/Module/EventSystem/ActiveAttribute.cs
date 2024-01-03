using System;

namespace ET
{
    public class ActiveAttribute : BaseAttribute
    {
        public int Key;

        public ActiveAttribute(int key)
        {
            this.Key = key;
        }
    }

    public class ActiveEventAttribute: BaseAttribute
    {
        public Type EntityType;
        public int Key;

        public ActiveEventAttribute(Type entityType, int key)
        {
            this.EntityType = entityType;
            this.Key = key;
        }
    }
}