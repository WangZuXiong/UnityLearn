Shader "ShaderLearn/12.AlphaTest_Shader2.0"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
         _AlphaCut ("_AlphaCut", Range(0,1)) = 0.5

    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always
        Blend SrcAlpha OneMinusSrcAlpha // 传统透明度 file:///E:/UnityDocumentation_2019.7.22/Manual/SL-Blend.html

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
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float _AlphaCut;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                // just invert the colors
                col.rgb = 1 - col.rgb;

                //shader 2.0中可以直接取到像素点进行操作
                if(col.a >= _AlphaCut)
                {
                    return col;
                }else
                {
                    return fixed4(0,0,0,0);
                }
                
            }
            ENDCG
        }
    }
}
