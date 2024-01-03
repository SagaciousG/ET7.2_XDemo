using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public static class ExtensionEx
    {
        public static Vector3 RotateBy(this Vector3 position, Vector3 center, Vector3 angle)
        {
            var v1 = position.RotateRound(center, Vector3.forward, angle.z);
            var v2 = v1.RotateRound(center, Vector3.right, angle.x);
            var v3 = v2.RotateRound(center, Vector3.up, angle.y);
            return v3;
        }
        
        /// <summary>
        /// 围绕某点旋转指定角度
        /// </summary>
        /// <param name="position">自身坐标</param>
        /// <param name="center">旋转中心</param>
        /// <param name="axis">围绕旋转轴</param>
        /// <param name="angle">旋转角度</param>
        /// <returns></returns>
        public static Vector3 RotateRound(this Vector3 position, Vector3 center, Vector3 axis, float angle)
        {
            return Quaternion.AngleAxis(angle, axis) * (position - center) + center;
        }
        
        /// <summary>
        /// 计算一个Vector3绕指定轴旋转指定角度后所得到的向量。
        /// </summary>
        /// <param name="source">旋转前的源Vector3</param>
        /// <param name="axis">旋转轴</param>
        /// <param name="angle">旋转角度</param>
        /// <returns>旋转后得到的新Vector3</returns>
        public static Vector3 Rotate(this Vector3 source, Vector3 axis, float angle)
        {
            Quaternion q = Quaternion.AngleAxis(angle, axis); // 旋转系数
            return q * source; // 返回目标点
        }

        public static Vector3 RotateBy(this Vector3 source, Vector3 angle)
        {
            var v1 = source.Rotate(Vector3.forward, angle.z);
            var v2 = v1.Rotate(Vector3.right, angle.x);
            var v3 = v2.Rotate(Vector3.up, angle.y);
            return v3;
        }
        
        public static Vector2 ClampIn(this Rect rect, Vector2 r)
        {
            return new Vector2(Mathf.Clamp(r.x, rect.xMin, rect.xMax), Mathf.Clamp(r.y, rect.yMin, rect.yMax));
        }

        public static bool Contains(this Rect rect, Rect r)
        {
            return r.xMax <= rect.xMax
                   && r.yMax <= rect.yMax
                   && r.xMin >= rect.xMin
                   && r.yMin >= rect.yMin;
        }
        
        public static T AddComponentNotOwns<T>(this GameObject obj) where T : Component
        {
            if (obj.GetComponent<T>() == null)
                return obj.AddComponent<T>();
            return obj.GetComponent<T>();
        }

        public static bool ContainKey<T>(this List<T> list, int key)
        {
            if (key < 0 || list.Count < key)
                return false;
            return true;
        }

        public static Transform[] FindAllChildren(this GameObject obj)
        {
            var list = new List<Transform>();
            for (int i = 0; i < obj.transform.childCount; i++)
            {
                var child = obj.transform.GetChild(i);
                var children = child.gameObject.FindAllChildren();
                list.Add(child);
                list.AddRange(children);
            }

            return list.ToArray();
        }

        public static Rect Rect(this Bounds box)
        {
            var r = new Rect(Vector2.zero, new Vector2(box.size.x, box.size.z));
            r.center = new Vector2(box.center.x, box.center.z);
            return r;
        }
        
        public static void Display(this GameObject obj, bool show)
        {
            obj.SetActive(show);
            // if (obj.GetComponent<RectTransform>() != null)
            // {
            //     var cg = obj.GetComponent<CanvasGroup>();
            //     if (cg == null)
            //         cg = obj.AddComponent<CanvasGroup>();
            //     cg.alpha = show ? 1 : 0;
            //     cg.interactable = show;
            //     cg.blocksRaycasts = show;
            // }
            // else
            // {
            //     obj.SetActive(show);
            // }
        }

        public static bool IsDisplay(this GameObject obj)
        {
            return obj.activeSelf;
            // if (obj.GetComponent<RectTransform>() != null)
            // {
            //     var cg = obj.GetComponent<CanvasGroup>();
            //     if (cg == null)
            //         return true;
            //     return cg.alpha > 0;
            // }
            // else
            // {
            //     return obj.activeSelf;
            // }
        }
        
        public static T Last<T>(this T[] self)
        {
            return self[self.Length - 1];
        }

        public static T Last<T>(this List<T> self)
        {
            if (self.Count > 0)
                return self[self.Count - 1];
            return default;
        }

  

        /// <summary>
        /// 广度优先
        /// </summary>
        /// <param name="self"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Transform FindDepth(this Transform self, string name)
        {
            for (int i = 0; i < self.childCount; i++)
            {
                var trans = self.GetChild(i);
                if (trans.name == name)
                    return trans;
            }

            for (int i = 0; i < self.childCount; i++)
            {
                var trans = self.GetChild(i);
                var res = trans.FindDepth(name);
                if (res != null)
                    return res;
            }

            return null;
        }

        public static void OnClick(this Graphic graphic, Action action)
        {
            var btn = graphic.gameObject.AddComponentNotOwns<UIClickListener>();
            btn.AddClick(action);
        }
        
        public static void OnClick<T>(this Graphic graphic, Action<T> action, T argc)
        {
            var btn = graphic.gameObject.AddComponentNotOwns<UIClickListener>();
            btn.AddClick(action, argc);
        }
        
        public static void OnClick(this GameObject obj, Action action)
        {
            var graphic = obj.GetComponent<Graphic>();
            if (graphic == null)
                graphic = obj.AddComponent<EmptyGraphic>();
            var btn = graphic.gameObject.AddComponentNotOwns<UIClickListener>();
            btn.AddClick(action);
        }
        
        public static void OnClick<T>(this GameObject obj, Action<T> action, T argc)
        {
            var graphic = obj.GetComponent<Graphic>();
            if (graphic == null)
                graphic = obj.AddComponent<EmptyGraphic>();
            var btn = graphic.gameObject.AddComponentNotOwns<UIClickListener>();
            btn.AddClick(action, argc);
        }

        public static void OffClick(this Graphic graphic, Action action)
        {
            var btn = graphic.gameObject.AddComponentNotOwns<UIClickListener>();
            btn.RemoveClick(action);
        }
        
        public static void OffClick<T>(this Graphic graphic, Action<T> action)
        {
            var btn = graphic.gameObject.AddComponentNotOwns<UIClickListener>();
            btn.RemoveClick(action);
        }

        public static float Normalize(this float num)
        {
            if (num == 0)
                return 0;
            return num / Mathf.Abs(num);
        }
        
        public static int NormalizeToInt(this float num)
        {
            if (num == 0)
                return 0;
            return Mathf.RoundToInt(num / Mathf.Abs(num));
        }

        public static Transform[] Children(this Transform transform)
        {
            var trs = new Transform[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                trs[i] = transform.GetChild(i);
            }

            return trs;
        }
        
        public static bool TryMatchOne<T>(this IEnumerable<T> itor, Func<T, bool> compare, out T result)
        {
            foreach (var item in itor)
            {
                if (compare.Invoke(item))
                {
                    result = item;
                    return true;
                }
            }

            result = default;
            return false;
        }

        public static T FindComponentFromRootObjs<T>(this UnityEngine.SceneManagement.Scene scene, string name = null) where T : MonoBehaviour
        {
            foreach (var o in scene.GetRootGameObjects())
            {
                if (!string.IsNullOrEmpty(name))
                {
                    if (o.name != name)
                        continue;
                }

                if (o.GetComponent<T>() != null)
                    return o.GetComponent<T>();
            }

            return null;
        }

        public static string[] ToStringArray<T>(this IEnumerable<T> itor)
        {
            var list = new List<string>();
            foreach (var obj in itor)
            {
                list.Add(obj.ToString());
            }

            return list.ToArray();
        }

        public static Rect CloseIn(this Rect rect, Rect containIn)
        {
            var x = Mathf.Clamp(rect.center.x, containIn.xMin + rect.width / 2, containIn.xMax - rect.width / 2);
            var y = Mathf.Clamp(rect.center.y, containIn.yMin + rect.height / 2, containIn.yMax - rect.height / 2);
            rect.center = new Vector2(x, y);
            return rect;
        }

        public static int Symbol(this int num)
        {
            return num / Mathf.Abs(num);
        }
        
        public static void ClearChildren(this Transform self)
        {
            for (int i = self.childCount - 1; i >= 0; i--)
            {
                if (Application.isPlaying)
                {
                    UnityEngine.Object.Destroy(self.GetChild(i).gameObject);
                }
                else
                {
                    UnityEngine.Object.DestroyImmediate(self.GetChild(i).gameObject);
                }
            }
        }

        public static string[] ToStringArray(this IEnumerable list)
        {
            var res = new List<string>();
            foreach (object o in list)
            {
                res.Add(o.ToString());
            }

            return res.ToArray();
        }

        public static string FormatStringArr(this IEnumerable<string> list, char symbol = ',')
        {
            var sb = new StringBuilder();
            foreach (string s in list)
            {
                sb.Append($"{s},");
            }

            return sb.ToString().Trim(symbol);
        }

        public static int IndexOf<T>(this IEnumerable<T> list, T obj)
        {
            if (list == null)
                return -1;
            var index = -1;
            foreach (T t in list)
            {
                index++;
                if (t.Equals(obj))
                {
                    break;
                }
            }

            return index;
        }

        public static T[] Concat<T>(this T[] self, params T[][] arrs)
        {
            var length = self.Length;
            foreach (T[] arr in arrs)
            {
                length += arr.Length;
            }

            T[] res = new T[length];
            Array.Copy(self, res, self.Length);
            var pointer = self.Length;
            foreach (var arr in arrs)
            {
                Array.Copy(arr, 0, res, pointer, arr.Length);
                pointer += arr.Length;
            }

            return res;
        }

        public static bool IsDictionary(this Type type)
        {
            return type.IsGenericType &&
                    type.GetGenericTypeDefinition() == typeof (Dictionary<,>);
        }

        public static Rect Resize(this Rect self, float x, float y, float width, float height)
        {
            return new Rect(self.x + x, self.y + y, self.width + width, self.height + height);
        }
        
        public static Rect Resize(this Rect self, Rect offset)
        {
            return new Rect(self.x + offset.x, self.y + offset.y, self.width + offset.width, self.height + offset.height);
        }

        public static Vector3 ToUnityV3(this System.Numerics.Vector3 v3)
        {
            return new Vector3(v3.X, v3.Y, v3.Z);
        }
        
        public static System.Numerics.Vector3 ToNumV3(this Vector3 v3)
        {
            return new System.Numerics.Vector3(v3.x, v3.y, v3.z);
        }

        public static Vector3 X0Z(this Vector2 v2)
        {
            return new Vector3(v2.x, 0, v2.y);
        }

        public static int3 ToInt3(this Vector3 v3)
        {
            v3 *= 1000;
            return new int3((int)v3.x, (int)v3.y, (int)v3.z);
        }
    }
}