Shader "Custom/DiamondTransition"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Progress("Animation Progress", Range(0.0, 1.0)) = 0.0
		_DiamondSize("Diamond Size", Int) = 10
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass 
        {
            CGPROGRAM
            // compiles vert as vertex function, vertex sets per-vertex variables such as position
            #pragma vertex vert 

            // compiles frag as fragment function, fragment sets per-pixel variables such as color
            #pragma fragment frag

            #include "UnityCG.cginc" // similar to using in C#, includes helper functions 

            // input for vertex shader 
            struct appdata
            {
				float4 vertex : POSITION; // vertex position input
				float2 uv : TEXCOORD0; // texture coordinate input
            }; 

            // input for vertex shader to fragment shader
			struct v2f {
				float2 uv : TEXCOORD0;
			};

            // vertex shader
            v2f vert (appdata i, out float4 outpos : SV_POSITION /*clip space position output*/)
            {
				v2f o;
				o.uv = i.uv;
				outpos = UnityObjectToClipPos(i.vertex);
				return o;
            }

            // animate the shader based on progress
            uniform float _Progress;  

            // size of diamond in pixels
            uniform float _DiamondSize = 10; 

			// color of texture 
			uniform fixed4 _Color; 

            // fragment shader 
			fixed4 frag(v2f i, UNITY_VPOS_TYPE screenPos : VPOS) : SV_Target
            {
                float xFraction = frac(screenPos.x / _DiamondSize); // returns fraction of column pixel is in (positive)
                float yFraction = frac(screenPos.y / _DiamondSize); // returns fraction of row pixel is in (positive) 

				float xDistance = abs(xFraction - 0.5); // manhattan distance of xFraction to the midpoint 
				float yDistance = abs(yFraction - 0.5); // manhattan distance of yFraction to the midpoint 

                // bottom left sum is 0 top right sum is the closest you can get to 2
				// 0 <= (xFraction + yFraction) < 2 always, multiply progress by 2
				// offset total sum by uv.xy for diagonal sweep
				// 0 <= i.uv.xy <= 2, multiply progress by 2, again 
                // exclude pixels with sum greater than progress
                if (xDistance + yDistance + i.uv.x + i.uv.y < _Progress * 4)
                {
					discard; // don't render this pixel 
                }

				// TODO - Pixelate result 

                return _Color; // draw pixel
            }
            ENDCG
        }
    }
}
