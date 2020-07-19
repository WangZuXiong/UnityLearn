Shader "ShaderLearn/16.Tags"//子着色器使用标签来告知它们期望何时以何种方式被渲染到渲染引擎。
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        //渲染顺序 - Queue 标签
        Tags {"Queue" = "Transparent"}//sphere cube

        //Tags {"Queue" = "Geometry"}//sphere cube


        Pass
        {
            
        }

        /*
            您可以使用 Queue 标签来确定对象的绘制顺序。着色器决定其对象属于哪个渲染队列，这样任何透明着色器都可以确保它们在所有不透明对象之后绘制，依此类推。

            有四个预定义的渲染队列，但预定义的渲染队列之间可以有更多的队列。预定义队列包括：

            Background - 此渲染队列在任何其他渲染队列之前渲染。通常会对需要处于背景中的对象使用此渲染队列。
            Geometry（默认值）- 此队列用于大部分对象。不透明几何体使用此队列。
            AlphaTest - 进行 Alpha 测试的几何体将使用此队列。这是不同于 Geometry 队列的单独队列，因为在绘制完所有实体对象之后再渲染经过 Alpha 测试的对象会更有效。
            Transparent - 此渲染队列在 Geometry 和 AlphaTest 之后渲染，按照从后到前的顺序。任何经过 Alpha 混合者（即不写入深度缓冲区的着色器）都应该放在这里（玻璃、粒子效果）。
            Overlay - 此渲染队列旨在获得覆盖效果。最后渲染的任何内容都应该放在此处（例如，镜头光晕）。
        */
    }
}
