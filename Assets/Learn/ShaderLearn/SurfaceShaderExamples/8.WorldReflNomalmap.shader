//如果您想做一些受法线贴图影响的反射，需要稍微复杂一些：需要将 INTERNAL_DATA 添加到 Input 结构，并使用 WorldReflectionVector 函数在写入法线输出后计算每像素反射矢量。
Shader "Custom/8.WorldReflNomalmap"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _BumpMap ("Bump Map", 2D) = "bump" {}
        _Cube ("Cube Map", CUBE) = "" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        CGPROGRAM
    
        #pragma surface surf Lambert

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BumpMap;
            float3 worldRefl;
            INTERNAL_DATA
        };

        sampler2D _MainTex;
        sampler2D _BumpMap;
        samplerCUBE _Cube;

        void surf (Input IN, inout SurfaceOutput o)
        {
           o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb * 0.5;
           o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
           o.Emission = texCUBE(_Cube, WorldReflectionVector(IN, o.Normal)).rgb;//Emission 射出
        }
        ENDCG
    }
    FallBack "Diffuse"
}
