using Unity.Mathematics;

namespace ET
{
    public struct MoveStart
    {
        public Unit Unit;
    }

    public struct MoveStop
    {
        public Unit Unit;
    }
    
    public struct ChangePosition
    {
        public Unit Unit;
        public float3 OldPos;
    }

    public struct ChangeRotation
    {
        public Unit Unit;
    }

    public struct UnitLevelChange
    {
        public Unit Unit;
        public int OldLevel;
    }
    
    public struct UnitProfessionUpdate
    {
        public Unit Unit;
    }
}