using UnityEngine;
using YooAsset;

namespace ET
{
    public enum CodeMode
    {
        Client = 1,
        Server = 2,
        ClientServer = 3,
    }

    public enum HotfixMode
    {
        None,
        Bundle,
    }
    
    [CreateAssetMenu(menuName = "ET/CreateGlobalConfig", fileName = "GlobalConfig", order = 0)]
    public class GlobalConfig: ScriptableObject
    {
        public bool DebugOpen = true;
        public CodeMode CodeMode;
        public EPlayMode EPlayMode;
        public bool UseLocalCodes;

        public bool AutoLogin;
        public string Account = "123";
        public string Password = "123";
        public bool SkillTestMode;
        public bool EnterDummy;
    }
}