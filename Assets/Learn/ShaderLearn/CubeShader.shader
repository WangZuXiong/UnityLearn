/*
Shader "Custom/CubeShader"
{
    Properties
    {
        _Color ("颜色", Color) = (1,1,1,1)
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
		_MainTex ("贴图", 2D) = "white" {}//2D贴图
		_Float("Float Value", Float) = 0.5//浮点数
		_Range("Range Value", Range(0.1, 10)) = 1.0//带范围的浮点数
		_Cube("Cube Map", Cube) = ""{}//CubeMap，天空盒
		_Rect("Rect Type", Rect) = ""{}//非2的幂次方的纹理
		_Vector("Vector Value", Vector) = (1, 2, 3, 4)//四维向量

    }
    SubShader
    {
		//这个Tags里面是当前SubShader的所有标签
        Tags { "RenderType"="Opaque" }
		//Levels of Detail
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
		//Surface方法名叫surf，我们使用的光照模型叫做Standard fullforwardshadows
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

		//后面我们定义了变量_MainTex。注意这里的定义是真正的Shader内部的定义，属性的定义其实还是Unity层面的一个接口，我们用同名的方式来关联属性的值和内部的变量。
        sampler2D _MainTex;

		//在后面我们定义了一个结构体Input，同时可以看到这个结构体是给下面的surf函数用的
        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        UNITY_INSTANCING_BUFFER_START(Props)
            
        UNITY_INSTANCING_BUFFER_END(Props)
		//surf函数我们可以理解为一个回调函数。这个surf函数接受一个参数是我们定义的Input类型，另一个参数SurfaceOutput类型是Unity内置的
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            //内置的tex2D方法，这个方法是从一张贴图中根据贴图坐标得到颜色等信息
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
*/

Shader "Custom/CubeShader" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _MoveAmount("MoveAmount", Range(-1, 1)) = 0
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf MyLight vertex:MyVert finalcolor:MyFinalColor

        sampler2D _MainTex;
        float _MoveAmount;

        struct Input  
        {
            float2 uv_MainTex;
            float4 vertColor;
        };
        void MyVert(inout appdata_full v, out Input IN)
        {
            UNITY_INITIALIZE_OUTPUT(Input, IN);
            v.vertex.xyz += v.normal * _MoveAmount;
            IN.vertColor = v.color;
        }

        void surf (Input IN, inout SurfaceOutput o)
        {  
            float4 c;
            c =  tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }  

        inline fixed4 LightingMyLight (SurfaceOutput s, fixed3 lightDir, fixed atten)
        {
            fixed diff = max (0, dot (s.Normal, lightDir));

            fixed4 c;
            c.rgb = s.Albedo * _LightColor0.rgb * (diff * atten * 2);
            c.a = s.Alpha;
            return c;
        }

        void MyFinalColor(Input IN, SurfaceOutput o, inout fixed4 color) 
		{
			//直接用光照模型输出的颜色信息作为最终的颜色信息
            color = color;
        } 
        ENDCG
    } 
    FallBack "Diffuse"
}