#ifndef MYENGINE_URP_UI_BLURRED_BASE
#define MYENGINE_URP_UI_BLURRED_BASE

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

CBUFFER_START(UnityPerMaterial)
    half4 _MainTex_ST;
CBUFFER_END

TEXTURE2D(_MainTex);    SAMPLER(sampler_MainTex);
float4 _MainTex_TexelSize;

struct Attributes
{
    float4 positionOS       : POSITION;
    float4 normalOS         : NORMAL;
    half4  color            : COLOR;
    float2 uv               : TEXCOORD0;
};

struct Varyings
{
    float4 positionCS       : SV_POSITION;
    half4  color            : COLOR;
    float2  uv               : TEXCOORD0;
};

Varyings vert(Attributes input)
{
    Varyings output = (Varyings)0;
    float3 positionWS = TransformObjectToWorld(input.positionOS.xyz);
    float4 positionCS = TransformWorldToHClip(positionWS);
    output.positionCS = positionCS;
    output.color = input.color;
    output.uv = input.uv;
    return output;
}

half4 fragHorizontal(Varyings input) : COLOR
{
    float texelSize = _MainTex_TexelSize.x;
    half3 c0 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv - float2(texelSize * 3.23076923, 0.0)).rgb;
    half3 c1 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv - float2(texelSize * 1.38461538, 0.0)).rgb;
    half3 c2 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv                                      ).rgb;
    half3 c3 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv + float2(texelSize * 1.38461538, 0.0)).rgb;
    half3 c4 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv + float2(texelSize * 3.23076923, 0.0)).rgb;

    half3 color = c0 * 0.07027027 + c1 * 0.31621622 + c2 * 0.22702703 + c3 * 0.31621622 + c4 * 0.07027027;
    return half4(color.rgb, 1);
}

half4 fragVertical(Varyings input) : COLOR
{
    float texelSize = _MainTex_TexelSize.y;
    half3 c0 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv - float2(0, texelSize * 3.23076923)).rgb;
    half3 c1 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv - float2(0, texelSize * 1.38461538)).rgb;
    half3 c2 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv                                     ).rgb;
    half3 c3 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv + float2(0, texelSize * 1.38461538)).rgb;
    half3 c4 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv + float2(0, texelSize * 3.23076923)).rgb;

    half3 color = c0 * 0.07027027 + c1 * 0.31621622 + c2 * 0.22702703 + c3 * 0.31621622 + c4 * 0.07027027;
    return half4(color.rgb, 1);
}
#endif