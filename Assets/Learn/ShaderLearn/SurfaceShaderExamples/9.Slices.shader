//通过世界空间位置进行的切片
//下面的着色器通过丢弃几乎水平的环形中的像素来对游戏对象“切片”。为实现此效果，它使用了基于像素世界位置的 Cg/HLSL 函数 clip()。我们将使用内置的表面着色器变量 worldPos。
Shader "SurfaceShaderExamples/9.Slices"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _BumpMap ("Bump Map", 2D) = "nump" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        CGPROGRAM

        #pragma surface surf Lambert

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_Normal;
            float3 worldPos;
        };

        sampler2D _MainTex;
        sampler2D _BumpMap;

        void surf (Input IN, inout SurfaceOutput o)
        {
          clip(frac((IN.worldPos.y + IN.worldPos.z * 0.1) * 10) - 0.5);//frac 压裂
          o.Albedo = tex2D(_MainTex, IN.uv_MainTex);
          o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_Normal));
        }
        ENDCG
    }
    FallBack "Diffuse"
}
