namespace ET
{
    [UniqueId(100, 20000)]
    public static class TimerInvokeType
    {
        // 框架层100-200，逻辑层的timer type从200起
        public const int WaitTimer = 100;
        public const int SessionIdleChecker = 101;
        public const int ActorLocationSenderChecker = 102;
        public const int ActorMessageSenderChecker = 103;
        
        // 框架层100-200，逻辑层的timer type 200-300
        public const int MoveTimer = 201;
        public const int AITimer = 202;
        public const int SessionAcceptTimeout = 203;
        public const int UnitSaveData = 204;
        public const int MapSaveTimer = 205;
        
        //300+
        public const int PVERoundCountDown = 301;
        
        //400+
        public const int SkillRunTimelineTimer = 400; // +35=客户端用 +6=服务器用
   
        
        //客户端用TimerType 10000+
        public const int UIBattleRoundCountDown = 10001;
        
        public const int BuildMapAutoSave = 10011;
        public const int UIBuildMapTemplateAutoSave = 10012;
    }
}