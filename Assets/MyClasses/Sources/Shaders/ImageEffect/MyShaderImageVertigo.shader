/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Shader:      Image Effect/Vertigo (version 1.0)
 */

Shader "MyClasses/Image Effect/Vertigo"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Wave ("Wave", Range(0, 1)) = 0.01
        _Intensity ("Intensity", Range(0, 1)) = 0.01
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
            fixed _Wave;
            fixed _Intensity;

            fixed4 frag (v2f i) : SV_Target
            {
                half2 uv_offset = (0, sin((i.vertex.x * _Wave) + (_Time[1] * _Intensity * 100)) / 100);
                return tex2D(_MainTex, i.uv + uv_offset);
            }
            ENDCG
        }
    }
}