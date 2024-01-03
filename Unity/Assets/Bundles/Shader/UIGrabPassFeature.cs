using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace MyEngine.Rendering
{
    public class UIGrabPassFeature : ScriptableRendererFeature
    {
        #region singleton
        private static UIGrabPassFeature m_Instance;
        public static UIGrabPassFeature Instance
        {
            get
            {
                return m_Instance;
            }
        }

        private UIGrabPassFeature() { }
        #endregion

        private static readonly string k_GrabPassTextureName = "_UIGrabPassTexture";

        [Serializable]
        public class UIGrabPassSetting
        {
            [Range(0.1f, 1.0f)]
            public float renderScale = 0.3f;
            public string uiCameraName = "";
            public Material blurredMaterial;
        }


        public UIGrabPassSetting settings = new UIGrabPassSetting();
        private UIGrabPassRenderPass m_GrabPassRenderPass = null;
        private RenderTargetHandle m_GrabPassTexture;
        private Camera UICamera = null;


        public override void Create()
        {
            m_Instance = this;
            m_GrabPassTexture.Init(k_GrabPassTextureName);
            m_GrabPassRenderPass = new UIGrabPassRenderPass(settings);
            m_GrabPassRenderPass.renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if (UICamera == null)
            {
                if (renderingData.cameraData.camera.name == m_GrabPassRenderPass.settings.uiCameraName)
                {
                    UICamera = renderingData.cameraData.camera;
                }
            }
            if (UICamera != null && renderingData.cameraData.camera == UICamera)
            {
                m_GrabPassRenderPass.settings = settings;
                m_GrabPassRenderPass.Setup(renderer.cameraColorTarget, m_GrabPassTexture);
                renderer.EnqueuePass(m_GrabPassRenderPass);
                return;
            }
        }
    }



    public class UIGrabPassRenderPass : ScriptableRenderPass
    {
        public UIGrabPassFeature.UIGrabPassSetting settings;
        private RenderTargetIdentifier m_Source;
        private RenderTargetHandle m_Destination;
        private const string m_ProfileTag = "UI Grab Pass";
        private int m_UIGrabPassVerticalRT = Shader.PropertyToID("_UIGrabPassVerticalBlurredRT");
        private int m_UIGrabPassHorizontalRT = Shader.PropertyToID("_UIGrabPassHorizontalBlurredRT");
        private int m_UIGrabPassCacheRT = Shader.PropertyToID("_UIGrabPassCacheRT");

        public UIGrabPassRenderPass(UIGrabPassFeature.UIGrabPassSetting settings)
        {
            this.settings = settings;
        }

        public void Setup(RenderTargetIdentifier source, RenderTargetHandle destination)
        {
            m_Source = source;
            m_Destination = destination;
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            RenderTextureDescriptor descriptor = cameraTextureDescriptor;
            descriptor.depthBufferBits = 0;
            descriptor.width = (int)(descriptor.width * settings.renderScale);
            descriptor.height = (int)(descriptor.height * settings.renderScale);

            cmd.GetTemporaryRT(m_UIGrabPassCacheRT, descriptor, FilterMode.Bilinear);
            cmd.GetTemporaryRT(m_UIGrabPassVerticalRT, descriptor, FilterMode.Bilinear);
            cmd.GetTemporaryRT(m_UIGrabPassHorizontalRT, descriptor, FilterMode.Bilinear);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get(m_ProfileTag);
            //Lower Resolution
            cmd.Blit(m_Source, m_UIGrabPassVerticalRT);
            cmd.Blit(m_UIGrabPassVerticalRT, m_UIGrabPassHorizontalRT, settings.blurredMaterial, 0);
            cmd.Blit(m_UIGrabPassHorizontalRT, m_UIGrabPassCacheRT, settings.blurredMaterial, 1);
            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();
            CommandBufferPool.Release(cmd);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(m_UIGrabPassVerticalRT);
            cmd.ReleaseTemporaryRT(m_UIGrabPassHorizontalRT);
        }
    }
} 