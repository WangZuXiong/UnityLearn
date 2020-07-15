Shader "ShaderLearn/7.RiverDemo"//片段着色器的uv偏移
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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

            fixed4 frag (v2f i) : SV_Target
            {
                //fixed4 col = tex2D(_MainTex, i.uv);//i.uv 表示贴图的uv
                //fixed4 col = tex2D(_MainTex, (0.5,0.5));//这取的是贴图的中心点

                //_Time		自关卡加载以来的时间 (t/20, t, t*2, t*3)，用于将着色器中的内容动画化。
                float2 tempUv = i.uv;
                tempUv.x += _Time.y;
                tempUv.y += _Time.y;

                fixed4 col = tex2D(_MainTex, tempUv);
                // just invert the colors
                //col.rgb = 1 - col.rgb;
                return col;
            }
            ENDCG
        }
    }
}
