Shader "ShaderLearn/11.AlphaTest_Shader1.0"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _AlphaCut ("_AlphaCut", float) = 0.5
    }
    SubShader
    {
        //AlphaTest//写在这里可以让都多Pass复用

        Pass
        {
           //AlphaTest //写在这里只能使这个Pass生效 

           //AlphaTest Greater 0.5

           AlphaTest Greater [_AlphaCut]


           SetTexture[_MainTex]
           {
            
           }
        }
    }
}
/*
    file:///E:/UnityDocumentation_2019.7.22/Manual/SL-AlphaTest.html

    Shader1.0中 AlphaTest的指令

    Greater	仅渲染 Alpha 大于 AlphaValue 的像素。
    GEqual	仅渲染 Alpha 大于或等于 AlphaValue 的像素。
    Less	仅渲染 Alpha 值小于 AlphaValue 的像素。
    LEqual	仅渲染 Alpha 值小于或等于 AlphaValue 的像素。
    Equal	仅渲染 Alpha 值等于 AlphaValue 的像素。
    NotEqual	仅渲染 Alpha 值不等于 AlphaValue 的像素。
    Always	渲染所有像素。这在功能上等同于 AlphaTest Off。
    Never	不渲染任何像素。
*/