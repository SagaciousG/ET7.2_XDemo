namespace ET.Client
{
    public static class TestComponentSystem
    {
        public class TestComponentAwakeSystem : AwakeSystem<TestComponent>
        {
            protected override void Awake(TestComponent self)
            {
            }
        }
        
    
    }
}