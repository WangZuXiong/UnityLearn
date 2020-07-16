Shader "ShaderLearn/OffsetGreen"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Offset 0,0
        Pass
        {
            Color(0,1,0,1)
        }
    }
}
