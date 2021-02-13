Shader "UnityShaderExamples/Diffuse ZWrite"
{
    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        LOD 200

         // 仅渲染到深度缓冲区的额外通道
        Pass
        {
            ZWrite Off
            ColorMask 0
        }

        // 粘贴在透明/漫射着色器的前向渲染通道中
        UsePass "Transparent/Diffuse/FORWARD"
    }

    Fallback "Transparent/VertexLit"
}
