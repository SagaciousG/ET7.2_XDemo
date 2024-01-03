using System;
using System.Collections;
using ET;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace ET
{
    public class DebugWindowServerMsg : DebugWindowBase
    {
        private string[] _tabNames = new[] {"Client", "Server", "All" };
        private int[] _tabVals = new[] { 1, 2, 3 };
        private int _selectedIndex;
        private bool _locked = false;
        private string _searchStr;
        private Vector2 _scrollPos;
        private Vector2 _scrollPos2;
        private Vector2 _scrollPos3;

        private int _selectedLog;
        private bool _showStack = true;
        private object _serializedObj;
        private bool _showInner;
        protected override void OnDrawWindow(int id)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Clear"))
            {
                ServerMessageLogs.Instance.Msgs.Clear();
            }


            this._searchStr = GUILayout.TextField(this._searchStr, GUILayout.Width(200));
            
            GUILayoutHelper.ToogleButton(ref this._locked, new GUIContent("Lock"));
            GUILayoutHelper.ToogleButton(ref this._showStack, new GUIContent("ShowStack"));
            
            GUILayout.FlexibleSpace();
            GUILayoutHelper.ToogleButton(ref this._showInner, new GUIContent("ShowInner"));
            this._selectedIndex = GUILayout.Toolbar(this._selectedIndex, this._tabNames);
            GUILayout.EndHorizontal();
            
            var list = ServerMessageLogs.Instance.Msgs;
            var flag = this._tabVals[this._selectedIndex];

            if (this._locked)
            {
                this._scrollPos = new Vector2(0, Single.MaxValue);
            }
            GUILayout.BeginHorizontal();
            this._scrollPos = GUILayout.BeginScrollView(this._scrollPos, GUILayout.Width(0.4f * this._windowRect.width));
            var index = 0;
            for (int i = 0; i < list.Count; i++)
            {
                ServerMessageLogs.MsgContent content = list[i];
                if (((int)content.logType & flag) != (int)content.logType)
                {
                    continue;
                }
                if (!this._showInner && content.msgType == ServerMessageLogs.MsgType.Inner)
                    continue;

                if (!string.IsNullOrEmpty(this._searchStr))
                {
                    if (!content.title.Contains(this._searchStr) && !content.opCode.ToString().Contains(this._searchStr))
                    {
                        continue;
                    }
                }

                GUI.backgroundColor = i == this._selectedLog ? Color.cyan : Color.white;
                var rect = new Rect(0, index * 50, 0.4f * this._windowRect.width, 50);
                GUILayout.BeginArea(rect, "", "FrameBox");
                {
                    GUILayout.BeginHorizontal();
                    if (content.logType == ServerMessageLogs.LogType.Client)
                    {
                        GUI.backgroundColor = content.isSend? Color.green : Color.white;
                    }
                    else
                    {
                        GUI.backgroundColor = content.isSend? Color.blue : Color.yellow;
                    }
                    var style = new GUIStyle("ColorPickerCurrentExposureSwatchBorder");
                    style.alignment = TextAnchor.MiddleCenter;
                    style.richText = true;
                    if (content.isSend)
                    {
                        if (content.msgType == ServerMessageLogs.MsgType.Outer)
                        {
                            if (content.logType == ServerMessageLogs.LogType.Client)
                            {
                                GUILayout.Box("<color=white>C2S</color>", style, GUILayout.Width(40), GUILayout.Height(45));
                            }
                            else
                            {
                                GUILayout.Box("<color=white>InS</color>", style, GUILayout.Width(40), GUILayout.Height(45));
                            }
                        }
                        else
                        {
                            GUILayout.Box("<color=white>In</color>", style, GUILayout.Width(40), GUILayout.Height(45));
                        }
                    }
                    else
                    {
                        if (content.logType == ServerMessageLogs.LogType.Client)
                            GUILayout.Box("<color=white>S2C</color>", style, GUILayout.Width(40), GUILayout.Height(45));
                        else
                            GUILayout.Box("<color=white>RD</color>", style, GUILayout.Width(40), GUILayout.Height(45));
                    }
                    GUI.backgroundColor = i == this._selectedLog ? Color.cyan : Color.white;
                    GUILayout.BeginVertical();
                    var desc = content.desc.IsNullOrEmpty() ? "" : $"<color=#888888>[{content.desc}]</color>";
                    GUILayout.Label($"{DateTime.FromBinary(content.timeTick):[hh:mm:ss]} [{content.opCode}] [{content.symbol}] {desc}");
                    GUILayout.Label($"{content.title}");
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                }
                GUI.backgroundColor = Color.white;
                GUILayout.EndArea();
                GUILayoutUtility.GetRect(rect.width, rect.height);

                if (Event.current.type == UnityEngine.EventType.MouseDown)
                {
                    if (rect.Contains(Event.current.mousePosition))
                    {
                        this._selectedLog = i;
                        this._serializedObj = MongoDB.Bson.Serialization.BsonSerializer.Deserialize(content.msg, content.type);
                        this._window.Repaint();
                    }
                }

                index++;
            }
            GUILayout.EndScrollView();

            GUILayout.BeginVertical();
            var height = this._showStack ? this._windowRect.height * 0.5f : this._windowRect.height;
            this._scrollPos2 = GUILayout.BeginScrollView(this._scrollPos2, GUILayout.Height(height));
            if (this._serializedObj != null)
            {
                if (this._selectedLog > 0 && this._selectedLog < list.Count)
                {
                    var content = list[this._selectedLog];
                    var dateTime = DateTime.FromFileTime(content.timeTick);
                    EditorGUILayout.SelectableLabel($"Time :  {dateTime:yyyy-MM-d hh:mm:ss} {dateTime.Millisecond}");
                    EditorGUILayout.SelectableLabel($"Zone : {content.zone}");
                    EditorGUILayout.SelectableLabel($"Actor : {content.actorId}");
                    EditorGUILayout.SelectableLabel($"MsgType : {content.msgType}");
                    EditorGUILayout.SelectableLabel($"Opcode : {content.opCode} {content.title}");
                    TypeDrawHelper.BeginDraw(this._serializedObj);
                }
            }
            GUILayout.EndScrollView();

            this._scrollPos3 = GUILayout.BeginScrollView(this._scrollPos3);
            if (this._selectedLog >= 0 && this._selectedLog < list.Count)
            {
                var content = list[this._selectedLog];
                var logs = content.stack.Split('\n');
                var style = new GUIStyle() { wordWrap = true, stretchWidth = true, richText = true, };
                foreach (string str in logs)
                {
                    if (DebugWindowLog.IgnoreTrack(str))
                        continue;
                    var color = "#ffffff";
                    var msg = DebugWindowLog.StacktraceWithHyperlinks(str, out var file, out var line);
                    if (GUILayout.Button($"<color={color}>{msg}</color>", style))
                    {
#if UNITY_EDITOR
                        if (!string.IsNullOrEmpty(file))
                            InternalEditorUtility.OpenFileAtLineExternal(file, line);
#endif
                    }
                }
            }

            GUILayout.EndScrollView();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }
    }
}