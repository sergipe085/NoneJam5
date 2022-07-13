Shader "Unlit/ShaderTest"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BaseColor ("Base Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        HLSLINCLUDE

        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

        CBUFFER_START(UnityPerMaterial)
            float4 _BaseColor;
        CBUFFER_END

        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex)

        struct VertexInput {
            float4 position: POSITION;
            float2 uv: TEXCOORD0;
        };

        struct VertexOutput {
            float4 position: SV_POSITION;
            float2 uv: TEXCOORD0;
        };

        ENDHLSL

        Pass {
            HLSLINCLUDE

            #pragma vertex vert
            #pragma fragment frag

            VertexOutput vert(VertexInput input) {
                VertexOutput output;

                o.position = TransformObjectToHClip(i.position.xyz);
                o.uv = i.uv;
                return o;
            }

            float4 frag(VertexOutput input): SV_TARGET {
                float4 baseTex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                return baseTex * _BaseColor;
            }

            ENDHLSL
        }
    }
}