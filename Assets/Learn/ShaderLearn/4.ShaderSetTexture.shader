Shader "ShaderLearn/4.ShaderSetTexture"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _BlendTex ("Blend Texture", 2D) = "white" {}

        _MainRange ("Main Range", Range(0,1)) = 1
        _BlendRange ("Blend Range", Range(0,1)) = 1

        
        _Color ("Color", Color) = (1,1,1,1)

        

    }
    SubShader
    {
        Pass
        {
            Color[_Color]
           
            SetTexture [_MainTex]
            {
                //Combine primary * Texture  //其实就是像素点相乘 (1,1,1,1)*(1,1,0.5,0.5) 越乘越暗
                Combine primary + Texture   //其实就是像素点相加 (1,1,1,1)+(1,1,0.5,0.5) 越加越亮
            }

            SetTexture [_MainTex]
            {
                Combine Texture  
            }

            
            SetTexture [_BlendTex]
            {
                //纹理的混合    
                //Combine Previous * Texture //上一个纹理和当前纹理相乘
                //Combine (Previous * _MainRange) * (Texture * _BlendRange) 不行

                //combine src1 lerp (src2) _src3_：使用 src2 的 Alpha 在 src3 和 src1 之间插值。请注意，插值方向是相反的：当 Alpha 为 1 时使用 src1，而当 Alpha 为 0 时使用 src3。
                //Combine  Texture lerp(Previous) Previous //(1 - t) * A + t * B
                
                //相当于创建一个临时变量
                //ConstantColor (0,0,1,1)
                ConstantColor[_Color]
                Combine Texture * Constant
            }




            //几个关键字的含义
            //Previous 表示前一个 SetTexture 出来以后的像素
            //Preimary 表示顶点计算出来的颜色
            //Texture 等于 SetTexture 当前的纹理变量
            //Constant 表示一个固定的颜色

            /*
            官方释义
            Previous 表示上一个 SetTexture 的结果。
            Primary 表示光照计算产生的颜色或者是顶点颜色（如果已绑定）。
            Texture 表示由 SetTexture 中 TextureName 指定的纹理的颜色（请参阅上文）。
            Constant 表示 ConstantColor 中指定的颜色。
            */
        }
    }
}
