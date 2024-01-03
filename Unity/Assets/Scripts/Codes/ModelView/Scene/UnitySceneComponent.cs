using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class UnitySceneComponent : Entity, IAwake<int>, IDestroy
    {
        public int MapID { get; set; }
        public string SceneName;
        public UnityEngine.SceneManagement.Scene Scene { get; set; }

        public List<GameObject> ActivedRootObjs = new List<GameObject>();
    }
}