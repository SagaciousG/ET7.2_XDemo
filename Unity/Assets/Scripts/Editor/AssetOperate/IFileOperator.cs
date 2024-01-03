﻿using System.Collections.Generic;

namespace ET
{
    public interface IFileOperator
    {
        string Selected { set; get; }
        
        void Init(string[] allFiles, ref List<string> showFiles);
        void Top();
        void Bot();
    }
}