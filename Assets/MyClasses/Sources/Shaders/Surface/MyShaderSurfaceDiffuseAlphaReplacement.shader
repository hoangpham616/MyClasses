// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Shader:      Surface/Diffuse (Alpha Replacement) (version 1.0)
 */

Shader "MyClasses/Surface/Diffuse (Alpha Replacement)" 
{
    Properties 
    {
        _MainTex ("Albedo (RGB) and Alpha (A)", 2D) = "white" {}
        _ColorRGB ("Albedo Color", Color) = (1, 1, 1, 1)
        _ColorA ("Alpha Color", Color) = (0, 0, 0, 1)
        _Glossiness ("Smoothness", Range(0, 1)) = 0.5
        _Metallic ("Metallic", Range(0, 1)) = 0.0
    }

    SubShader 
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
        
        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        struct Input 
        {
            float2 uv_MainTex;
        };
        
        sampler2D _MainTex;
        fixed4 _ColorRGB;
        fixed4 _ColorA;
        fixed _Glossiness;
        fixed _Metallic;

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf(Input i, inout SurfaceOutputStandard o) 
        {
            fixed4 c = tex2D(_MainTex, i.uv_MainTex) * _ColorRGB;
            o.Albedo = lerp(_ColorA.rgb, c.rgb, c.a);
            o.Alpha = c.a;
            o.Smoothness = _Glossiness;
            o.Metallic = _Metallic;
        }
        ENDCG
    }

    FallBack "Diffuse"
}