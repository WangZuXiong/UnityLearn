Shader "ShaderLearn/10.GaussBlurDemo"//高斯模糊
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Ambient ("Ambient", float) = 0.01
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

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
            float _Ambient;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                fixed4 colUp = tex2D(_MainTex, i.uv + float2(0,_Ambient));

                fixed4 colDowm = tex2D(_MainTex, i.uv + float2(0,-_Ambient));

                fixed4 colLeft = tex2D(_MainTex, i.uv + float2(-_Ambient,0));

                fixed4 colRight = tex2D(_MainTex, i.uv + float2(_Ambient,0));

                col = (col + colUp + colDowm + colLeft + colRight) / 5.0;
               
                return col;
            }
            ENDCG
        }
    }
}
