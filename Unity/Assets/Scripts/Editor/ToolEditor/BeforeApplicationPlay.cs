using UnityEditor;
using UnityEngine;

namespace ET
{
    [InitializeOnLoad]
    public static class BeforeApplicationPlay
    {
        static BeforeApplicationPlay()
        {
            EditorApplication.playModeStateChanged += OnState;
        }

        private static void OnState(PlayModeStateChange obj)
        {
            switch (obj)
            {
                case PlayModeStateChange.ExitingEditMode:
                {
                    FileHelper.CleanDirectory("./Config");
                    FileHelper.CopyDirectory("../Config", "./Config");
                    Debug.Log("复制Config");
                    break;
                }
            }
        }
    }
}