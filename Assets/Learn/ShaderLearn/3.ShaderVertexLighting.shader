Shader "ShaderLearn/3.ShaderVertexLighting"//顶点着色器
{
    Properties
    {
        _TestColor ("TestColor", Color) = (1,1,1,1)
        _Shininess ("光泽度数字", Range(0.01,1)) = 0.7 
        _SpecularColor ("镜面反射颜色",Color) = (1,1,1,1)
    }
    SubShader
    {
        Pass
        {
            //将对象设置为纯色。颜色是用圆括号括起的四个 RGBA 值，或者是用方括号括起的颜色属性名称。
            //Color(1,0,0,1)
            //Color[_TestColor]

            //Material 代码块用于定义对象的材质属性。
            Material
            {
                //漫射颜色 
                Diffuse[_TestColor]

                //环境颜色
                Ambient[_TestColor]

                //光泽度数字
                Shininess[_Shininess]

                //镜面反射颜色
                Specular[_SpecularColor]
            }

            //要使 Material 代码块中定义的设置生效，必须使用 Lighting On 命令启用光照。如果关闭光照，则直接从 Color 命令获取颜色。
            Lighting On

            //此命令将镜面反射光照添加到着色器通道的末尾，因此镜面反射光照不受纹理影响。仅当使用 Lighting On 时才有效。
            SeparateSpecular On

        }
    }
}
