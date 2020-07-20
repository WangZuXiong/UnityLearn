//屏幕空间中的细节纹理
//屏幕空间中的细节纹理对于士兵头部模型没有实际意义，但是在这里可用于说明如何使用内置的 screenPos 输入：
Shader "Custom/6.ScreenPos"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Detail ("Detail", 2D) = "gray" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
       
        CGPROGRAM
        #pragma surface surf Lambert 

        struct Input
        {
            float2 uv_MainTex;
            float4 screenPos;
        };

        sampler2D _MainTex;
        sampler2D _Detail;

        void surf (Input IN, inout SurfaceOutput o)
        {
            o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
            float2 screenUV = IN.screenPos.xy / IN.screenPos.w;
            screenUV *= float2(8,6);
            o.Albedo *= tex2D(_Detail, screenUV).rgb * 2;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
