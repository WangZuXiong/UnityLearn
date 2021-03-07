Shader "SurfShaderLearn/9.Fresnel"//菲尼尔
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _EatRation ("Ration", float) = 1.2 //折射率

        _FresnelBias ("菲尼尔的偏移", float) = 1
        _FresnelScale ("菲尼尔的缩放", float) = 1
        _FresnelPower ("菲尼尔的指数", float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
   
        #pragma surface surf Lambert vertex:MyVertex

        struct Input
        {
            float2 uv_MainTex;
            float3 worldRefl;
            float3 refract;
            float refractFact;
        };

        
        fixed4 _Color;
        float _FresnelBias;
        float _FresnelScale;
        float _FresnelPower;
        float _EatRation;


        void MyVertex (inout appdata_full v, out Input data)
        {
            UNITY_INITIALIZE_OUTPUT(Input, data);

            float localNormal = normalize(mul(v.normal, (float3x3)unity_WorldToObject));
     
            float3 viewDir = -WorldSpaceViewDir(v.vertex);

            data.refract = refract(viewDir, localNormal, _EatRation);

            data.refractFact = _FresnelBias + _FresnelScale * pow(1 + dot(viewDir, localNormal), _FresnelPower);
        }

        sampler2D _MainTex;

        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 cflect = tex2D (_MainTex, IN.worldRefl);//反射
            fixed4 cfract = tex2D (_MainTex, IN.refract);//折射

            o.Albedo = IN.refractFact * cflect + (1 - IN.refractFact) * cfract.rgb;

            o.Alpha = cfract.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
