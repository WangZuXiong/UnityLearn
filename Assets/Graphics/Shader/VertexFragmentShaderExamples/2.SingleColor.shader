// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "VertexFragmentShaderExamples/2.SingleColor"
{
    Properties
    {
        //材质检视面板的颜色属性，默认颜色为白色
        _Color ("Main Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
      
            //顶点着色器
            //这次不适用 “appdata” 结构，仅手动拼写输入
            //并且不返回 v2f 结构，同样仅返回单个输出
            //float4 裁剪位置
            float4 vert (float4 vertex : POSITION) : SV_POSITION
            {
                return UnityObjectToClipPos(vertex);
            }
            
            //来之材质的颜色
            float4 _Color;

            //像素着色器
            fixed4 frag () : SV_Target
            {
               return _Color;
            }
            ENDCG
        }
    }
}

//这次，着色器函数只是手动拼出输入，而不是使用输入结构 (appdata) 和输出结构 (v2f)。两种方式都有效，选择使用哪种方式取决于您编写代码的风格和偏好。