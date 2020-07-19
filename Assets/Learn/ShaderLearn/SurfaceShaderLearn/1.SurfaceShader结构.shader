Shader "ShaderLearn/SurfaceShader结构"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5//平滑度
        _Metallic ("Metallic", Range(0,1)) = 0.0//金属关泽
        _Occlusion ("Occlusion", Range(0,1)) = 0.0//遮盖

        _SpecularColor ("Specular Color", Color) = (1,1,1,1)//高光的颜色     
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf StandardSpecular fullforwardshadows//# pragma surface surfaceFunction lightModel [optionalparams]

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        half _Occlusion;
        fixed4 _Color;
        fixed4 _SpecularColor;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandardSpecular o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Specular = _SpecularColor;
            // Metallic and smoothness come from slider variables
            //o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Occlusion = _Occlusion;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
/*
    以下是表面着色器的标准输出结构：

    struct SurfaceOutput
    {
        fixed3 Albedo;  // 漫射颜色
        fixed3 Normal;  // 切线空间法线（如果已写入）
        fixed3 Emission;
        half Specular;  // 0..1 范围内的镜面反射能力
        fixed Gloss;    // 镜面反射强度
        fixed Alpha;    // 透明度 Alpha
    };
    在 Unity 5 中，表面着色器还可以使用基于物理的光照模型。内置标准光照模型和标准镜面反射光照模型（见下文）分别使用以下输出结构：

    struct SurfaceOutputStandard
    {
        fixed3 Albedo;      // 基础（漫射或镜面反射）颜色
        fixed3 Normal;      // 切线空间法线（如果已写入）
        half3 Emission;
        half Metallic;      // 0=非金属，1=金属
        half Smoothness;    // 0=粗糙，1=平滑
        half Occlusion;     // 遮挡（默认为 1）
        fixed Alpha;        // 透明度 Alpha
    };
    struct SurfaceOutputStandardSpecular
    {
        fixed3 Albedo;      // 漫射颜色
        fixed3 Specular;    // 镜面反射颜色
        fixed3 Normal;      // 切线空间法线（如果已写入）
        half3 Emission;
        half Smoothness;    // 0=粗糙，1=平滑
        half Occlusion;     // 遮挡（默认为 1）
        fixed Alpha;        // 透明度 Alpha
    };
*/