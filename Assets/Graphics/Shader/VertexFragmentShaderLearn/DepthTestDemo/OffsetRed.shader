Shader "Hidden/OffsetGreen"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Offset 1,0
        Pass
        {
            Color(1,0,0,1)
        }
    }
}
