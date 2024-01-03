using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class GameCameraComponent : Entity, IAwake
    {
        public Camera Camera { get; set; }
    }
}