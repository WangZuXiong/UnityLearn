Shader "SurfaceShaderLearn/3.Fragment"//片段着色器
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _SpeedX ("Speed X", float) = 1
        _SpeedY ("Speed Y", float) = 1

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Cull Off

        CGPROGRAM
        #pragma surface surf Lambert

        sampler2D _MainTex;
        fixed4 _Color;
        float _SpeedX;
        float _SpeedY;

        struct Input
        {
            float2 uv_MainTex;
            //表面着色器输入结构
            /*
            输入结构 Input 通常具有着色器所需的所有纹理坐标。纹理坐标必须命名为“uv”后跟纹理名称的形式（如果要使用第二个纹理坐标集，则以“uv2”开头）。

            可以放入输入结构的其他值：

            float3 viewDir - 包含视图方向，用于计算视差效果、边缘光照等等。
            具有 COLOR 语义的 float4 - 包含插值的每顶点颜色。
            float4 screenPos - 包含反射或屏幕空间效果的屏幕空间位置。请注意，这不适合 GrabPass；您需要使用 ComputeGrabScreenPos 函数自己计算自定义 UV。
            float3 worldPos - 包含世界空间位置。
            float3 worldRefl - 在_表面着色器不写入 o.Normal_ 的情况下，包含世界反射矢量。有关示例，请参阅反光漫射 (Reflect-Diffuse) 着色器。
            float3 worldNormal - 在_表面着色器不写入 o.Normal_ 的情况下，包含世界法线矢量。
            float3 worldRefl; INTERNAL_DATA - 在_表面着色器写入 o.Normal_ 的情况下，包含世界反射矢量。要获得基于每像素法线贴图的反射矢量，请使用 WorldReflectionVector (IN, o.Normal)。有关示例，请参阅反光凹凸 (Reflect-Bumped) 着色器。
            float3 worldNormal; INTERNAL_DATA - 在_表面着色器写入 o.Normal_ 的情况下，包含世界法线矢量。要获得基于每像素法线贴图的法线矢量，请使用 WorldNormalVector (IN, o.Normal)。
            */

        };

        UNITY_INSTANCING_BUFFER_START(Props)
           
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutput o)
        {
            float2 tempUv = IN.uv_MainTex;
            tempUv.x += _Time.x * _SpeedX;
            tempUv.y += _Time.y * _SpeedY;

            fixed4 c = tex2D (_MainTex, tempUv) * _Color;
            o.Albedo = c.rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
