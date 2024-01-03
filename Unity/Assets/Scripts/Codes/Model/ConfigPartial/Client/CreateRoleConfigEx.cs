using System.Collections.Generic;

namespace ET
{
    public partial class CreateRoleConfigCategory
    {
        private MultiMap<int, CreateRoleConfig> _groupRoles = new();
        public override void AfterEndInit()
        {
            foreach (var config in list)
            {
                _groupRoles.Add(config.Group, config);
            }
        }

        public bool TryGetByGroup(int group, out List<CreateRoleConfig> arr)
        {
            return _groupRoles.TryGetValue(group, out arr);
        }
    }
}