using System;
using System.Collections.Generic;
using ET;
using UnityEngine;
using UnityEngine.EventSystems;
using EventSystem = UnityEngine.EventSystems.EventSystem;

namespace ET
{
    public enum InputState
    {
        Down,
        Holding,
        Up
    }

    public enum InputEventType
    {
        Unknown,
        Zoom,
        Click,
        DragBegin,
        Drag,
        DragEnd,
        Hold,
        ContextClick,
        ContextDragBegin,
        ContextDrag,
        ContextDragEnd,
        ContextHold,
    }

    public struct InputData
    {
        public InputState inputState;
        public InputEventType eventType;
        public Vector2 position;
        
        public Vector2 beginPos;
        public Vector2 endPos;
        public Vector2 lastPos; //上一帧的位置
        public float holdDuration;
        public Vector2 zoomDelta;

        public Vector2 PosDetail => position - lastPos;
    }

    public struct InputListener
    {
        public int Id;
        public Action<InputData, object> Func;
        public object arg;
    }
    
    public class InputAwakeSystem : AwakeSystem<InputComponent>
    {
        protected override void Awake(InputComponent self)
        {
            InputComponent.Instance = self;
        }
    }

    public class InputUpdateSystem : UpdateSystem<InputComponent>
    {
        protected override void Update(InputComponent self)
        {
            var scroll = Input.GetAxis("Mouse ScrollWheel");
            var mousePosition = Input.mousePosition;
            if (Math.Abs(scroll) > 0.01f)
            {
                self.isOverUI = RayUtil.IsOverUI();
                self.isGetMouse = false;
                self.beginPos = mousePosition;
                self.beginTime = Time.time;
                self.eventType = InputEventType.Zoom;
                self.zoomDelta = Input.mouseScrollDelta;
                
                self.FireEvent(InputState.Holding);
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                self.isOverUI = RayUtil.IsOverUI();
                self.isGetMouse = true;
                self.beginPos = mousePosition;
                self.beginTime = Time.time;
                self.dragBeginInvoke = true;
                self.eventType = InputEventType.Unknown;
                
                self.FireEvent(InputState.Down);
            }

            var leftTime = Time.time - self.beginTime;
            
            if ((Input.GetMouseButtonUp(0)) && self.isGetMouse)
            {
                self.isGetMouse = false;
                
                if (self.eventType == InputEventType.Unknown)
                {
                    self.eventType = InputEventType.Click;
                }
                else if (self.eventType == InputEventType.Drag || self.eventType == InputEventType.DragBegin)
                {
                    self.eventType = InputEventType.DragEnd;
                }
                self.FireEvent(InputState.Up);
            }

            if (self.isGetMouse)
            {
                switch (self.eventType)
                {
                    case InputEventType.Drag:
                    case InputEventType.DragBegin:
                    case InputEventType.DragEnd:
                        if (!self.isOverUI)
                        {
                            //拖拽事件
                            if (self.dragBeginInvoke)
                            {
                                self.eventType = InputEventType.DragBegin;
                                self.dragBeginInvoke = false;
                            }
                            else
                            {
                                self.eventType = InputEventType.Drag;
                            }
                        }
                        break;
                    case InputEventType.Hold:
                        //从hold状态可以被转化为Drag
                        if ( Vector3.Distance(mousePosition, self.beginPos) > self.MoveDistance)
                        {
                            self.eventType = InputEventType.Drag;
                        }
                        break;
                    default:
                        if (Vector3.Distance(mousePosition, self.beginPos) > self.MoveDistance)
                        {
                            self.eventType = InputEventType.Drag;
                        }
                        else if (leftTime > self.HoldDecision)
                        {
                            self.eventType = InputEventType.Hold;
                        }
                        break;
                }
                self.FireEvent(InputState.Holding);
            }

            self.lastPos = mousePosition;
        }
    }

    [FriendOf(typeof(InputComponent))]
    public static class InputComponentSystem
    {
        public static void FireEvent(this InputComponent self, InputState state)
        {
            var data = new InputData()
            {
                eventType = self.eventType,
                inputState = state,
                position = Input.mousePosition,
                holdDuration = Time.time - self.beginTime,
                beginPos = self.beginPos,
                endPos = Input.mousePosition,
                lastPos = self.lastPos,
                zoomDelta = self.zoomDelta,
            };
            if (self.isOverUI)
            {
                for (var i = self.InputUIOnlyListeners.Count - 1; i >= 0; i--)
                {
                    var listener = self.InputUIOnlyListeners[i];
                    listener.Func?.Invoke(data, listener.arg);
                }
            }
            else
            {
                for (var i = self.InputBlockByUIListeners.Count - 1; i >= 0; i--)
                {
                    var listener = self.InputBlockByUIListeners[i];
                    listener.Func?.Invoke(data, listener.arg);
                }
            }

            for (var i = self.InputListeners.Count - 1; i >= 0; i--)
            {
                var listener = self.InputListeners[i];
                listener.Func?.Invoke(data, listener.arg);
            }
        }

        /// <summary>
        /// 点击非UI区域触发
        /// </summary>
        /// <param name="self"></param>
        /// <param name="func"></param>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static int AddBlockUIListener(this InputComponent self, Action<InputData, object> func, object arg = null)
        {
            self.InputBlockByUIListeners.Insert(0, new InputListener(){Func = func, arg = arg, Id = func.GetHashCode()});
            return func.GetHashCode();
        }
        
        /// <summary>
        /// 任意位置触发
        /// </summary>
        /// <param name="self"></param>
        /// <param name="func"></param>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static int AddListener(this InputComponent self, Action<InputData, object> func, object arg = null)
        {
            self.InputListeners.Insert(0, new InputListener(){Func = func, arg = arg, Id = func.GetHashCode()});
            return func.GetHashCode();
        }
        
        /// <summary>
        /// 点击在UI上触发
        /// </summary>
        /// <param name="self"></param>
        /// <param name="func"></param>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static int AddUIOnlyListener(this InputComponent self, Action<InputData, object> func, object arg = null)
        {
            self.InputUIOnlyListeners.Insert(0, new InputListener(){Func = func, arg = arg, Id = func.GetHashCode()});
            return func.GetHashCode();
        }
        
        public static void RemoveListener(this InputComponent self, Action<InputData, object> func)
        {
            self.InputListeners.RemoveAll(a => a.Id == func.GetHashCode());
            self.InputBlockByUIListeners.RemoveAll(a => a.Id == func.GetHashCode());
            self.InputUIOnlyListeners.RemoveAll(a => a.Id == func.GetHashCode());
        }
        
        public static void RemoveListener(this InputComponent self, int id)
        {
            self.InputListeners.RemoveAll(a => a.Id == id);
            self.InputBlockByUIListeners.RemoveAll(a => a.Id == id);
            self.InputUIOnlyListeners.RemoveAll(a => a.Id == id);
        }
    }

    [ComponentOf(typeof(Scene))]
    public class InputComponent : Entity, IUpdate, IAwake
    {
        [StaticField]
        public static InputComponent Instance;
        
        public float MoveDistance = 0.1f;
        public float HoldDecision = 0.5f;
        
        public bool isGetMouse = false;

        public Vector3 beginPos;
        public Vector3 lastPos;
        public float beginTime;

        public bool dragBeginInvoke;

        public InputEventType eventType;
        public bool isOverUI;

        public Vector2 zoomDelta;
        
        public List<InputListener> InputBlockByUIListeners = new List<InputListener>();
        public List<InputListener> InputUIOnlyListeners = new List<InputListener>();
        public List<InputListener> InputListeners = new List<InputListener>();
    }
}