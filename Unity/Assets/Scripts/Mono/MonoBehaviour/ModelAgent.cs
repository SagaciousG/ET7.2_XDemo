using System;
using System.Collections.Generic;
using System.Threading.Tasks;
  using UnityEngine;

namespace ET
{
    public class ModelAgent : MonoBehaviour
    {
        public event Action OnComplete;

        private Material[] _materials;

        private float _duration;
        private bool _start;
        private float _cur;
        private bool _running;

        private bool _initialized;
        private bool _requireInit;
        
        private float _curAlpha = 1;
        private float _startAlpha = 1;
        private float _endAlpha = 1;

        private Color _color;

        private Dictionary<int, float> _matAlpha = new Dictionary<int, float>();
        private static readonly int MainColor = Shader.PropertyToID("_MainColor");

        public float alpha
        {
            set
            {
                if (Math.Abs(_curAlpha - value) < 0.001f)
                {
                    return;
                }

                _curAlpha = value;
                if (!_initialized)
                {
                    _requireInit = true;
                    return;
                }
                RefreshAlpha();
            }
            get => _curAlpha;
        }

        public Color color
        {
            set
            {
                if (_color == value)
                    return;

                _color = value;
                _curAlpha = value.a;
                if (!_initialized)
                {
                    _requireInit = true;
                    return;
                }
                foreach (var mat in _materials)
                {
                    mat.color = value;
                }
            }
            get => _color;
        }
        void Start()
        {
            var renderers = GetComponentsInChildren<Renderer>();
            var list = new List<Material>();
            foreach (var r in renderers)
            {
                list.AddRange(r.materials);
            }

            _materials = list.ToArray();

            foreach (var mat in _materials)
            {
                _matAlpha[mat.GetHashCode()] = mat.color.a;
            }

            _initialized = true;
            if (_requireInit)
            {
                RefreshAlpha();
            }
        }

        void Update()
        {
            if (_start && _running)
            {
                alpha = _startAlpha + (_endAlpha - _startAlpha) * (_cur / _duration);

                _cur += Time.deltaTime;
                if (_cur >= _duration)
                {
                    _start = false;
                    _running = false;
                    OnComplete?.Invoke();
                }
            }
        }

        private void RefreshAlpha()
        {
            foreach (var mat in _materials)
            {
                var c = mat.color;
                c.a = _curAlpha;
                mat.color = c;
            }
        }
        public void ResetAlpha()
        {
            if (_materials != null)
            {
                foreach (var mat in _materials)
                {
                    var c = mat.color;
                    c.a = _matAlpha[mat.GetHashCode()];
                    mat.color = c;
                }
            }

            _cur = 0;
        }

        public void DOAlpha(float a, float time)
        {
            _startAlpha = _curAlpha;
            _endAlpha = a;
            _duration = time;
            _running = true;
            _cur = 0;
            _start = true;
        }

        public void FadeIn(float time, bool reset = true)
        {
            if (reset)
            {
                ResetAlpha();
            }

            _duration = time;
            _start = true;
            _running = true;
            _cur = 0;
            _startAlpha = 0;
            _endAlpha = 1;
        }

        public void FadeOut(float time, bool reset = true)
        {
            if (reset)
            {
                ResetAlpha();
            }

            _duration = time;
            _start = true;
            _running = true;
            _cur = 0;
            _startAlpha = 1;
            _endAlpha = 0;
        }

        public void Stop()
        {
            if (_start)
            {
                OnComplete?.Invoke();
            }

            _start = false;
            _running = false;
        }

        public void Pause()
        {
            _running = false;
        }

        public void Resume()
        {
            _running = true;
        }

        private void OnDestroy()
        {
            ResetAlpha();
        }

    }

}
