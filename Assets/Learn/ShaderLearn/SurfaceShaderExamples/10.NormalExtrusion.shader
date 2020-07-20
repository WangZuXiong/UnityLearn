//使用顶点修改器进行法线挤出
//可以使用“顶点修改器”函数来修改顶点着色器中的传入顶点数据。
//这可用于程序化动画和沿法线挤出等操作。
//表面着色器编译指令 vertex:functionName 将用于此目的，其中的一个函数采用 inout appdata_full 参数。
//以下着色器沿着法线按照材质中指定的量移动顶点：
Shader "Custom/10.NormalExtrusion"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Amount ("Extrusion Amount", Range(-1, 1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }


        CGPROGRAM
     
        #pragma surface surf Lambert vertex:vert

        struct Input
        {
            float2 uv_MainTex;
        };

        float _Amount;

        void vert(inout appdata_full v)
        {
            v.vertex.xyz += v.normal * _Amount * 0.1;
        }

        sampler2D _MainTex;

        void surf (Input IN, inout SurfaceOutput o)
        {
            o.Albedo  = tex2D(_MainTex, IN.uv_MainTex).rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
