//法线贴图
Shader "SurfaceShaderExamples/DiffuseBump"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BumpMap ("Bumpmap", 2D) = "bump" {}
        //对于 2D 纹理，默认值为空字符串或内置默认纹理之一：
        //“white”（RGBA：1,1,1,1）、
        //“black”（RGBA：0,0,0,0）、
        //“gray”（RGBA：0.5,0.5,0.5,0.5）、
        //“bump”（RGBA：0.5,0.5,1,0.5）
        //或“red”（RGBA：1,0,0,0）。
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        CGPROGRAM
 
        #pragma surface surf Lambert 

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BumpTex;
        };

        sampler2D _MainTex;
        sampler2D _BumpMap;

        void surf (Input IN, inout SurfaceOutput  o)
        {
           o.Albedo = tex2D(_MainTex,IN.uv_MainTex).rgb;
           o.Normal =  UnpackNormal (tex2D(_BumpMap,IN.uv_BumpTex));
           /*
            新增了支持 BC5 格式的法线贴图类型。

            在此之前，Unity 支持不同压缩格式的 RGB 法线贴图或混合 AG 法线贴图（x 在 Alpha 通道，y 在绿色通道）。
            现在支持 RG 法线贴图（x 在红色通道，y 在绿色通道）。 
            UnpackNormal 着色器函数已升级，现在允许使用 RGB、AG 和 RG 法线贴图而无需添加着色器变体。
            为了能够做到这一点，UnpackNormal 函数依赖于将法线贴图的未使用通道设置为 1。
            即，一个混合 AG 法线贴图必须编码为 (1, y, 1, x)，而 RG 为 (x, y, 0, 1)。Unity 法线贴图编码器强制执行此设置。

            如果用户使用的是未经修改的 Unity，则无需进行升级。
            然而，如果用户创建了自己的法线贴图着色器或自己的编码，他们可能需要考虑将混合 AG 法线贴图编码为 (1, y, 1, x)。
            如果用户在解压缩法线贴图之前在混合 AG 中混合了法线贴图，他们可能需要使用 UnpackNormalDXT5nm 而不是 UnpackNormal。
           */
        }
        ENDCG
    }
    FallBack "Diffuse"
}
