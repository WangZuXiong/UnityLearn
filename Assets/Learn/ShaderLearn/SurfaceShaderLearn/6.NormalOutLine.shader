Shader "SurfaceShaderLearn/6.NormalOutLine"//利用法线实现边缘发光的效果
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _NormalTex ("Normal Texture", 2D) = "dump" {}
        _LinePower ("强度", float) = 1
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
            float2 uv_NormalTex;
            float3 viewDir;//包含视图方向，用于计算视差效果、边缘光照等等。
        };

    
        fixed4 _Color;
        sampler2D _MainTex;
        sampler2D _NormalTex;
        float _LinePower;

        void surf (Input IN, inout SurfaceOutput o)
        {
            o.Albedo =  tex2D (_MainTex, IN.uv_MainTex).rgb;
            float3 tempNormal = UnpackNormal(tex2D (_NormalTex, IN.uv_NormalTex));
            o.Normal = tempNormal;
            //o.Emission = (1 - dot(IN.viewDir,clamp(tempNormal,0,1))) * _LinePower * _Color;//1-x 取反

            o.Emission =  pow((1 - dot(IN.viewDir,clamp(tempNormal,0,1))),_LinePower) * _Color;

        }

        ENDCG
    }
    FallBack "Diffuse"
}