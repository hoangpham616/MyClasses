/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Shader:      Image Effect/Color Drift Glitch (version 1.0)
 * Based on KinoGlitch of Keijiro Takahashi
 */

Shader "MyClasses/Image Effect/Color Drift Glitch"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _Amplitude ("Amplitude", Range(0, 1)) = 0.02
        _Intensity ("Intensity", Range(0, 1)) = 0.1
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
            fixed _Amplitude;
            fixed _Intensity;

            fixed4 frag(v2f i) : SV_Target
            {
                float u = i.uv.x;
                float v = i.uv.y;
                float drift = sin(_Time.y * _Intensity * 1000) * _Amplitude * 0.5;
                half4 src1 = tex2D(_MainTex, frac(float2(u, v)));
                half4 src2 = tex2D(_MainTex, frac(float2(u + drift, v)));
                return fixed4(src1.r, src2.g, src1.b, 1);
            }
            ENDCG
        }
    }
}