Shader "Custom/DiamondTransition"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Progress("Animation Progress", Range(0.0, 1.0)) = 0.0
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
                float4 vertex : POSITION; // vertex position 
                float2 uv : TEXCOORD0; // texture coordinate 
            }; 

            // input for vertex shader to fragment shader
            struct v2f
            {
                float4 pos : SV_POSITION;  // clip space position 
                float2 uv : TEXCOORD0; // texture coordinate 
                float4 screenPos : TEXCOOR1; // screen position of pixel 
            }; 

            // vertex shader
            v2f vert (appdata i)
            {
                v2f o; // output 
                o.pos = UnityObjectToClipPos (i.vertex); 
                o.uv = i.uv; 
                o.screenPos = ComputeScreenPos(o.pos); 
                return o; 
            }

            // animate the shader based on progress
            uniform float progress;  

            // size of diamond in pixels
            uniform float diamondSize = 10; 

            // fragment shader 
            float4 frag (v2f i) : SV_TARGET // states that this function returns only one value
            {
                float xFraction = frac(i.screenPos.x / diamondSize); // returns fraction of column pixel is in (positive)
                float yFraction = frac(i.screenPos.y / diamondSize); // returns fraction of row pixel is in (positive)

                // 0 <= (xFraction + yFraction) < 2 always
                // bottom left is 0 top right is the closest you can get to 2
                // if pixel has reached progress discard it 
                /*if (xFraction + yFraction > progress * 2)
                {
                    discard; 
                }*/

                return 0; // draw if not discarded
            }
            ENDCG
        }
    }
}

// read : https://docs.unity3d.com/Manual/SL-ShaderSemantics.html?_ga=2.251543426.541076280.1644425229-537311678.1631294492
