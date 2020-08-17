Shader "Hidden/ImageShadow"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MainTex_TexelSize("_MainTex_TexelSize", 2D) = "white" {}


        _LineSize("_LineSize", float) = 0.5
        _Ambient("Ambient", float) = 0.001
        //_UVOffset("UVOffset", float) = 0.001
        _Power("_Power", float) = 1
        _ShadowColor("_ShadowColor", color) = (0,0,0,1)

    }
    SubShader
    {
        // No culling or depth
        Cull Off 
        ZWrite Off 
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

            float _LineSize;

            v2f vert(appdata v)
            {
                v2f o;
                v.vertex.xy *= (_LineSize * 0.1 + 1);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float _Ambient;
            float4 _ShadowColor;
            //float _UVOffset;
            float4 _MainTex_TexelSize;
            float _Power;

            fixed4 frag(v2f i) : SV_Target
            {
                //fixed4 col = tex2D(_MainTex, i.uv + float2(1, 1));

                //附近透明度大于0的像素点
                float count = 0;
                float x0 = i.uv.x - _Ambient;
                float x1 = i.uv.x + _Ambient;

                float y0 = i.uv.y - _Ambient;
                float y1 = i.uv.y + _Ambient;

                [unroll(5)]
                for (float k = x0; k < x1; )
                {
                    [unroll(5)]
                    for (float j = y0; j < y1; )
                    {
                        fixed4 col = tex2D(_MainTex, i.uv + float2(k, j));
                        if (col.a > 0)
                        {
                            count++;
                        }
                        j = j + _MainTex_TexelSize.z;
                    }
                    k = k + _MainTex_TexelSize.w;
                }
                return count * _ShadowColor * _Power;
            }
            ENDCG
        }

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
      
                if (col.a > 0)
                {
                    return col;
                }
                else
                {
                    return fixed4(1,1,1,0);
                }
            }
            ENDCG
        }
    }
}
