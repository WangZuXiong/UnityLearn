Shader "ShaderLearn/13.StencilTest"//模板测试
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
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

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
    }
}


/*
    file:///E:/UnityDocumentation_2019.7.22/Manual/SL-Stencil.html

    if((referenceValue & readMask) comparisonFunction (stencilBufferValue & readMask))
    {
        通过    
    }else
    {
        丢弃
    }

    Ref referenceValue
    //要比较的参考值（如果 Comp 是 always 以外的任何值）和/或要写入缓冲区的值（如果 Pass、Fail 或 ZFail 设置为替换）。值为 0 到 255 之间的整数。


    //比较函数comparisonFunction
    Greater	仅渲染参考值大于缓冲区值的像素。
    GEqual	仅渲染参考值大于或等于缓冲区值的像素。
    Less	仅渲染参考值小于缓冲区值的像素。
    LEqual	仅渲染参考值小于或等于缓冲区值的像素。
    Equal	仅渲染参考值等于缓冲区值的像素。
    NotEqual	仅渲染参考值不同于缓冲区值的像素。
    Always	使模板测试始终通过。
    Never	使模板测试始终失败。

    //模板操作
    Keep	    保持缓冲区的当前内容。
    Zero	    将 0 写入缓冲区。
    Replace	    将参考值写入缓冲区。
    IncrSat	    递增缓冲区中的当前值。如果该值已经是 255，则保持为 255。
    DecrSat	    递减缓冲区中的当前值。如果该值已经是 0，则保持为 0。
    Invert	    将所有位求反。
    IncrWrap	递增缓冲区中的当前值。如果该值已经是 255，则变为 0。
    DecrWrap	递减缓冲区中的当前值。如果该值已经是 0，则变为 255。
*/
