namespace ET
{
    public enum MoneyType
    {
        None = 0,
        [Name("铜币")]
        Copper = 1,
        [Name("技能点")]
        Sp = 2,
        [Name("经验")]
        Exp = 3,
    }
    
    public enum BagItemType
    {
        [Name("全部")]
        All = 999,
        [Name("道具")]
        Consume = 1,
        [Name("装备")]
        Arms = 2,
        [Name("材料")]
        Stuff = 3,
        
        
        //>=100不做背包分页
        [Name("货币")]
        Money = 100,
    }

    public enum EquipmentPart
    {
        [Name("头盔")]
        Head,
        [Name("肩甲")]
        Arm,
        [Name("上衣")]
        Cloth,
        [Name("腰带")]
        Belt,
        [Name("裤子")]
        Pants,
        [Name("鞋子")]
        Shoes,
        [Name("武器")]
        Weapon,
        [Name("武器/副手")]
        WeaponOrShield,
        [Name("副手")]
        Shield,
    }
    //装备槽
    public enum EquipmentHole
    {
        [Name("头盔")]
        Head,
        [Name("肩甲")]
        Arm,
        [Name("上衣")]
        Cloth,
        [Name("腰带")]
        Belt,
        [Name("裤子")]
        Pants,
        [Name("鞋子")]
        Shoes,
        [Name("副手")]
        WeaponL,
        [Name("武器")]
        WeaponR
    }
    
    public enum UnitPartType
    {
        [Name("身体")]
        Skin,
        [Name("头发")]
        Hair,
        [Name("头盔")]
        Head,
        [Name("肩甲")]
        Arm,
        [Name("上衣")]
        Cloth,
        [Name("下身")]
        Pants,
        [Name("武器(左)")]
        Left,
        [Name("武器(右)")]
        Right,
        [Name("眼睛")]
        Eye,
        
    }

    public enum ConsumeType
    {
        Skill = 1,
    }
}