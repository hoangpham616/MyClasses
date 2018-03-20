/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Shader:      Unlit/AlphaMask (version 1.1)
 */

Shader "MyClasses/Unlit/AlphaMask"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1, 1, 1, 1)
		_AlphaMask("Alpha Mask", 2D) = "white" {}
		[MaterialToggle] _FlipAlphaMask("Flip Alpha Mask", int) = 0
		[HideIninspector] _AlphaUV("Alpha UV", Vector) = (1, 1, 0, 0)
		[HideIninspector] _Min("Min", Vector) = (0, 0, 0, 0)
		[HideIninspector] _Max("Max", Vector) = (1, 1, 0, 0)
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

		LOD 0

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

			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
				half2 texcoord : TEXCOORD0;
				float4 worldPosition : TEXCOORD1;
			};

			fixed4 _Color;
			fixed4 _TextureSampleAdd;
            float4 _ClipRect;
			sampler2D _MainTex;
			sampler2D _AlphaMask;
            sampler2D _Mask;
            half4 _MaskUv;
			float2 _AlphaUV;
			float2 _Min;
			float2 _Max;
			int _FlipAlphaMask = 0;

			v2f vert(appdata i)
			{
				v2f o;

				o.worldPosition = i.vertex;
				o.vertex = UnityObjectToClipPos(o.worldPosition);
				o.texcoord = i.texcoord;
				#if UNITY_HALF_TEXEL_OFFSET
				o.vertex.xy += (_ScreenParams.zw - 1.0) * float2(-1, 1);
				#endif
				o.color = i.color * _Color;

				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				half4 color = (tex2D(_MainTex, i.texcoord) + _TextureSampleAdd) * i.color;
                
                float a = tex2D(_AlphaMask, (i.texcoord - _Min) / _AlphaUV).a;
                if (_FlipAlphaMask > 0)
                {
                    a = 1 - a;
                }
                color.a *= a;

				return color;
			}
			ENDCG
		}
	}
}