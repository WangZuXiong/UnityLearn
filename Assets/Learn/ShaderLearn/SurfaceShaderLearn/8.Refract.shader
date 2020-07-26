Shader "SurfShaderLearn/8.Refract"//折射
{
    Properties
    {
        _MainTex ("Main Tex", 2D) = "white" {}
        _CubeMap ("Cube Map", CUBE) = "" {}
        _Rate ("Rate Power", float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        CGPROGRAM

        #pragma surface surf Lambert vertex:myVert 


        struct Input
        {
            float2 uv_MainTex;
            float3 refr;
        };
        float _Rate;

        void myVert(inout appdata_full v, out Input data)
        {
            UNITY_INITIALIZE_OUTPUT(Input, data);
            //由世界变成物体
            //将物体的法线转化为local的法线
            float3 localNormal = normalize(mul(v.normal, (float3x3)unity_WorldToObject));
            //计算入射角 点到摄像机的入射角
            float3 viewDir = -WorldSpaceViewDir(v.vertex);
            //计算折射角
            data.refr = refract(viewDir,localNormal, _Rate);
        }

        sampler2D _MainTex;
        samplerCUBE _CubeMap;

        void surf (Input IN, inout SurfaceOutput o)
        {
           //o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
           o.Albedo = texCUBE(_CubeMap, IN.refr).rgb;         
        }
        ENDCG
    }
    FallBack "Diffuse"
}
