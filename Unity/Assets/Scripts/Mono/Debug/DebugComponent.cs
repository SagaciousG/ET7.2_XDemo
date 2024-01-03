using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

namespace ET
{
    public class DebugComponent : MonoBehaviour
    {
        private static List<RaycastResult> m_RaycastResult = new List<RaycastResult>();
        public static void New()
        {
            var obj = new GameObject("[Debug]");
            UnityEngine.Object.DontDestroyOnLoad(obj);
            _instance = obj.AddComponent<DebugComponent>();
        }

        private static DebugComponent _instance;
        
        private void Awake()
        {
            _instance = this;
            Application.logMessageReceivedThreaded += this.OnLog;
            
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
            {
                m_RaycastResult.Clear();
                if (Input.GetMouseButtonDown(1) && Input.GetKey(KeyCode.LeftControl))
                {
                    PointerEventData data = new(UnityEngine.EventSystems.EventSystem.current);
                    data.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                    UnityEngine.EventSystems.EventSystem.current.RaycastAll(data, m_RaycastResult);
                    if (m_RaycastResult.Count > 0)
                    {
                        UnityEditor.EditorGUIUtility.PingObject(m_RaycastResult[0].gameObject);
                        return;
                    }

                    var result = RayUtil.RayCast(Camera.main, Input.mousePosition, 0);
                    if (result.HasValue)
                    {
                        UnityEditor.EditorGUIUtility.PingObject(result.Obj);
                    }
                }
            }
#endif
        }

        private void OnDestroy()
        {
            _instance = null;
            Application.logMessageReceivedThreaded -= this.OnLog;
        }
        
        private void OnLog(string condition, string stacktrace, LogType type)
        {
            ConsoleLogs.Instance.Add(condition, stacktrace, type);
        }
    }
}