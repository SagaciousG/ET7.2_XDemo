namespace ET
{
    public static class EntryWordHelper
    {
        public static void Parse(Unit unit, int word, int wordArg, OperatorType operatorType)
        {
            var cfg = EntryWordConfigCategory.Instance.Get(word);
            var numericComponent = unit.GetComponent<NumericComponent>();
            switch ((EntryWordType)cfg.Type)
            {
                case EntryWordType.Property:
                {
                    var propertyConfig = PropertyConfigCategory.Instance.Get(cfg.Param1.ToInt32());
                    var val = numericComponent.GetAsInt(propertyConfig.Key * 10 + 2);
                    switch (operatorType)
                    {
                        case OperatorType.Add:
                            val += wordArg;
                            break;
                        case OperatorType.Sub:
                            val -= wordArg;
                            break;
                    }
                    numericComponent.Set(propertyConfig.Key * 10 + 2, val);
                    break;
                }
                case EntryWordType.PropertyPercent:
                {
                    var propertyConfig = PropertyConfigCategory.Instance.Get(cfg.Param1.ToInt32());
                    var val = numericComponent.GetAsInt(propertyConfig.Key * 10 + 3);
                    switch (operatorType)
                    {
                        case OperatorType.Add:
                            val += wordArg;
                            break;
                        case OperatorType.Sub:
                            val -= wordArg;
                            break;
                    }
                    numericComponent.Set(propertyConfig.Key * 10 + 3, val);
                    break;
                }
            }
        }
    }
}