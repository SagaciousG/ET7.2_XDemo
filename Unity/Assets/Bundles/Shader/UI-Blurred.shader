Shader "MyEngine/URP/UI/UI-Blurred"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector" = "true"
            "RenderType" = "Transparent"
            "RenderPipelint" = "UniversalPipeline"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        // #0
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment fragHorizontal
            #include "./Includes/UI-Blurred_Base.hlsl"
            ENDHLSL
        }

        // #1
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment fragVertical
            #include "./Includes/UI-Blurred_Base.hlsl"
            ENDHLSL
        }
    }
}