namespace ET
{
    public static class ErrorCodeHelper
    {
        public static string GetCodeLog(int code)
        {
            return ErrorCodeConfigCategory.Instance.Get(code)?.Log;
        }
        
        public static string GetCodeTips(int code)
        {
            var cfg = ErrorCodeConfigCategory.Instance.Get(code);
            if (cfg == null)
                return code.ToString();
            if (string.IsNullOrEmpty(cfg.ClientTips))
            {
                return cfg.Header;
            }
            return cfg.ClientTips;
        }
    }
}