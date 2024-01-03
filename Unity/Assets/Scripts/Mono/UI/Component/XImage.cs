using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class XImage : Image
    {
        public enum FlipDirection
        {
            None          = 0,
            Horizontal    = 1,
            Vertical      = 2,
            All           = 3,
        }

        public event Action OnSpriteChanged;
        
        private string _spriteAsset;
        
        private Task<Sprite> _loadTask;
        private bool _gray;
        [SerializeField]
        private FlipDirection _flip;

        private Material _grayMat;
        private static readonly int Property = Shader.PropertyToID("show gray");


        protected override void Awake()
        {
        }

        protected override async void Start()
        {
            if (!string.IsNullOrEmpty(_spriteAsset))
            {
                sprite = await SpritePool.Instance.Fetch(_spriteAsset);
                if (sprite == null)
                {
                    sprite = null;
                }
                else
                {
                    sprite = sprite;
                }
            }
            else
            {
                if (sprite != null)
                    _spriteAsset = sprite.name;
            }
        }
        
        public new Sprite sprite
        {
            get => base.sprite;
            set
            {
                base.sprite = value;
                OnSpriteChanged?.Invoke();
            }
        }
        
        public bool gray
        {
            set
            {
                _gray = value;
                _grayMat ??= new Material(Shader.Find("UI/UIGray"));
                material = _grayMat;
                _grayMat.SetFloat(Property, value ? 0 : 1);
            }
            get => _gray;
        }
        

        public FlipDirection Flip
        {
            set
            {
                _flip = value;
                UpdateGeometry();
            }
            get => _flip;
        }
        public string Skin
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _spriteAsset = value;
                    sprite = null;
                    return;
                }
                if(value == _spriteAsset)
                    return;
                
                if (!string.IsNullOrEmpty(_spriteAsset))
                {
                    SpritePool.Instance.Collect(_spriteAsset, sprite);
                }
                _spriteAsset = value;
                this.SetSprite();
            }
            get => _spriteAsset;
        }

        private async void SetSprite()
        {
            sprite = await SpritePool.Instance.Fetch(_spriteAsset);
        }

        public void UpdateGraph()
        {
            UpdateGeometry();
        }
        
        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            base.OnPopulateMesh(toFill);
 
            Vector2 rectCenter = rectTransform.rect.center;

            int vertCount = toFill.currentVertCount;
            for (int i = 0; i < vertCount; i++)
            {
                UIVertex uiVertex = new UIVertex();
                toFill.PopulateUIVertex(ref uiVertex, i);

                Vector3 pos = uiVertex.position;
                var x = ((int)_flip & (int) FlipDirection.Horizontal) == (int) FlipDirection.Horizontal ? (pos.x + (rectCenter.x - pos.x) * 2) : pos.x;
                var y = ((int)_flip & (int) FlipDirection.Vertical) == (int) FlipDirection.Vertical ? (pos.y + (rectCenter.y - pos.y) * 2) : pos.y;
                uiVertex.position = new Vector3(x, y, pos.z);

                toFill.SetUIVertex(uiVertex, i);
            }
        }

        protected override void OnDestroy()
        {
            if (Application.isPlaying)
            {
                if (!string.IsNullOrEmpty(_spriteAsset) && sprite != null)
                {
                    SpritePool.Instance.Collect(_spriteAsset, sprite);
                }
            }
            base.OnDestroy();
        }
    }
}