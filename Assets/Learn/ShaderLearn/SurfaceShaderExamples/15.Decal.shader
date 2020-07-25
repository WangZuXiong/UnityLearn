//贴花
///贴花通常用于在运行时向材质添加细节（例如，子弹冲击力效果）。
//贴花在延迟渲染中特别有用，因为贴花在照亮之前会改变 G 缓冲区，因此可以节省开销。

//在常规情况下，贴花应该在不透明对象之后渲染，并且不应该是阴影投射物，如以下示例中的 ShaderLab“Tags”中所示。
Shader "SurfaceShaderExamples/15.Decal"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry+1" "ForceNoShadowCasting"="True" }
        LOD 200
        Offset -1,-1

        CGPROGRAM
     
        #pragma surface surf Lambert decal:blend

        struct Input
        {
            float2 uv_MainTex;
        };

        sampler2D _MainTex;

        void surf (Input IN, inout SurfaceOutput o)
        {
           half4 c = tex2D(_MainTex,IN.uv_MainTex);
           o.Albedo = c.rgb;
           o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
