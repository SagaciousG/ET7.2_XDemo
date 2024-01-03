using System;

namespace ET
{
    [Flags]
    public enum UnitType: byte
    {
        //角色类型
        [Name("宠物")]
        Pet            = 1  << 0, //宠物
        [Name("真实的玩家")]
        Player         = 1  << 1, //人物
        Monster        = 1  << 2, //怪物
        NPC            = 1  << 3, //
        Robot          = 1  << 4, //
    }
    
    /// <summary>
    /// 使用必须是三种类型组合使用，每种类型至少存在一个
    /// 如：
    /// 1：（Self|Role|Alive）表示目标为自己的角色且活着
    /// 2：(Self|Partner|Role|Pet|Alive)表示我方所有活着的角色
    /// </summary>
    [Flags]
    public enum UnitFaction
    {
        //阵营
        [Name("自己")]
        Self           = 1 << 0, //自己
        [Name("队友")]
        Partner        = 1 << 1, //伙伴，不包括自己
        [Name("敌方")]
        Enemy          = 1 << 2, //敌方
    }

    //阵营
    public enum UnitCamp
    {
        Blue,
        Red,
    }
    
    [Flags]
    public enum AliveType
    {
        //状态
        [Name("存活")]
        Alive          = 1 << 0, //活着的
        [Name("死亡")]
        Die            = 1 << 1, //死的
    }
}