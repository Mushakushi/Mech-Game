// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/ColorSwap"
{
	Properties
	{
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_ReplaceColor("Replace Color", Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			// compiles vert as vertex function, vertex sets per-vertex variables such as position
			#pragma vertex vert 

			// compiles frag as fragment function, fragment sets per-pixel variables such as color
			#pragma fragment frag

			// input for vertex shader 
			struct appdata
			{
				float4 vertex : POSITION; // vertex position input
				float2 uv : TEXCOORD0; // texture coordinate input
			};

			// input for vertex shader to fragment shader
			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION; // clip space position
			};

			// vertex shader
			v2f vert(appdata i)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(i.vertex);
				o.uv = i.uv;
				return o;
			}

			// texture sampled
			sampler2D _MainTex;
			fixed4 _ReplaceColor; 

			// fragment shader 
			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);

				if (col.a == 0) discard;

				// if length (magnitude) delta between two colors is less than value (they are "equal")
				if (length(abs(_ReplaceColor.rgb - col.rgb)) < 0.1) {
					col.gb = sin(_Time * 0.1f); // replace this pixel
				}
					

				return col;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}