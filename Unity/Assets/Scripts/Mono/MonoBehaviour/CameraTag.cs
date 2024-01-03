using System;
using ET;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraTag : MonoBehaviour
{
    public static Camera MainCamera;
    public CameraRenderType CameraType;
    public int Priority;
    private Vector3 _originPos;
    private Vector3 _originRot;
    private float _originFov;
    private Camera _cam;
    private UniversalAdditionalCameraData _cameraData;
    private UniversalAdditionalCameraData _mainCameraData;
    private void Awake()
    {
        _cam = this.GetComponent<Camera>();
        _originFov = _cam.fieldOfView;
        _originPos = transform.position;
        _originRot = transform.rotation.eulerAngles;
        _cameraData = gameObject.GetComponent<UniversalAdditionalCameraData>();
        _cameraData.renderType = CameraRenderType.Overlay;

        if (gameObject.CompareTag("MainCamera"))
        {
            Debug.LogError($"不允许设置Tag = MainCamera");
            gameObject.tag = "Untagged";
        }
        _mainCameraData = Camera.main.gameObject.GetComponent<UniversalAdditionalCameraData>();
    }

    private void OnEnable()
    {
        if (CameraType == CameraRenderType.Base)
            MainCamera = _cam;
        _mainCameraData.cameraStack.Add(_cam);
        SortStack();
    }

    private void OnDisable()
    {
        if (CameraType == CameraRenderType.Base)
            MainCamera = Init.Instance.MainCamera;
        _mainCameraData.cameraStack.Remove(_cam);
    }
    
    private void SortStack()
    {
        _mainCameraData.cameraStack.Sort((a, b) =>
        {
            var ta = a.GetComponent<CameraTag>();
            var tb = b.GetComponent<CameraTag>();
            var an = ta == null ? -1 : ta.Priority;
            var bn = tb == null ? -1 : tb.Priority;
            return bn - an;
        });
    }

}
