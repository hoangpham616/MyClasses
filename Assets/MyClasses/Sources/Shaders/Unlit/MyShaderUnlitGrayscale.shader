/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Shader:      Unlit/Grayscale (version 1.3)
 */

Shader "MyClasses/Unlit/Grayscale"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    	_Brightness("Color to Brightness (Default #4C961C)", Color) = (0.3, 0.59, 0.11, 1)
    	_EffectAmount("Effect Amount", Range(0, 1)) = 1

		// UI system default properties
		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255
		_ColorMask("Color Mask", Float) = 15
    }

    SubShader
    {
        Tags
	    {
	        "Queue" = "Transparent"
	        "IgnoreProjector" = "True"
	        "RenderType" = "Transparent"
	        "PreviewType" = "Plane"
	        "CanUseSpriteAtlas" = "True"
	    }

        Cull Off
        Lighting Off
        ZWrite Off
        Fog { Mode Off }
        Blend SrcAlpha OneMinusSrcAlpha

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
		        fixed4 color : COLOR;
		    };

		    struct v2f
		    {
		        float4 vertex : SV_POSITION;
		        float2 uv : TEXCOORD0;
		        fixed color : COLOR;
		    };

	    	sampler2D _MainTex;
	    	fixed3 _Brightness;
	    	fixed _EffectAmount;

		    v2f vert(appdata i)
		    {
		        v2f o;
		        o.vertex = UnityObjectToClipPos(i.vertex);
		        o.uv = i.uv;
		        o.color = i.color;
		        return o;
		    }

		    fixed4 frag(v2f i) : SV_Target
		    {
		        fixed4 c = tex2D(_MainTex, i.uv) * i.color;
		        c.rgb = lerp(c.rgb, dot(c.rgb, _Brightness), _EffectAmount);
		        return c;
		    }
	        ENDCG
	    }
    }

    SubShader
    {
        Tags
	    {
	        "Queue" = "Transparent"
	        "IgnoreProjector" = "True"
	        "RenderType" = "Transparent"
	        "PreviewType" = "Plane"
	        "CanUseSpriteAtlas" = "True"
	    }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest[unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
	    {
	        CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"

	        struct appdata
		    {
		        float4 vertex : POSITION;
		        float2 uv : TEXCOORD0;
		        fixed4 color : COLOR;
		    };

		    struct v2f
		    {
		        float4 vertex : SV_POSITION;
		        float2 uv : TEXCOORD0;
		        float4 worldPosition : TEXCOORD1;
		        fixed4 color : COLOR;
		    };

	    	sampler2D _MainTex;
		    fixed4 _ClipRect;
            fixed3 _Brightness;
	    	fixed _EffectAmount;

		    v2f vert(appdata i)
		    {
		        v2f o;
		        o.vertex = UnityObjectToClipPos(i.vertex);
#ifdef UNITY_HALF_TEXEL_OFFSET
		        o.vertex.xy += (_ScreenParams.zw - 1) * fixed2(-1, 1);
#endif
		        o.uv = i.uv;
		        o.worldPosition = i.vertex;
		        o.color = i.color;
		        return o;
		    }

		    fixed4 frag(v2f i) : SV_Target
		    {
		        fixed4 c = tex2D(_MainTex, i.uv) * i.color;
		        c.a *= UnityGet2DClipping(i.worldPosition.xy, _ClipRect);
		        c.rgb = lerp(c, dot(c.rgb, _Brightness), _EffectAmount);
		        return c;
		    }
	        ENDCG
	    }
    }

    Fallback "Sprites/Default"
}
