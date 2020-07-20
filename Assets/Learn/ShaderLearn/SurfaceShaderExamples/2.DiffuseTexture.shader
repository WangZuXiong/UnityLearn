//纹理
Shader "SurfaceShaderExamples/DiffuseTexture"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
   

        CGPROGRAM

        #pragma surface surf Standard fullforwardshadows

       

        struct Input
        {
            float2 uv_MainTex;
        };
         sampler2D _MainTex;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            o.Albedo = tex2D(_MainTex,IN.uv_MainTex).rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
