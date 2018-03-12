// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Shader:      Unlit/Blur (version 1.0)
 */

Shader "MyClasses/Unlit/Blur"
{
    Properties
    {
        _BlurColor("Blur Color", Color) = (0, 0, 0, 0)
        _HorizontalBlur("Horizontal Blur", Range(0, 100)) = 1
        _VerticalBlur("Vertical Blur", Range(0, 100)) = 1
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }

        GrabPass { Tags {"LightMode" = "Always"} }

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
                float4 uv : TEXCOORD0;
            };

            sampler2D _GrabTexture;
            float4 _GrabTexture_TexelSize;
            half4 _BlurColor;
            float _HorizontalBlur;

            v2f vert(appdata i)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(i.vertex);
                #if UNITY_UV_STARTS_AT_TOP
                float scale = -1.0;
                #else
                float scale = 1.0;
                #endif
                o.uv.xy = (float2(o.vertex.x, o.vertex.y * scale) + o.vertex.w) * 0.5;
                o.uv.zw = o.vertex.zw;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                half4 sum = _BlurColor;

                #define GRABPIXEL(weight,kernelx) tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(float4(i.uv.x + _GrabTexture_TexelSize.x * kernelx * _HorizontalBlur, i.uv.y, i.uv.z, i.uv.w))) * weight

                sum += GRABPIXEL(0.05, -4.0);
                sum += GRABPIXEL(0.09, -3.0);
                sum += GRABPIXEL(0.12, -2.0);
                sum += GRABPIXEL(0.15, -1.0);
                sum += GRABPIXEL(0.18, 0.0);
                sum += GRABPIXEL(0.15, +1.0);
                sum += GRABPIXEL(0.12, +2.0);
                sum += GRABPIXEL(0.09, +3.0);
                sum += GRABPIXEL(0.05, +4.0);

                return sum;
            }
            ENDCG
        }

        GrabPass { Tags {"LightMode" = "Always"} }

        // vertical blur
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
                float4 uv : TEXCOORD0;
            };

            sampler2D _GrabTexture;
            float4 _GrabTexture_TexelSize;
            half4 _BlurColor;
            float _VerticalBlur;

            v2f vert(appdata i)
            {
                v2f o;
                
                o.vertex = UnityObjectToClipPos(i.vertex);
                #if UNITY_UV_STARTS_AT_TOP
                float scale = -1.0;
                #else
                float scale = 1.0;
                #endif
                o.uv.xy = (float2(o.vertex.x, o.vertex.y*scale) + o.vertex.w) * 0.5;
                o.uv.zw = o.vertex.zw;
                
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                half4 sum = _BlurColor;

                #define GRABPIXEL(weight, kernely) tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(float4(i.uv.x, i.uv.y + _GrabTexture_TexelSize.y * kernely * _VerticalBlur, i.uv.z, i.uv.w))) * weight

                sum += GRABPIXEL(0.05, -4.0);
                sum += GRABPIXEL(0.09, -3.0);
                sum += GRABPIXEL(0.12, -2.0);
                sum += GRABPIXEL(0.15, -1.0);
                sum += GRABPIXEL(0.18, 0.0);
                sum += GRABPIXEL(0.15, +1.0);
                sum += GRABPIXEL(0.12, +2.0);
                sum += GRABPIXEL(0.09, +3.0);
                sum += GRABPIXEL(0.05, +4.0);

                return sum;
            }
            ENDCG
        }
    }

    Fallback "Sprites/Default"
}