Shader "ShaderLearn/5.Shader2.0 Shader的结构"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert         //#pragma vertex name 表示代码包含给定函数中的顶点程序（此处为 __vert__）。   //定义一个顶点着色器的入口函数
            #pragma fragment frag       //#pragma fragment name 表示代码包含给定函数中的片元程序（此处为 __frag__）。 //定义一个片段着色器的入口函数
            #include "UnityCG.cginc"    //编译之后的指令只是普通的 Cg/HLSL 代码。我们首先包含一个内置 include 文件：   //引入库 D:\Unity\2018.4.0f1\Editor\Data\CGIncludes\UnityCG.cginc

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            //这个函数的参数类型是appdata(是一个结构体struct)
            //这个函数的返回类型是v2f(是一个结构体struct)
            //appdata v是从meshrender里面传进来的
            //v2f 是顶点着色器的输出值 片段着色器的输入值 
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                // just invert the colors
                col.rgb = 1 - col.rgb;
                return col;
            }
            ENDCG
        }
        /*
            常用语义：
            POSITION：获取模型顶点信息
            NORMAL：获取法线信息
            TEXTCOORD(n)：高精度的从顶点传递信息到片段着色器 float2/float3/float4
            COLOR：表示低精度从顶点传递信息到片段着色器 float4
            TANGENT：表示切线信息
            SV_POSITION：UnityObjectToClipPos(v.vertex);
            SV_Target：表示输出到哪一个render target


            //官方释义
            POSITION 是顶点位置，通常为 float3 或 float4。
            NORMAL 是顶点法线，通常为 float3。
            TEXCOORD0 是第一个 UV 坐标，通常为 float2、float3 或 float4。
            TEXCOORD1、TEXCOORD2 和 TEXCOORD3 分别是第 2、第 3 和第 4 个 UV 坐标。
            TANGENT 是切线矢量（用于法线贴图），通常为 float4。
            COLOR 是每顶点颜色，通常为 float4。
        */

        /*
        官方定义的三种结构 D:\Unity\2018.4.0f1\Editor\Data\CGIncludes\UnityCG.cginc
        通常，顶点数据输入在结构中声明，而不是 逐个列出。在 UnityCG.cginc include 文件中 定义了几个常用的顶点结构，在大多数情况下， 仅使用它们就足够了。这些结构为：
        //位置、法线和一个纹理坐标。
        struct appdata_base {
        float4 vertex : POSITION;
        float3 normal : NORMAL;
        float4 texcoord : TEXCOORD0;
        UNITY_VERTEX_INPUT_INSTANCE_ID
        };

        //位置、切线、法线和一个纹理坐标。
        struct appdata_tan {
        float4 vertex : POSITION;
        float4 tangent : TANGENT;
        float3 normal : NORMAL;
        float4 texcoord : TEXCOORD0;
        UNITY_VERTEX_INPUT_INSTANCE_ID
        };

        //位置、切线、法线、四个纹理坐标和颜色。
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
        */
    }
}
