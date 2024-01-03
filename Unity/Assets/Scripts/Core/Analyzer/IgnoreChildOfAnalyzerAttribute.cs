using System;

namespace ET
{
    //使指定类忽略AddChild分析器
    [AttributeUsage(AttributeTargets.Class)]
    public class IgnoreAddChildAnalyzerAttribute : Attribute
    {
    
    }
}