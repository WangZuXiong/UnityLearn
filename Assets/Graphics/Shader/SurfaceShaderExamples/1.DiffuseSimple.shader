//简单着色器
//下面的着色器将表面颜色设置为“白色”。它使用内置的兰伯特（漫射）光照模型。
Shader "SurfaceShaderExamples/DiffuseSimple" 
{
    Properties
    {
        _Albedo ("漫射颜色", Color ) = (1,1,1,1)
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
  

        CGPROGRAM
        #pragma surface surf Lambert 

        struct Input
        {
            float4 color : COLOR;
        };

        fixed4 _Albedo;

        void surf (Input IN, inout SurfaceOutput  o)
        {
            //  fixed3 ;  漫射颜色
            //o.Albedo = 1;

            //o.Albedo = fixed3(1,0,0);
            //o.Albedo = fixed3(0,1,0);
            o.Albedo = fixed3(0,0,1);

            o.Albedo = _Albedo.rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
