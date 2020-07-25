Shader "SurfaceShaderLearn/2.Vertex"//顶点着色器
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Offeset ("顶点位置偏移量", float) = 1
        _OffesetWithNormal ("沿着法线方向的顶点位置偏移量", float) = 1
        _Scale ("缩放", float) = 1


    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
        
        Cull Off//禁用剔除 - 绘制所有面。用于特殊效果。

        CGPROGRAM


        //可选参数/自定义修改器函数/vertex:VertexFunction
        //vertex:VertexFunction - 自定义顶点修改函数。
        //在生成的顶点着色器的开始处调用此函数，并且此函数可以修改或计算每顶点数据。请参阅表面着色器示例。
        #pragma surface surf Lambert vertex:myVert

        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };


        UNITY_INSTANCING_BUFFER_START(Props)
           
        UNITY_INSTANCING_BUFFER_END(Props)


        half _Offeset;
        half _OffesetWithNormal;
        half _Scale;

        //顶点着色器
        void myVert (inout appdata_base v)
        {
            v.vertex.xyz += _Offeset;
            v.vertex.xyz += v.normal*_OffesetWithNormal;
            v.vertex.xyz *= _Scale;
        }


        //inout关键字表示既是输入也是输出
        void surf (Input IN, inout SurfaceOutput o)
        {
            o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
/*

    struct appdata_base {
    float4 vertex : POSITION;
    float3 normal : NORMAL;
    float4 texcoord : TEXCOORD0;
    UNITY_VERTEX_INPUT_INSTANCE_ID
    };

    struct appdata_tan {
        float4 vertex : POSITION;
        float4 tangent : TANGENT;
        float3 normal : NORMAL;
        float4 texcoord : TEXCOORD0;
        UNITY_VERTEX_INPUT_INSTANCE_ID
    };

    struct appdata_full {
        float4 vertex : POSITION;
        float4 tangent : TANGENT;
        float3 normal : NORMAL;
        float4 texcoord : TEXCOORD0;
        float4 texcoord1 : TEXCOORD1;
        float4 texcoord2 : TEXCOORD2;
        float4 texcoord3 : TEXCOORD3;
        fixed4 color : COLOR;
        UNITY_VERTEX_INPUT_INSTANCE_ID
    };

    struct SurfaceOutput
    {
        fixed3 Albedo;  // 漫射颜色
        fixed3 Normal;  // 切线空间法线（如果已写入）
        fixed3 Emission;
        half Specular;  // 0..1 范围内的镜面反射能力
        fixed Gloss;    // 镜面反射强度
        fixed Alpha;    // 透明度 Alpha
    };
*/