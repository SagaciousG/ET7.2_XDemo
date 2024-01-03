using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ET
{

	public class UIReferenceCollector: MonoBehaviour
	{
		//用于序列化的List
		public List<UnityEngine.Object> data = new List<UnityEngine.Object>();
		//Object并非C#基础中的Object，而是 UnityEngine.Object
		private readonly Dictionary<string, UnityEngine.Object> dict = new();
		private bool _isInit;

		 public T Get<T>(string key) where T : class
		 {
		 	if (!_isInit)
		 	{
		 		foreach (var collectorData in data)
		 		{
		 			string fieldName = "";
		 			switch (collectorData)
		 			{
		 				case GameObject go:
		 					fieldName = go.name;
		 					break;
		 				case Component com:
		 					fieldName = com.gameObject.name;
		 					break;
		 			}
		 			dict[fieldName] = collectorData;
		 		}
		
		 		_isInit = true;
		 	}
		 	UnityEngine.Object dictGo;
		 	if (!dict.TryGetValue(key, out dictGo))
		 	{
		 		return null;
		 	}
		 	return dictGo as T;
		 }
	}
}