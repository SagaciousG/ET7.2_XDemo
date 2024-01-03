using System.Collections.Generic;

namespace ET
{
    //技能测试角色单位
    [ChildOf(typeof(TestUser))]
    public class TestUnit : Entity, IAwake, ISerializeToEntity
    {
        public UnitType UnitType { get; set; }
        public ProfessionNum ProfessionNum { get; set; }
        
        public int Pos { get; set; }
        public int Map { get; set; }
        public int UnitShow { get; set; }
        public string Name { get; set; }
        
    }
}