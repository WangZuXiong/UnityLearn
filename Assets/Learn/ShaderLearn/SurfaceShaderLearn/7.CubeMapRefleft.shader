Shader "SurfaceShaderLearn/7.CubeMapRefleft"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _CubeMapTex ("Cube Map", Cube) = "white" {}
        _WorldReflPower ("worldRefl power", float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
      
        #pragma surface surf Lambert

        struct Input
        {
            float2 uv_MainTex;
            float3 worldRefl;//float3 worldRefl - 在_表面着色器不写入 o.Normal_ 的情况下，包含世界反射矢量。有关示例，请参阅反光漫射 (Reflect-Diffuse) 着色器。
        };

        sampler2D _MainTex;
        samplerCUBE _CubeMapTex;
        float _WorldReflPower;

        void surf (Input IN, inout SurfaceOutput o)
        {
            o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
            
            o.Emission = texCUBE(_CubeMapTex, IN.worldRefl).rgb * _WorldReflPower;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
