using ET;
using UnityEngine;

namespace ET
{
    //挂载点查找方式
    public enum MountPointFindType
    {
        [Name("空")]
        None,
        [Name("指定节点下", "指定一个节点向下搜索(广度优先)")]
        FindInTarget,
    }
    
    public static class TransformHelper
    {
        public static Transform FindInTarget(Transform target, string path)
        {
            if (path.IndexOf('/') > -1)
            {
                return target.Find(path);
            }

            return target.FindDepth(path);
        }
    }
}