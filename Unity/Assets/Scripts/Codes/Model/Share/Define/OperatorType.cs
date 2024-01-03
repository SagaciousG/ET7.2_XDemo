namespace ET
{
    public enum OperatorType
    {
        Add,
        Sub,
    }

    public enum CompareSymbol
    {
        [Name("大于")]
        Large,
        [Name("小于")]
        Less,
        [Name("等于")]
        Equal,
        [Name("大于等于")]
        NotLess,
        [Name("小于等于")]
        NotLarge,
        [Name("不等于")]
        NotEqual,
    }
}