using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public class ConsoleLogs
    {
        public static ConsoleLogs Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ConsoleLogs();
                return _instance;
            }
        }

        public int logCount => this._logCount;
        public int warnCount => this._warnCount;
        public int errorCount => this._errorCount;
        public int fatalCount => this._fatalCount;
        public Queue<LogInfo> Logs => this._errorLogs;

        private static ConsoleLogs _instance;
        private Queue<LogInfo> _logs = new Queue<LogInfo>();
        private Queue<LogInfo> _errorLogs = new Queue<LogInfo>();

        private int _logCount;
        private int _warnCount;
        private int _errorCount;
        private int _fatalCount;
        
        public void Add(string title, string stack, LogType type)
        {
            if (title.StartsWith("[Message]")) //协议消息
            {
                ServerMessageLogs.Instance.Add(title.Replace("[Message]", ""), stack);
                return;
            }
            
            
            var hour = DateTime.Now.Hour;
            var min = DateTime.Now.Minute;
            var second = DateTime.Now.Second;
            var id = this._logs.Count;
            var logInfo = new LogInfo()
            {
                stack = stack,
                title = title,
                logType = type,
                Hour = hour,
                Minute = min,
                Second = second,
                id = id
            };
            switch (type)
            {
                case LogType.Log:
                case LogType.Assert:
                    this._logCount++;
                    this._logs.Enqueue(logInfo);
                    break;
                case LogType.Warning:
                    this._warnCount++;
                    this._logs.Enqueue(logInfo);
                    break;
                case LogType.Error:
                    this._errorCount++;
                    this._errorLogs.Enqueue(logInfo);
                    break;
                case LogType.Exception:
                    this._fatalCount++;
                    this._errorLogs.Enqueue(logInfo);
                    break;
            }
            
            if (_logs.Count > 1000)
            {
                var log = _logs.Dequeue();
                switch (log.logType)
                {
                    case LogType.Log:
                    case LogType.Assert:
                        this._logCount--;
                        break;
                    case LogType.Warning:
                        this._warnCount--;
                        break;
                }
            }
            
            if (_errorLogs.Count > 1000)
            {
                var log = _errorLogs.Dequeue();
                switch (log.logType)
                {
                    case LogType.Error:
                        this._errorCount--;
                        break;
                    case LogType.Exception:
                        this._fatalCount--;
                        break;
                }
            }
        }

        public void Clear()
        {
            this._logs.Clear();
            this._errorLogs.Clear();
            _errorCount = 0;
            _fatalCount = 0;
            _logCount = 0;
            _warnCount = 0;
        }
        
        

        public struct LogInfo
        {
            public string title;
            public string stack;
            public LogType logType;
            public int Hour;
            public int Minute;
            public int Second;
            public int repeated;
            public int id;
        }
    }
}