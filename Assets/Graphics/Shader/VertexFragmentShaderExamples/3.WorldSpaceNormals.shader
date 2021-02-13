Shader "VertexFragmentShaderExamples/3.WorldSpaceNormals"//使用网格法线，轻松获利
{
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
           
            // 包含 UnityObjectToWorldNormal helper 函数的 include 文件
            #include "UnityCG.cginc"

            struct v2f
            {
                //将输出世界空间发现作为常规("texcoord") 插值器之一//coord坐标
                half3 worldNormal : TEXCOORD0;
                float4 pos : SV_POSITION;
            };

            //顶点着色器：将对象空间法线也作为输入
            v2f vert (float4 vertex : POSITION, float3 normal : NORMAL)
            {
                v2f o;
                //该函数将顶点从对象空间转换为屏幕。这使得代码更易于阅读，并且在某些情况下更有效。
                o.pos = UnityObjectToClipPos(vertex);
                // UnityCG.cginc 文件包含将法线从对象变换到世界空间的函数，请使用该函数
                o.worldNormal = UnityObjectToWorldNormal(normal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = 0;
                //法线是具有xyz分量的矢量，处于-1..1的范围，要将其显示为颜色，请将其范围设置为0..1，并放入rgb分量中
                col.rgb = i.worldNormal * 0.5 + 0.5;
                return col;
            }
            ENDCG
        }
    }
}

//在所谓的“插值器”（有时称为“变化”）中，我们已经看到数据可从顶点传入片元着色器。在 HLSL 着色语言中，它们通常用 TEXCOORDn 语义进行标记，其中每一个最多可以是一个 4 分量矢量（有关详细信息，请参阅语义页面）
//此外，我们学习了如何使用一种简单的技术将标准化矢量（在 –1.0 到 +1.0 范围内）可视化为颜色：只需将它们乘以二分之一并加二分之一