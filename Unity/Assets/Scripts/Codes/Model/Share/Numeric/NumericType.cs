namespace ET
{
	public static class NumericType
	{
		public const int Max = 10000;
		
		public const int Power = 1003; //力量
		public const int PowerBase = Power * 10 + 1;
		public const int PowerAdd = Power * 10 + 2;
		public const int PowerPct = Power * 10 + 3;
		public const int PowerFinalAdd = Power * 10 + 4;
		public const int PowerFinalPct = Power * 10 + 5;
		
		public const int Intellect = 1004; //智力
		public const int IntellectBase = Intellect * 10 + 1;
		public const int IntellectAdd = Intellect * 10 + 2;
		public const int IntellectPct = Intellect * 10 + 3;
		public const int IntellectFinalAdd = Intellect * 10 + 4;
		public const int IntellectFinalPct = Intellect * 10 + 5;
		
		public const int Physique = 1005; //体质
		public const int PhysiqueBase = Physique * 10 + 1;
		public const int PhysiqueAdd = Physique * 10 + 2;
		public const int PhysiquePct = Physique * 10 + 3;
		public const int PhysiqueFinalAdd = Physique * 10 + 4;
		public const int PhysiqueFinalPct = Physique * 10 + 5;
		
		public const int Agile = 1006; //敏捷
		public const int AgileBase = Agile * 10 + 1;
		public const int AgileAdd = Agile * 10 + 2;
		public const int AgilePct = Agile * 10 + 3;
		public const int AgileFinalAdd = Agile * 10 + 4;
		public const int AgileFinalPct = Agile * 10 + 5;
		
		public const int Insight = 1007; //洞察
		public const int InsightBase = Insight * 10 + 1;
		public const int InsightAdd = Insight * 10 + 2;
		public const int InsightPct = Insight * 10 + 3;
		public const int InsightFinalAdd = Insight * 10 + 4;
		public const int InsightFinalPct = Insight * 10 + 5;
		
		public const int MagicAttack = 1008; //魔法攻击
		public const int MagicAttackBase = MagicAttack * 10 + 1;
		public const int MagicAttackAdd = MagicAttack * 10 + 2;
		public const int MagicAttackPct = MagicAttack * 10 + 3;
		public const int MagicAttackFinalAdd = MagicAttack * 10 + 4;
		public const int MagicAttackFinalPct = MagicAttack * 10 + 5;
		
		public const int Attack = 1009; //物理攻击
		public const int AttackBase = Attack * 10 + 1;
		public const int AttackAdd = Attack * 10 + 2;
		public const int AttackPct = Attack * 10 + 3;
		public const int AttackFinalAdd = Attack * 10 + 4;
		public const int AttackFinalPct = Attack * 10 + 5;
		
		public const int AttackNum = 1010; //出手次数
		public const int AttackNumBase = AttackNum * 10 + 1;
		public const int AttackNumAdd = AttackNum * 10 + 2;
		
		public const int AttackSpeed = 1011; //出手速度
		public const int AttackSpeedBase = AttackSpeed * 10 + 1;
		public const int AttackSpeedAdd = AttackSpeed * 10 + 2;
		
		public const int MagicDefense = 1012; //魔法防御
		public const int MagicDefenseBase = MagicDefense * 10 + 1;
		public const int MagicDefenseAdd = MagicDefense * 10 + 2;
		public const int MagicDefensePct = MagicDefense * 10 + 3;
		public const int MagicDefenseFinalAdd = MagicDefense * 10 + 4;
		public const int MagicDefenseFinalPct = MagicDefense * 10 + 5;
		
		public const int Defense = 1013; //物理防御
		public const int DefenseBase = Defense * 10 + 1;
		public const int DefenseAdd = Defense * 10 + 2;
		public const int DefensePct = Defense * 10 + 3;
		public const int DefenseFinalAdd = Defense * 10 + 4;
		public const int DefenseFinalPct = Defense * 10 + 5;
		
		public const int HitRate = 1014; //命中值
		public const int HitRateBase = HitRate * 10 + 1;
		public const int HitRateAdd = HitRate * 10 + 2;
		
		public const int Dodge = 1015; //闪避值
		public const int DodgeBase = Dodge * 10 + 1;
		public const int DodgeAdd = Dodge * 10 + 2;
		
		public const int InsightValue = 1016; //洞察力
		public const int InsightValueBase = InsightValue * 10 + 1;
		public const int InsightValueAdd = InsightValue * 10 + 2;
		
		public const int Heath = 1017; //血量
		public const int HeathBase = Heath * 10 + 1;
		
		public const int HeathMax = 1018; //血量上限
		public const int HeathMaxBase = HeathMax * 10 + 1;
		public const int HeathMaxAdd = HeathMax * 10 + 2;
		public const int HeathMaxPct = HeathMax * 10 + 3;
		public const int HeathMaxFinalAdd = HeathMax * 10 + 4;
		public const int HeathMaxFinalPct = HeathMax * 10 + 5;
		
		public const int MagicPower = 1019; //蓝量
		public const int MagicPowerBase = MagicPower * 10 + 1;
		
		public const int MagicPowerMax = 1020; //蓝量上限
		public const int MagicPowerMaxBase = MagicPowerMax * 10 + 1;
		public const int MagicPowerMaxAdd = MagicPowerMax * 10 + 2;
		public const int MagicPowerMaxPct = MagicPowerMax * 10 + 3;
		public const int MagicPowerMaxFinalAdd = MagicPowerMax * 10 + 4;
		public const int MagicPowerMaxFinalPct = MagicPowerMax * 10 + 5;
		
		public const int MagicPenetration = 1021; //魔法穿透
		public const int MagicPenetrationBase = MagicPenetration * 10 + 1;
		public const int MagicPenetrationAdd = MagicPenetration * 10 + 2;
		
		public const int DefensePenetration = 1022; //护甲穿透
		public const int DefensePenetrationBase = DefensePenetration * 10 + 1;
		public const int DefensePenetrationAdd = DefensePenetration * 10 + 2;
		
		public const int Parry = 1023; //格挡
		public const int ParryBase = Parry * 10 + 1;
		public const int ParryAdd = Parry * 10 + 2;
		
		public const int TheShock = 1024; //反震值
		public const int TheShockBase = TheShock * 10 + 1;
		public const int TheShockAdd = TheShock * 10 + 2;
		
		public const int Backfire = 1025; //魔法反噬
		public const int BackfireBase = Backfire * 10 + 1;
		public const int BackfireAdd = Backfire * 10 + 2;
		
		public const int Critical = 1026; //暴击率
		public const int CriticalBase = Critical * 10 + 1;
		public const int CriticalAdd = Critical * 10 + 2;
		
		public const int CritResistance = 1027; //暴击抵抗
		public const int CritResistanceBase = CritResistance * 10 + 1;
		public const int CritResistanceAdd = CritResistance * 10 + 2;
		
		public const int MagicShield = 1028; //魔法护盾
		public const int MagicShieldBase = MagicShield * 10 + 1;
		public const int MagicShieldAdd = MagicShield * 10 + 2;
	}
}
