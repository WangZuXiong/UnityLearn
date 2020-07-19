Shader "ShaderLearn/15.Blend"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        //Cull Off ZWrite Off ZTest Always
        //Blend SrcAlpha OneMinusSrcAlpha 

        //BlendOp Min
        BlendOp Max
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
                // col.rgb = 1 - col.rgb;
                return col;
            }
            ENDCG
        }
    }
}
/*
    Blend SrcAlpha OneMinusSrcAlpha // 传统透明度
    Blend One OneMinusSrcAlpha // 预乘透明度
    Blend One One // 加法
    Blend OneMinusDstColor One // 软加法
    Blend DstColor Zero // 乘法
    Blend DstColor SrcColor // 2x 乘法


    BlendOp 指定将要渲染的像素和GBuffer里面的像素进行逻辑运算
    当这个指令存在，Blend 这个指令就会被忽略

    BlendOp Op：不将混合颜色相加，而是对它们执行不同的操作。
    BlendOp OpColor, OpAlpha：同上，但是对颜色 (RGB) 通道和 Alpha (A) 通道使用不同的混合操作。
*/
