using System;
using UnityEngine;

namespace ET
{
    [RequireComponent(typeof(XImage))]
    public class UIImageBinding : MonoBehaviour
    {
        public XImage Follow;

        private XImage _self;
        private void Awake()
        {
            _self = GetComponent<XImage>();
            Follow.OnSpriteChanged += OnSpriteChanged;
        }

        private void OnDestroy()
        {
            Follow.OnSpriteChanged -= OnSpriteChanged;
        }

        private void OnSpriteChanged()
        {
            _self.sprite = Follow.sprite;
        }
    }
}