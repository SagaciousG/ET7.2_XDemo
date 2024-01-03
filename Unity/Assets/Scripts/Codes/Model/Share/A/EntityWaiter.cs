using System.Collections.Generic;

namespace ET
{
    [ChildOf()]
    public class EntityWaiter : Entity, IAwake<string>
    {
        public string Key { get; set; }
        public List<ETCancellationToken> Waittings { get; set; } = new();
    }

    public static class WaiterKey
    {
        public const string CreateMyUnit = "CreateMyUnit";
        public const string ShellWaiter = "ShellWaiter";
        public const string ViewGOWaiter = "ViewGOWaiter";
    }
}