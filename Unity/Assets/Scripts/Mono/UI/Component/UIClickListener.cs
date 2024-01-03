using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ET
{
    public class UIClickListener : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        private List<IActionRun> _actions = new List<IActionRun>();
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.dragging)
                return;
            var currentOverGo = eventData.pointerCurrentRaycast.gameObject;
            var pointerUpHandler = ExecuteEvents.GetEventHandler<UIClickListener>(currentOverGo);
            if (eventData.button != PointerEventData.InputButton.Left)
                return;
            if (!eventData.eligibleForClick)
                return;
            if (eventData.pointerClick != pointerUpHandler)
                return;
            this.DoClick();
        }
        
        private void DoClick()
        {
            foreach (var action in _actions)
            {
                action.Run();
            }
        }

        public void AddClick(Action action)
        {
            _actions.Add(new ActionNoArg(){Action = action});
        }

        public void RemoveClick(Action action)
        {
            _actions.RemoveAll(a => a.UID() == action.GetHashCode());
        }
        
        public void AddClick<T>(Action<T> action, T par)
        {
            _actions.Add(new ActionWithArg<T>()
            {
                Action = action,
                arg = par
            });
        }

        public void RemoveClick<T>(Action<T> action)
        {
            _actions.RemoveAll(a => a.UID() == action.GetHashCode());
        }

        public void RemoveAllClick()
        {
            _actions.Clear();
        }
        
        private class ActionNoArg : IActionRun
        {
            public Action Action;
            public void Run()
            {
                Action?.Invoke();
            }
            
            public long UID()
            {
                return Action.GetHashCode();
            }
        }
        
        private class ActionWithArg<T> : IActionRun
        {
            public Action<T> Action;
            public T arg;
            public void Run()
            {
                Action?.Invoke(arg);
            }

            public long UID()
            {
                return Action.GetHashCode();
            }
        }
        
        private interface IActionRun
        {
            void Run();
            long UID();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            
        }

        public void OnPointerUp(PointerEventData eventData)
        {

        }
    }
}