Shader "ShaderLearn/14.DepthTest"//深度测试
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        //ZWrite On //是否将要渲染的物体写入深度缓冲区（GBuffer）
        //ZWrite Off //不往深度缓冲区写入深度值
        //控制是否将此对象的像素写入深度缓冲区（默认值为 _On_）。如果要绘制实体对象，请将其保留为 on。如果要绘制半透明效果，请切换到 ZWrite Off


        //ZTest Less //当前值要小于深度缓冲区里面的值才能写入深度缓冲区
        //ZTest Always //总是能通过
        //ZTest Less | Greater | LEqual | GEqual | Equal | NotEqual | Always


        //ZWrite On    ZTest通过     该像素的深度能写入深度缓存     该颜色会写入颜色缓存
        //ZWrite On    ZTest不通过   该像素的深度不能写入深度缓存   该颜色不会写入颜色缓存
        //ZWrite Off   ZTest通过     该像素的深度不能写入深度缓存   该颜色会写入颜色缓存
        //ZWrite Off   ZTest不通过   该像素的深度不能写入深度缓存   该颜色不会写入颜色缓存
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
                col.rgb = 1 - col.rgb;
                return col;
            }
            ENDCG
        }
    }
}
