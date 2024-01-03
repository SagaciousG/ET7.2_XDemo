using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ET
{
    public abstract class DebugWindowBase
    {  
        protected Rect _windowRect => this._window.position;
        protected EditorWindow _window;
        private bool _controlSize;
        private Vector2 _startPos;
        private Vector2 _startSize;

        public void Init(EditorWindow win)
        {
            this._window = win;
        }
        
        public void Draw()
        {
            OnDrawWindow(0);
        }

        protected abstract void OnDrawWindow(int id);
    }
}