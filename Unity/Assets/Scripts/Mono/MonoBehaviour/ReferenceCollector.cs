using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//Object并非C#基础中的Object，而是 UnityEngine.Object
using Object = UnityEngine.Object;

[Serializable]
public class ReferenceCollectorGroup : ISerializationCallbackReceiver
{
	public string key;
    public List<ReferenceCollectorData> Datas;
    
    public readonly Dictionary<string, Object> dict = new();
    public void OnBeforeSerialize()
    {
	    
    }

    public void OnAfterDeserialize()
    {
	    dict.Clear();
	    foreach (ReferenceCollectorData referenceCollectorData in Datas)
	    {
			dict[referenceCollectorData.key] = referenceCollectorData.gameObject;
	    }
    }
}

[Serializable]
public class ReferenceCollectorData
{
	public string key;
	public Object gameObject;
}

public class ReferenceCollector: MonoBehaviour, ISerializationCallbackReceiver
{
	public List<ReferenceCollectorGroup> data = new ();
    private readonly Dictionary<string, ReferenceCollectorGroup> dict = new();

#if UNITY_EDITOR
	public void Add(string group, Object obj)
	{
		var key = obj.name;
		UnityEditor.SerializedObject serializedObject = new UnityEditor.SerializedObject(this);
		foreach (var collectorGroup in data)
		{
			if (collectorGroup.key == group)
			{
				var find = false;
				foreach (var collectorData in collectorGroup.Datas)
				{
					if (collectorData.key == key)
					{
						collectorData.gameObject = obj;
						find = true;
						break;
					}
				}

				if (!find)
				{
					collectorGroup.Datas.Add(new ReferenceCollectorData(){gameObject = obj, key = key});
				}
				break;
			}
		}
        //应用与更新
        UnityEditor.EditorUtility.SetDirty(this);
		serializedObject.ApplyModifiedProperties();
		serializedObject.UpdateIfRequiredOrScript();
	}
    //删除元素，知识点与上面的添加相似
	public void Remove(string group, string key)
	{		
		UnityEditor.SerializedObject serializedObject = new UnityEditor.SerializedObject(this);
		foreach (var collectorGroup in data)
		{
			if (collectorGroup.key == group)
			{
				for (var i = 0; i < collectorGroup.Datas.Count; i++)
				{
					var collectorData = collectorGroup.Datas[i];
					if (collectorData.key == key)
					{
						collectorGroup.Datas.RemoveAt(i);
						break;
					}
				}
				break;
			}
		}
		
		UnityEditor.EditorUtility.SetDirty(this);
		serializedObject.ApplyModifiedProperties();
		serializedObject.UpdateIfRequiredOrScript();
	}

	public void Clear()
	{
		UnityEditor.SerializedObject serializedObject = new UnityEditor.SerializedObject(this);
        //根据PropertyPath读取prefab文件中的数据
        //如果不知道具体的格式，可以直接右键用文本编辑器打开，搜索data就能找到
        var dataProperty = serializedObject.FindProperty("data");
		dataProperty.ClearArray();
		UnityEditor.EditorUtility.SetDirty(this);
		serializedObject.ApplyModifiedProperties();
		serializedObject.UpdateIfRequiredOrScript();
	}

	public void Sort()
	{
		UnityEditor.SerializedObject serializedObject = new UnityEditor.SerializedObject(this);
		foreach (var group in data)
		{
			group.Datas.Sort((a, b) => string.CompareOrdinal(a.key, b.key));
		}
		UnityEditor.EditorUtility.SetDirty(this);
		serializedObject.ApplyModifiedProperties();
		serializedObject.UpdateIfRequiredOrScript();
	}
#endif
    //使用泛型返回对应key的gameobject
	public T Get<T>(string key, string group = null) where T : class
	{
		if (string.IsNullOrEmpty(group))
		{
			var first = data.First();
			if (first == null)
				return null;
			first.dict.TryGetValue(key, out var obj);
			return obj as T;
		}

		if (dict.TryGetValue(group, out var g))
		{
			g.dict.TryGetValue(key, out var obj);
			return obj as T;
		}

		return null;
	}
	
	public void OnBeforeSerialize()
	{
	}
    //在反序列化后运行
	public void OnAfterDeserialize()
	{
		dict.Clear();
		foreach (var group in data)
		{
			dict[group.key] = group;
		}
	}
}
