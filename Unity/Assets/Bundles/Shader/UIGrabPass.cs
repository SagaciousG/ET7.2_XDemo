using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGrabPass : MonoBehaviour
{
    [Range(0.1f, 1.0f)]
    public float blurredResolutionScale = 0.3f;
    public RenderTexture grabPassRenderTexture;

    private RawImage m_RawImage;
    private static int s_GrabPassObjectCount = 0;
    private int m_FrameCount = 0;
    private bool m_IsClosed;

    private void OnEnable()
    {
        grabPassRenderTexture = new RenderTexture(640, 480, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
        grabPassRenderTexture.useMipMap = false;
        grabPassRenderTexture.anisoLevel = 0;
        if (s_GrabPassObjectCount == 0)
        {
            MyEngine.Rendering.UIGrabPassFeature.Instance.SetActive(true);
        }
        s_GrabPassObjectCount++;
        m_RawImage = gameObject.GetComponent<RawImage>();
        if (m_RawImage != null)
        {
            m_RawImage.enabled = false;
        }
        m_IsClosed = false;
        m_FrameCount = 0;
    }

    private void Update()
    {
        if (m_FrameCount > 1)
        {
            CloseUIGrabPass();
        }
        else if (m_FrameCount == 1)
        {
            MyEngine.Rendering.UIGrabPassFeature.Instance.settings.renderScale = blurredResolutionScale;
            Graphics.Blit(Shader.GetGlobalTexture("_UIGrabPassCacheRT"), grabPassRenderTexture);
            if (m_RawImage != null)
            {
                m_RawImage.texture = grabPassRenderTexture;
                m_RawImage.enabled = true;
            }
            m_FrameCount++;
        }
        else
        {
            m_FrameCount++;
        }
    }

    private void OnDisable()
    {
        CloseUIGrabPass();
        Destroy(grabPassRenderTexture);
    }

    private void CloseUIGrabPass()
    {
        if (m_IsClosed == false)
        {
            m_IsClosed = true;
            s_GrabPassObjectCount--;
            if (s_GrabPassObjectCount < 0)
            {
                s_GrabPassObjectCount = 0;
                MyEngine.Rendering.UIGrabPassFeature.Instance.SetActive(false);
            }
        }
    }

}