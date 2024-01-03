using System;
using System.Text;
using System.Text.RegularExpressions;

namespace ET
{
    public class UnityLogger: ILog
    {
        private static string[] IgnoreStackType = new []
        {
            "ET.UnityLogger",
            "ET.Log",
            "ET.Logger"
        };
        
        private static Regex _atFile = new Regex(@"at (.*) in (.*)\:(\d+)");
        private static StringBuilder msgSB = new StringBuilder();
        private static StringBuilder lineSB = new StringBuilder();
        
        public void Trace(string message)
        {
            message = StacktraceWithHyperlinks(message);
            UnityEngine.Debug.Log(message);
        }

        public void Debug(string message)
        {
            message = StacktraceWithHyperlinks(message);
            UnityEngine.Debug.Log(message);
        }

        public void Info(string message)
        {
            message = StacktraceWithHyperlinks(message);
            UnityEngine.Debug.Log(message);
        }

        public void Warning(string message)
        {
            message = StacktraceWithHyperlinks(message);
            UnityEngine.Debug.LogWarning(message);
        }

        public void Error(string message)
        {
            message = StacktraceWithHyperlinks(message);
            UnityEngine.Debug.LogError(message);
        }

        public void Error(Exception e)
        {
            UnityEngine.Debug.LogException(e);
        }

        public void Trace(string message, params object[] args)
        {
            message = StacktraceWithHyperlinks(message);
            UnityEngine.Debug.LogFormat(message, args);
        }

        public void Warning(string message, params object[] args)
        {
            message = StacktraceWithHyperlinks(message);
            UnityEngine.Debug.LogWarningFormat(message, args);
        }

        public void Info(string message, params object[] args)
        {
            message = StacktraceWithHyperlinks(message);
            UnityEngine.Debug.LogFormat(message, args);
        }

        public void Debug(string message, params object[] args)
        {
            message = StacktraceWithHyperlinks(message);
            UnityEngine.Debug.LogFormat(message, args);
        }

        public void Error(string message, params object[] args)
        {
            message = StacktraceWithHyperlinks(message);
            UnityEngine.Debug.LogErrorFormat(message, args);
        }
        
        
        private static bool IgnoreTrack(string line)
        {
            foreach (string s in IgnoreStackType)
            {
                if (line.StartsWith(s))
                    return true;
            }

            return false;
        }

        private static string StacktraceWithHyperlinks(string stacktraceText)
        {
            msgSB.Clear();
            var ss = stacktraceText.Split('\n');
            foreach (string s in ss)
            {
                if (string.IsNullOrEmpty(s))
                    continue;
                if (IgnoreTrack(s))
                    continue;
                msgSB.AppendLine(StacktraceWithHyperlinksAtLine(s));
            }

            return msgSB.ToString();
        }

        private static string StacktraceWithHyperlinksAtLine(string stacktraceText)
        {
            lineSB.Clear();
            string str1 = ") (at ";
            int num1 = stacktraceText.IndexOf(str1, StringComparison.Ordinal);
            if (num1 > 0)
            {
                int num2 = num1 + str1.Length;
                if (stacktraceText[num2] != '<')
                {
                    string str2 = stacktraceText.Substring(num2);
                    int length = str2.LastIndexOf(":", StringComparison.Ordinal);
                    if (length > 0)
                    {
                        int num3 = str2.LastIndexOf(")", StringComparison.Ordinal);
                        if (num3 > 0)
                        {
                            string str3 = str2.Substring(length + 1, num3 - (length + 1));
                            string str4 = str2.Substring(0, length);
                            lineSB.Append(stacktraceText.Substring(0, num2));
                            lineSB.Append("<a href=\"" + str4 + "\" line=\"" + str3 + "\">");
                            lineSB.Append(str4 + ":" + str3);
                            lineSB.Append("</a>)");
                        }
                    }
                }
            }
            else if (_atFile.IsMatch(stacktraceText))
            {
                var result = _atFile.Match(stacktraceText);
                lineSB.Append(result.Groups[1]);
                lineSB.Append($"(at <a href=\"{result.Groups[2]}\" line=\"{result.Groups[3]}\">{result.Groups[2]}:{result.Groups[3]}</a>)");
            }
            else
            {
                return stacktraceText;
            }

            return lineSB.ToString();
        }
    }
}