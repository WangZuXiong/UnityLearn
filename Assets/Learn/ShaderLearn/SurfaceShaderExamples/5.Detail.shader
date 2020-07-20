//细节纹理
//为获得不同效果，让我们添加一个与基础纹理结合的细节纹理。细节纹理通常在材质中使用相同的 UV，但使用不同平铺，因此我们需要使用不同的输入 UV 坐标。
Shader "Custom/Detail"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Bumpmap ("Bump Map", 2D) = "bump" {}
        _Detail ("Detail", 2D) = "gray" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }


        CGPROGRAM

        #pragma surface surf Lambert

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BumpMap;
            float2 uv_Detail;
        };
        
        sampler2D _MainTex;
        sampler2D _Bumpmap;
        sampler2D _Detail;

        void surf (Input IN, inout SurfaceOutput o)
        {
            //o.Albedo = tex2D(_MainTex,IN.uv_MainTex).rgb;
            //o.Albedo *= tex2D(_Detail,IN.uv_Detail).rgb * 2;
            o.Albedo = tex2D(_MainTex,IN.uv_MainTex).rgb * tex2D(_Detail,IN.uv_Detail).rgb * 2;
            o.Normal = UnpackNormal(tex2D(_Bumpmap,IN.uv_BumpMap));
        }
        ENDCG
    }
    FallBack "Diffuse"
}
