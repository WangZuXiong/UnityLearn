//使用最终颜色修改器自定义雾效
//使用最终颜色修改器（见上文）的常见情况是在前向渲染中实现完全自定义的雾效。
//雾效需要影响最终计算的像素着色器颜色，这正是 finalcolor 修改器的功能。

//下面是一个根据与屏幕中心的距离应用雾效色调的着色器。
//此着色器将顶点修改器与自定义顶点数据 (fog) 和最终颜色修改器组合在一起。用于前向渲染附加通道时，雾效需要淡化为黑色。此示例将解决这一问题并检查是否有 UNITY_PASS_FORWARDADD。
Shader "Custom/13"
{
    Properties
    {
        _FogColor ("Color", Color) = (0.3,0.4,0.7,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
      
        #pragma surface surf Lambert finalcolor:mycolor vertex:myvert

     
        struct Input
        {
            float2 uv_MainTex;
            half fog;
        };

        void myvert(inout appdata_full v, out Input data)
        {
            UNITY_INITIALIZE_OUTPUT(Input, data);//UNITY_INITIALIZE_OUTPUT(type,name)	将给定_类型_的变量_名称_初始化为零。
            float4 hpos = UnityObjectToClipPos(v.vertex);
            hpos.xy /= hpos.w;
            data.fog = min (1, dot(hpos.xy,hpos.xy) * 0.5);
        }

        fixed4 _FogColor;

        void mycolor(Input IN, SurfaceOutput o, inout fixed4 color)
        {
            fixed3 fogColor = _FogColor.rgb;
            #ifdef UNITY_PASS_FORWARDADD
            fogColor = 0;
            #endif
            color.rgb = lerp(color.rgb, fogColor, IN.fog);
        } 

        sampler2D _MainTex;

        void surf (Input IN, inout SurfaceOutput o)
        {
           o.Albedo = tex2D(_MainTex, IN.uv_MainTex);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
