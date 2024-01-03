using System;

namespace ET
{
    public class PoolObject : IDisposable
    {
        public void Dispose()
        {
            ObjectPool.Instance.Recycle(this);
        }
    }
}