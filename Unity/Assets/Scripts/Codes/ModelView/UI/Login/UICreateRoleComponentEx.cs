using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
	public partial class UICreateRoleComponent
	{
		public MultiMap<int, CreateRoleConfig> ArmsGroup;
		public MultiMap<string, CreateRoleConfig> OpGroup;
	}
}
