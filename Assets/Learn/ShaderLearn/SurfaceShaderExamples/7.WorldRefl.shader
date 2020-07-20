//立方体贴图反射
//下面的着色器将使用内置 worldRefl 输入来进行立方体贴图反射。它与内置的反射/漫射着色器非常类似：
Shader "Custom/7.WorldRefl"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Cube ("Cubemap", CUBE) = "" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
   

        CGPROGRAM
        #pragma surface surf Lambert

        struct Input
        {
            float2 uv_MainTex;
            //float3 worldRefl - 在_表面着色器不写入 o.Normal_ 的情况下，包含世界反射矢量。有关示例，请参阅反光漫射 (Reflect-Diffuse) 着色器。
            //float3 worldRefl; INTERNAL_DATA - 在_表面着色器写入 o.Normal_ 的情况下，包含世界反射矢量。要获得基于每像素法线贴图的反射矢量，请使用 WorldReflectionVector (IN,
            float3 worldRefl;
        };

        
        sampler2D _MainTex;
        samplerCUBE _Cube;

        void surf (Input IN, inout SurfaceOutput o)
        {
            o.Albedo = tex2D(_MainTex,IN.uv_MainTex).rgb * 0.5f;
            o.Emission = texCUBE(_Cube,IN.worldRefl).rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
