using System;
using ET;
using UnityEngine;
using UnityEngine.UI;
using YooAsset;

namespace ET
{
    [RequireComponent(typeof(RawImage))]
    public class XModelImage : MonoBehaviour
    {
        public bool SetClearWhenNoImage = true;
        private static Transform RawImageRoot;
        private static int Hole = 0;
        
        private Camera _renderCamera;
        private RawImage _rawImage;
        private RenderTexture _textureOutput;
        private RectTransform _rectTransform;
        private GameObject _currentObj;
        private int _index;
        private string _loadedPath;

        private ETTask _waitForStart;
        private bool _isStarted;

        private Color color
        {
            get
            {
                return _rawImage.color;
            }
            set
            {
                _rawImage.color = value;
                _defaultColor = value;
            }
        }

        private Color _defaultColor;
        
        private void Awake()
        {
            for (int i = 0; i < 32; i++)
            {
                if ((1 << i & Hole) == 0)
                {
                    this._index = i;
                    Hole = (1 << i) | Hole;
                    break;
                }
            }

            if (RawImageRoot == null)
            {
                RawImageRoot = new GameObject("[RawImageRoot]").transform;
                RawImageRoot.transform.position = Vector3.up * 1000;
                var component = new GameObject("Light").AddComponent<Light>();
                component.transform.SetParent(RawImageRoot.transform, false);
                component.transform.rotation = Quaternion.Euler(30, 0, 0);
                DontDestroyOnLoad(RawImageRoot.gameObject);
            }

            this._rectTransform = (RectTransform)this.transform;
            _waitForStart = ETTask.Create();
        }

        private void OnEnable()
        {
            if (this._renderCamera != null)
            {
                this._renderCamera.gameObject.SetActive(true);
            }
        }

        private void OnDisable()
        {
            if (this._renderCamera != null)
            {
                this._renderCamera.gameObject.SetActive(false);
            }
        }

        private void Start()
        {
            var rect = this._rectTransform.rect;
            this._textureOutput = new RenderTexture((int) rect.width, (int) rect.height, 24);
            this._rawImage = this.GetComponent<RawImage>();
            this._rawImage.texture = this._textureOutput;
            _defaultColor = _rawImage.color;
            this.CreateCamera();
            _isStarted = true;
            _waitForStart?.SetResult();
            _waitForStart = null;
        }

        private void CreateCamera()
        {
            this._renderCamera = new GameObject($"{this.name}Camera").AddComponent<Camera>();
            this._renderCamera.cullingMask = LayerMask.NameToLayer("ModelImage");
            this._renderCamera.transform.SetParent(RawImageRoot, false);
            this._renderCamera.clearFlags = CameraClearFlags.SolidColor;
            this._renderCamera.targetTexture = this._textureOutput;
            this._renderCamera.transform.localPosition = new Vector3(10 * this._index, 0, 0);
        }

        public void Resize(int width, int height)
        {
            this._textureOutput = new RenderTexture(width, height, 24);
            this._renderCamera.targetTexture = this._textureOutput;
            this._rawImage.texture = this._textureOutput;
        }
        
        public async ETTask<GameObject> Load(string prefabName)
        {
            return await Load(prefabName, new Vector3(0, 0, 5));
        }

        public async ETTask<GameObject> Load(string prefabName, Vector3 offset)
        {
            if (this._loadedPath == prefabName)
                return this._currentObj;
            var obj = await YooAssetHelper.LoadGameObjectAsync(prefabName, Package.Art);
            if (!_isStarted)
            {
                await _waitForStart;
            }
            
            obj.transform.SetParent(this._renderCamera.transform, false);
            if (this._currentObj != null)
            {
                Destroy(this._currentObj);
            }
            if (SetClearWhenNoImage)
            {
                color = obj == null ? Color.clear : Color.white;
            }

            if (obj == null)
                return null;
            obj.layer = LayerMask.NameToLayer("ModelImage");
            this._currentObj = obj;
            obj.transform.localPosition = offset;
            return obj;
        }

        private void OnDestroy()
        {
            if (this._currentObj != null)
            {
                Destroy(this._currentObj);
            }

            if (this._renderCamera != null)
            {
                Destroy(this._renderCamera.gameObject);
            }

            Hole = ~(1 << this._index) & Hole;
        }
    }
}