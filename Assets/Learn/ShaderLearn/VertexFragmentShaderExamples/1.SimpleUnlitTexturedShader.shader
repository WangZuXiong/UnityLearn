// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "VertexFragmentShaderExamples/1.SimpleUnlitTexturedShader"
{
    Properties
    {
        //[NoScaleOffset]标签：让纹理平铺/偏移不显示在材质的检视面板中
        [NoScaleOffset]_MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            // 使用 "vert" 函数作为顶点着色器
            #pragma vertex vert     
            // 使用 "frag" 函数作为像素（片元）着色器
            #pragma fragment frag   
          
            #include "UnityCG.cginc"

            // 顶点着色器输入
            struct appdata
            {
                float4 vertex : POSITION;   // 顶点位置
                float2 uv : TEXCOORD0;      // 纹理坐标
            };

            // 顶点着色器输出（"顶点到片元"）
            struct v2f
            {
                float2 uv : TEXCOORD0;      // 纹理坐标
                float4 vertex : SV_POSITION;// 裁剪空间位置
            };

            // 顶点着色器
            v2f vert (appdata v)
            {
                v2f o;
                // 将位置转换为裁剪空间
                //（乘以模型*视图*投影矩阵）
                o.vertex = UnityObjectToClipPos(v.vertex);
                // 仅传递纹理坐标
                o.uv = v.uv;
                return o;
            }

            // 我们将进行采样的纹理
            sampler2D _MainTex;

            // 像素着色器；返回低精度（"fixed4" 类型）
            // 颜色（"SV_Target" 语义）
            fixed4 frag (v2f i) : SV_Target
            {
                // 对纹理进行采样并将其返回
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}

//一些变量或函数定义后跟一个语义指示符，例如 : POSITION 或 : SV_Target。这些语义指示符将这些变量的“含义”传达给 GPU。有关详细信息，请参阅着色器语义页面。