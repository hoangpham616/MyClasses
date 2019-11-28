/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Shader:      Stencil/Mask (Always One) (version 1.0)
 */

Shader "MyClasses/Stencil/Mask (Always One)"
{
	SubShader
	{
		Tags { "RenderType" = "Opaque" "Queue" = "Geometry-1" }
		ColorMask 0
		Cull back
		Lighting Off
		ZWrite off
		ZTest always

		Stencil
		{
			Ref 1
			Comp always
			Pass replace
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				return o;
			}

			half4 frag(v2f i) : COLOR
			{
				return half4(1,1,1,1);
			}

			ENDCG
		}
	}
}