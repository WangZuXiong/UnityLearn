//所有着色器都以关键字 Shader 开头，后跟一个表示着色器名称的字符串。这是在__检视面板__中显示的名称。此着色器的所有代码必须放在随后的大括号内：__{ }__（称为代码块）。
//名称应该言简意赅。不一定要与 .shader 文件名匹配。
//要将着色器放入到 Unity 中的子菜单中，请使用斜杠，例如 MyShaders/Test 将在名为 MyShaders 的子菜单中显示为 Test__，或显示为 MyShaders > Test__。
Shader "ShaderLearn/1.Shader的结构"//shader的名字
{
    Properties//shader的属性 暴露在外面的成员变量
    {
       _MainTex ("Texture", 2D) = "white" {}
    }
    //不同的图形硬件具有不同的功能。
    //例如，某些显卡支持片元程序，其他显卡则不支持；
    //有些显卡在每个通道中可以放下四个纹理，而有些只能放下两个或一个；
    //为了让您充分利用用户拥有的任何硬件，着色器可以包含多个__子着色器 (SubShader)__。
    //当 Unity 渲染着色器时，它将遍历所有子着色器并采用硬件支持的第一个子着色器。

    SubShader//可以定义多个SubShader 符合的显卡
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        //每个子着色器由多个通道组成，
        //每个通道代表 为使用着色器材质渲染的同一对象执行 顶点和片元代码。 
        //许多简单的着色器只使用一个通道，但与光照交互的 着色器可能需要更多通道（有关详细信息，请参阅 光照管线）。
        //通道内部的 命令通常设置固定函数状态，例如 混合模式。
        Pass//一个SubShader里面可以有多个Shader。每个Pass都会渲染一次，多个Pass就会渲染多次。
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

    //如果前面个的SubShader都不兼容当前的显卡，就会执行Fallback
    // 可选回退
    //Fallback 命令可以在着色器的末尾使用；
    //如果当前着色器中没有__子着色器__可以在用户的图形硬件上运行，该命令会告诉应该使用哪个着色器。
    //效果等同于在结尾包含回退着色器中的所有子着色器。
    //例如，如果您要编写一个花哨的法线贴图着色器，那么您可以回退到内置的 VertexLit__ 着色器，而不必为旧显卡编写一个非常基本的非法线贴图子着色器。
    Fallback "diffuse"
}
