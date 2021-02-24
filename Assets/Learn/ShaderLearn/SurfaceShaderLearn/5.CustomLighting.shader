Shader "SurfaceShaderLearn/5.CustomLighting"//自定义灯光
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
   
        _SpecularPower ("镜面反射的强度", float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM

        #pragma surface surf MyCustom


        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

    
        fixed4 _Color;

        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }

        half _SpecularPower;
        //自定义漫反射  
        half4 LightingMyCustom (SurfaceOutput s, half3 lightDir, half3 viewDir, half atten) //atten 衰减
        {
            half4 result = 0;
            //漫反射颜色 = Dot(L,N) * 灯光颜色
            float3 NDotL = dot(lightDir,s.Normal); //法线和灯光进行点乘
            result.rgb = s.Albedo * _LightColor0 * NDotL * atten ; //_LightColor0：内置着色器变量 （在 Lighting.cginc 中声明）	fixed4	光源颜色。
            result.a = s.Alpha;
            //镜面反射 = 镜面反射 * 灯光颜色 * 衰减数
            half3 H = viewDir - lightDir;
            float HDotN = dot (H,s.Normal);
            result.rgb = s.Albedo * _LightColor0 * NDotL * atten + HDotN * _LightColor0 * atten * _SpecularPower;
            return result;
        }


        ENDCG
    }
    FallBack "Diffuse"
}
/*
    声明光照模型
    一个光照模型中包含多个名称以 Lighting 开头的常规函数。您可以在着色器文件中的任何位置声明这些函数，也可以在其中一个包含的文件中声明。这些函数是：

    1.half4 Lighting<Name> (SurfaceOutput s, UnityGI gi); 在_不依赖于_视图方向的光照模型的前向渲染路径中使用此函数。

    1.half4 Lighting<Name> (SurfaceOutput s, half3 viewDir, UnityGI gi); 在_依赖于_视图方向的光照模型的前向渲染路径中使用此函数。

    1.half4 Lighting<Name>_Deferred (SurfaceOutput s, UnityGI gi, out half4 outDiffuseOcclusion, out half4 outSpecSmoothness, out half4 outNormal); 在延迟光照路径中使用此函数。

    1.half4 Lighting<Name>_PrePass (SurfaceOutput s, half4 light); 在光照预通道（旧版延迟）光照路径中使用此函数。

    请注意，您无需声明所有函数。光照模型不一定会使用视图方向。同样，如果仅在前向渲染中使用光照模型，请勿声明 _Deferred 和 _Prepass 函数。这确保了使用视图方向的着色器仅编译到前向渲染。
*/