Shader "ShaderLearn/9.OutLineDemo"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _LineSize ("Line Size", float) = 0.1
        _LineColor ("Line Color", Color) = (1,0,0,1)
    }
    SubShader
    {
        // No culling or depth
        //Cull Off ZWrite Off ZTest Always

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

            float _LineSize;
          
            v2f vert (appdata v)
            {
                v2f o;
                //放大
                v.vertex.xy *= (_LineSize * 0.1 + 1);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float4 _LineColor;

            fixed4 frag (v2f i) : SV_Target
            {
                //fixed4 col = tex2D(_MainTex, i.uv);
                // just invert the colors
                //col.rgb = 1 - col.rgb;
                //return col;
                return fixed4(_LineColor.r,_LineColor.g,_LineColor.b,_LineColor.a);
            }
            ENDCG
        }
        //打开深度测试
        ZTest Always

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

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                // just invert the colors
                //col.rgb = 1 - col.rgb;
                return col;
                //return fixed4(0,1,0,1);
                
            }
            ENDCG
        }
    }
}


/*
    方案：
    当一个物体渲染两次，
    其中一个大一个小
    先渲染的放大一点返回纯色，后渲染的返回原来的颜色
*/