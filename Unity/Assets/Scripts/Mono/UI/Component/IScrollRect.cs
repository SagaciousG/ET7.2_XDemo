using System;

namespace ET
{
    public interface IScrollRect
    {
        void AddScrollListener(Action onScroll);
    }
}