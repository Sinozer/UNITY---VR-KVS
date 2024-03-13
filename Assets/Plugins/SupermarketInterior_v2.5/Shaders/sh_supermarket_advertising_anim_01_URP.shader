Shader "Supermarket/URP_sh_supermarket_advertising_anim_01_TEST"
{
    Properties
    {
        _MainTex("Main Tex (RGB)", 2D) = "white" {}
        _numU("numU", Float) = 4
        _numV("numV", Float) = 8
        _Speed("Speed", Float) = 0.3
    }
    HLSLINCLUDE
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

    struct Attributes
    {
        float4 positionOS : POSITION;
        float2 uv : TEXCOORD0;
    };

    struct Varyings
    {
        float4 positionHCS : SV_POSITION;
        float2 uv : TEXCOORD0;
    };

    CBUFFER_START(UnityPerMaterial)
        float _numU;
        float _numV;
        float _Speed;
        float4 _MainTex_ST;
    CBUFFER_END

    TEXTURE2D(_MainTex);
    SAMPLER(sampler_MainTex);

    Varyings vert(Attributes IN)
    {
        Varyings OUT;
        OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
        OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
        return OUT;
    }

    float Function_node_1253(float numCol, float Time, float numRow)
    {
        float oneCol = 1/numCol;
        float allSteps = 1/(numCol*numRow);
        int curentCol = floor(Time/allSteps);
        
        if(curentCol >= numCol)
        {
            curentCol = curentCol - ((floor(curentCol / numCol)) * numCol);
        }
        
        return (oneCol * curentCol);
    }

    float Function_node_4725(float numRow, float Time)
    {
        float oneRow = 1/numRow;
        int curentRow = (Time / oneRow);
        return (1 - (oneRow*curentRow)) - numRow;
    }

    half4 frag(Varyings IN) : SV_Target
    {
        float Time = floor(_Time.y * _Speed);
        float x = Time % _numU;
        float y = floor(Time / 4) % _numV;
        float2 uvSize = float2(1.0/_numU, 1.0/_numV);
        float2 uvTopLeftCell = float2(x*(1.0/_numU), 1 - y*(1.0/_numV));
        float2 uvAnimation = uvTopLeftCell + IN.uv * uvSize;
        float4 texColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uvAnimation);
        return texColor;
    }
    ENDHLSL

    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }
        Pass
        {
            Name "Forward"
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            ENDHLSL
        }
    }
    FallBack "Universal Render Pipeline/Lit"
}