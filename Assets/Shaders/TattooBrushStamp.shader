Shader "Tattoo/Brush Stamp"
{
    Properties
    {
        _Hardness ("Hardness", Range(0, 1)) = 0.95
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent"
        }

        Pass
        {
            Cull Off
            ZWrite Off
            ZTest Always

            Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha

            CGPROGRAM

            #pragma vertex Vert
            #pragma fragment Frag

            #include "UnityCG.cginc"

            struct AppData
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct VertexToFragment
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            float _Hardness;

            VertexToFragment Vert(AppData input)
            {
                VertexToFragment output;
                output.vertex = UnityObjectToClipPos(input.vertex);
                output.uv = input.uv;
                output.color = input.color;
                return output;
            }

            fixed4 Frag(VertexToFragment input) : SV_Target
            {
                float2 centeredUv = input.uv * 2.0 - 1.0;
                float distanceFromCenter = length(centeredUv);

                float edgeStart = min(
                    saturate(_Hardness),
                    0.9999
                );

                float brushMask = 1.0 - smoothstep(
                    edgeStart,
                    1.0,
                    distanceFromCenter
                );

                return fixed4(
                    input.color.rgb,
                    input.color.a * brushMask
                );
            }

            ENDCG
        }
    }
}
