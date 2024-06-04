Shader "TNTC/DottedLine"
{
    Properties
    {
        _Rep("Repeat Count", float) = 5
        _Spacing("Spacing", float) = 0.5
        _Thickness("Thickness", float) = 0.5
        _Sharpness("Sharpness", Range(0, 100)) = 100
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Packing.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            CBUFFER_START(UnityPerMaterial)
            float _Rep;
            float _Spacing;
            float _Thickness;
            float _Sharpness;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float2 uv           : TEXCOORD0;
                float4 color        : COLOR0;
            };

            struct Varyings
            {
                float2 uv           : TEXCOORD0;
                float4 positionCS   : SV_POSITION;
                float4 color        : COLOR0;
            };

            Varyings vert(Attributes input)
            {
                Varyings output;
                output.positionCS = mul(UNITY_MATRIX_MVP, input.positionOS);
                output.uv = input.uv;
                output.uv.x *= _Rep * _Spacing;
                output.color = input.color;

                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                input.uv.x = fmod(input.uv.x, _Spacing);
                float s = length(input.uv - float2(_Spacing, 1.0f) * 0.5);

                half4 color = input.color;
                color.a *= saturate((_Thickness - s) * _Sharpness);

                return color;
            }
            ENDHLSL
        }
    }
}
