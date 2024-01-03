using System;
using UnityEditor;
using UnityEngine;
using ET;

namespace ET
{
    public class GlobalConfigEditor : EditorWindow
    {
        [MenuItem("Tools/GlobeConfig")]
        static void ShowWin()
        {
            GetWindow<GlobalConfigEditor>().Show();
        }

        private GlobalConfig _config;
        private void OnEnable()
        {
            this._config = Resources.Load<GlobalConfig>("GlobalConfig");
        }

        private void OnGUI()
        {
            EditorGUI.BeginChangeCheck();
            GUILayoutHelper.EnumPopup("代码模式", ref this._config.CodeMode);
            GUILayoutHelper.EnumPopup("热更新", ref this._config.EPlayMode);
            this._config.DebugOpen = EditorGUILayout.Toggle("开启Debug", this._config.DebugOpen);
            this._config.UseLocalCodes = EditorGUILayout.Toggle("使用本地代码", this._config.UseLocalCodes);
            this._config.SkillTestMode = EditorGUILayout.Toggle("开启技能测试", this._config.SkillTestMode);
            if (this._config.SkillTestMode)
            {
                this._config.AutoLogin = EditorGUILayout.Toggle("自动登录", this._config.AutoLogin);
                if (this._config.AutoLogin)
                {
                    this._config.Account = EditorGUILayout.TextField("账号", this._config.Account);
                    this._config.Password = EditorGUILayout.TextField("密码", this._config.Password);
                    this._config.EnterDummy = EditorGUILayout.Toggle("自动进入木桩测试", this._config.EnterDummy);
                }
            }

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(this._config);
                AssetDatabase.SaveAssets();
            }
        }
    }
}