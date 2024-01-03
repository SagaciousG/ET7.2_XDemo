using System.Linq;

namespace ET
{
    
    public static class EntityWaiterSystem
    {
        public class AwakeSystem : AwakeSystem<EntityWaiter, string>
        {
            protected override void Awake(EntityWaiter self, string a)
            {
                self.Key = a;
            }
        }

        public static EntityWaiter AddWaiter(this Entity self, string waiterKey)
        {
            foreach (var waiter in self.GetChildren<EntityWaiter>())
            {
                if (waiter.Key == waiterKey)
                {
                    Log.Error($"已存在Waiter, Key={waiterKey}, 不允许多次添加");
                    return waiter;
                }
            }

            return self.AddChild<EntityWaiter, string>(waiterKey);
        }

        public static async ETTask<int> Wait(this Entity self, string waiterKey)
        {
            foreach (var waiter in self.GetChildren<EntityWaiter>())
            {
                if (waiter.Key == waiterKey)
                    return await waiter.Wait();
            }

            return 0;
        }
        
        public static void Dispatch(this EntityWaiter waiter)
        {
            if (waiter == null)
                return;
            var index = 0;
            while (index < waiter.Waittings.Count)
            {
                var token = waiter.Waittings[index];
                token.Cancel();
                index++;
            }
            waiter.Dispose();
        }
        
        public static async ETTask<int> Wait(this EntityWaiter waiter)
        {
            if (waiter == null)
            {
                return 0;
            }
            var token = new ETCancellationToken();
            waiter.Waittings.Add(token);
            await TimerComponent.Instance.WaitAsync(20 * 1000, token);
            if (token.IsCancel())
                return 0;
            return 1; //超时错误
        }
    }
}