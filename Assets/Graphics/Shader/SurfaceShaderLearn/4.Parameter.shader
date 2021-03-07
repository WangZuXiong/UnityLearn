Shader "SurfaceShaderLearn/4.Parameter"//自定义Input参数
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM

         #pragma surface surf Lambert vertex:myVert



        sampler2D _MainTex;
        fixed4 _Color;

        struct Input
        {
            float2 uv_MainTex;
            float3 myColor;
        };

        void myVert (inout appdata_base v,out Input o)
        {
            //UNITY_INITIALIZE_OUTPUT(type,name)	将给定_类型_的变量_名称_初始化为零。
            UNITY_INITIALIZE_OUTPUT(Input,o);//顶点着色器的运算次数远小于片段着色器，故将初始化放到顶点着色器里
            //o.myColor = _Color;
            o.myColor = _Color.rgb * abs(v.normal);//法线取值范围是[-1,1]，abs是求绝对值
        }

        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) ;
            //o.Albedo = c.rgb *  IN.myColor.rgb;
            o.Albedo = IN.myColor.rgb * 0.5;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
