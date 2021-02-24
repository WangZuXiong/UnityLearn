Shader "Hidden/ShaderVariants"
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
            #pragma multi_compile A B C
            #pragma multi_compile D E

            /*
                定义方式中值得注意的是，#pragma shader_feature A其实是 #pragma shader_feature _ A的简写，下划线表示未定义宏(nokeyword)。
                因此此时shader其实对应了两个变体，一个是nokeyword，一个是定义了宏A的。
                而#pragma multi_compile A并不存在简写这一说，所以shader此时只对应A这个变体。
                若要表示未定义任何变体，则应写为 #pragma multi_compile __ A。
            */

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
                col.rgb = 1 - col.rgb;
                return col;
            }
            ENDCG
        }
    }
}
