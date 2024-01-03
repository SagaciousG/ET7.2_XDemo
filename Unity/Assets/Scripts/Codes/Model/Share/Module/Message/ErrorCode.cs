namespace ET
{
	public static class ErrorCode
	{
		/// <summary> 账号已在另一设备登录 /// </summary>
		public const int ERR_OtherUserLogin = 200001;
		/// <summary> 服务器关闭 /// </summary>
		public const int ERR_ServerClose = 200002;
		/// <summary> 账号或密码错误 /// </summary>
		public const int ERR_AccountOrPwNotExist = 210001;
		/// <summary> 账号不存在 /// </summary>
		public const int ERR_AccountNotExist = 210002;
		/// <summary> 账号已注册 /// </summary>
		public const int ERR_AccountIsExist = 210003;
		public const int ERR_UnitIDNotExist = 210004;
		/// <summary> 名称已被使用 /// </summary>
		public const int ERR_UnitNameUsed = 210005;
		/// <summary> 物品不存在 /// </summary>
		public const int ERR_ItemNotExist = 210006;
		/// <summary> 物品不足 /// </summary>
		public const int ERR_ItemNotEnough = 210007;
		/// <summary> 技能已达到最大等级 /// </summary>
		public const int ERR_SkillLevelMax = 210008;
		/// <summary> 魔法值不足 /// </summary>
		public const int ERR_MPNotEnough = 211001;
		/// <summary> 血量不足 /// </summary>
		public const int ERR_HPNotEnough = 211002;
		/// <summary> 技能被中断 /// </summary>
		public const int ERR_SkillBeKilled = 211003;
		public const int ERR_TestUserLogined = 221001;
		public const int ERR_TestGetKeyNoDetail = 221002;
		public const int ERR_UserCommandHasDone = 221003;
		/// <summary> 选择的目标不合理 /// </summary>
		public const int ERR_TargetInvalid = 221004;
	}
}
