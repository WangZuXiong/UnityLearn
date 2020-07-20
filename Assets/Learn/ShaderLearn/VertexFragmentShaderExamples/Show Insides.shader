Shader "UnityShaderExamples/Show Insides"//此对象将仅渲染对象的背面：
{
    SubShader
    {
        Pass
        {
           Material
           {
            Diffuse (1,1,1,1)
           }

           Lighting On
           Cull Front

           /*
           控制应该剔除多边形的哪些面（不绘制）

                Back 不渲染背离观察者的多边形_（默认值）_。

                Front 不渲染面向观察者的多边形。用于从里到外翻转对象。

                Off 禁用剔除 - 绘制所有面。用于特殊效果。
           */
        }
    }
}