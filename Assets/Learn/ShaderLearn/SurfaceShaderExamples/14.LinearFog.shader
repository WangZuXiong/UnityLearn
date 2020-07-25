//线性雾效
Shader "SurfaceShaderExamples/14.LinearFog"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert finalcolor:mycolor vertex:myvert
        #pragma multi_compile_fog

        struct Input
        {
            float2 uv_MainTex;
            half fog;
        };
        sampler2D _MainTex;
        uniform half4 unity_FogStart;
        uniform half4 unity_FogEnd;
  
        void myvert(inout appdata_full v, out Input data)
        {
            UNITY_INITIALIZE_OUTPUT(Input, data);
            float pos = length(UnityObjectToViewPos(v.vertex).xyz);
            float diff = unity_FogEnd.x - unity_FogStart.x;
            float invDiff = 1.0f / diff;
            data.fog = clamp ((unity_FogEnd.x - pos) * invDiff, 0.0, 1.0);//clamp 返回限制在 min 和 max 范围内（含首尾）的 value。
        }

        void mycolor (Input IN, SurfaceOutput o, inout fixed4 color) 
        {
            #ifdef UNITY_PASS_FORWARDADD
            UNITY_APPLY_FOG_COLOR(IN.fog, color, float4(0,0,0,0));
            #else
            UNITY_APPLY_FOG_COLOR(IN.fog, color, unity_FogColor);
            #endif
        }

        void surf (Input IN, inout SurfaceOutput o)
        {
            //采样UV 将_MainTex贴图的信息赋值给IN.uv_MainTex
            half4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
