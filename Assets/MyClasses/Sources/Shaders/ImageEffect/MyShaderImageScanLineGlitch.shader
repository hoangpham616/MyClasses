/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Shader:      Image Effect/Scan Line Glitch (version 1.0)
 * Based on KinoGlitch of Keijiro Takahashi
 */

Shader "MyClasses/Image Effect/Scan Line Glitch"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _Intensity ("Intensity", Range(0, 1)) = 0.5
    }

    SubShader
    {
        Cull Off
        ZWrite Off
        ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert (appdata i)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(i.vertex);
                o.uv = i.uv;
                return o;
            }
            
            sampler2D _MainTex;
            fixed _Intensity;

            float nrand(float x, float y)
            {
                return frac(sin(dot(float2(x, y), float2(12.9898, 78.233))) * 43758.5453);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float u = i.uv.x;
                float v = i.uv.y;
                float thresh = 1 - _Intensity * 1.2;
                float disp = 0.002f + pow(_Intensity, 3) * 0.05f;
                float jitter = nrand(v, _Time.x) * 2 - 1;
                jitter *= step(thresh, abs(jitter)) * disp;
                return tex2D(_MainTex, frac(float2(u + jitter, v)));
            }
            ENDCG
        }
    }
}