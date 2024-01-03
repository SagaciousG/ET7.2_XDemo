using UnityEngine;
using UnityEngine.Rendering.Universal;
using ET;

namespace ET
{
    public static class CameraHelper
    {

        public static void AddToStack(this Camera self)
        {
            var additionalCameraData = Init.Instance.MainCamera.GetComponent<UniversalAdditionalCameraData>();
            self.gameObject.AddComponentNotOwns<UniversalAdditionalCameraData>().renderType = CameraRenderType.Overlay;
            additionalCameraData.cameraStack.Add(self);
        }
        
        public static void RemoveFromStack(this Camera self)
        {
            var additionalCameraData = Init.Instance.MainCamera.GetComponent<UniversalAdditionalCameraData>();
            additionalCameraData.cameraStack.Remove(self);
        }
        
        public static void LookAt(this Camera camera, Vector3 lookAt, float distance)
        {
            var forward = camera.transform.forward;
            var target = lookAt + distance * forward * -1;
            camera.transform.position = Vector3.Lerp(camera.transform.position, target, 0.5f);
        }
        
        
    }
}