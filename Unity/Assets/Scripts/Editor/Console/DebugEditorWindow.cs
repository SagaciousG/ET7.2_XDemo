using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ET
{
    public static class DebugUpdate
    {
       
        static DebugUpdate()
        {
            EditorApplication.update += Update;
        }

        private static void Update()
        {
            
        }
    }
    
    public class DebugEditorWindow : EditorWindow
    {
        [MenuItem("Tools/Debug _F6")]
        static void ShowWin()
        {
            GetWindow<DebugEditorWindow>().Show();
        }

        private int _selectedWin;
        private int _count;
        private string[] _showNames = new[] { "协议" };
        private Type[] _windowTypes = new[]
        {
            // typeof (DebugWindowLog), 
            // typeof (DebugWindowTools),
            typeof (DebugWindowServerMsg)
        };

        private List<DebugWindowBase> _wins = new List<DebugWindowBase>();

        private void OnEnable()
        {
            this._wins.Clear();
            foreach (Type windowType in this._windowTypes)
            {
                var win = (DebugWindowBase)Activator.CreateInstance(windowType);
                this._wins.Add(win);
                win.Init(this);
            }
        }

        private void OnGUI()
        {
            this._selectedWin = GUILayout.Toolbar(this._selectedWin, this._showNames);
            var cur = this._wins[this._selectedWin];
            
            cur.Draw();
        }

        private void Update()
        {
            _count++;
            if (_count >= 30)
            {
                _count = 0;
                Repaint();
            }
        }
    }
}