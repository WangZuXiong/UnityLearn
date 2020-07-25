//每顶点计算的自定义数据
//使用顶点修改器函数，还可以在顶点着色器中计算自定义数据，然后将数据按像素传递给表面着色器函数。
//此情况下使用相同的编译指令 vertex:functionName，但该函数应采用两个参数：inout appdata_full 和 out Input。
//您可以在其中填写除内置值以外的任何输入成员。

//注意：以这种方式使用的自定义输入成员不得包含以“uv”开头的名称，否则它们将无法正常工作。
//下面的示例定义了一个在顶点函数中计算的自定义 float3 customColor 成员：
Shader "SurfaceShaderExamples/11.CustomVertexData"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
  

        CGPROGRAM
      
        #pragma surface surf Lambert vertex:vert

        struct Input
        {
            float2 uv_MainTex;
            //float2 uv_Normal;
            float3 customColor;
        };

        void vert(inout appdata_full v,out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);
            //o.customColor = float4(1,0,0,1);
            o.customColor = abs(v.normal);
            //o.customColor.xy = abs(o.uv_Normal);
        }

        sampler2D _MainTex;

        void surf (Input IN, inout SurfaceOutput o)
        {
           o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb * IN.customColor;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
