/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Shader:      Surface/Simple Water (version 1.0)
 */

Shader "MyClasses/Surface/Simple Water" 
{
    Properties 
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Color ("Color", Color) = (1, 1, 1, 1)
        _HorizontalSpeed ("Horizontal Speed", Range(-10, 10)) = 0.5
        _VerticalSpeed ("Vertical Speed", Range(-10, 10)) = 0.5
    }

    SubShader 
    {
        Tags { "RenderType"="Transparent" }
        LOD 200
        
        CGPROGRAM
        #pragma surface surf Lambert
        #pragma target 3.0

        struct Input 
        {
            float2 uv_MainTex;
        };

        sampler2D _MainTex;
        fixed4 _Color;
        half _HorizontalSpeed;
        half _VerticalSpeed;
        
        void surf (Input i, inout SurfaceOutput o) 
        {
            half move_x = _HorizontalSpeed * _Time;
            half move_y = _VerticalSpeed * _Time;
            half2 move_offset = i.uv_MainTex + half2(move_x, move_y);

            half4 c = tex2D(_MainTex, move_offset);
            o.Albedo = c.rgb * _Color.rgb;
            o.Alpha = _Color.a;
        }
        ENDCG
    }

    FallBack "Diffuse"
}