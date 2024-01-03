using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace ET.Client
{
    [Invoke]
    public class GetAllClientConfigBytes: AInvokeHandler<ConfigComponent.GetAllConfigBytes, Dictionary<Type, byte[]>>
    {
        public override Dictionary<Type, byte[]> Handle(ConfigComponent.GetAllConfigBytes args)
        {
            Dictionary<Type, byte[]> output = new Dictionary<Type, byte[]>();
            HashSet<Type> configTypes = EventSystem.Instance.GetTypes(typeof (ConfigAttribute));
            foreach (Type configType in configTypes)
            {
                var configAttribute = configType.GetCustomAttribute<ConfigAttribute>();
                var v = YooAssetHelper.LoadAssetSync<TextAsset>($"{configType.Name}");
                if (v == null)
                {
                    Debug.LogError($"不存在配置文件{configType.Name}");
                    continue;
                }
                output[configType] = v.bytes;
            }

            return output;
        }
    }
    
    [Invoke]
    public class GetOneClientConfigBytes: AInvokeHandler<ConfigComponent.GetOneConfigBytes, byte[]>
    {
        public override byte[] Handle(ConfigComponent.GetOneConfigBytes args)
        {
            var v = YooAssetHelper.LoadAssetSync<TextAsset>($"{args.ConfigName}");
            if (v == null)
            {
                Debug.LogError($"不存在配置文件{args.ConfigName}或未生成配置{args.ConfigName}的路径映射");
                return null;
            }

            return v.bytes;
        }
    }
}