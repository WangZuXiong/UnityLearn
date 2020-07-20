Shader "ShaderLearn/6.WaveDemo"//顶点着色器的位置偏移
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Speed ("Speed", float) = 1
        _ARange ("弧度", float) = 1
        _Frequency ("频率", float) = 1

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

            //shader 2.0 Properties内的变量还需在这声明一次
            float _Speed;
            float _ARange;
            float _Frequency;


            v2f vert (appdata v)
            {
                v2f o;
                //三角函数公式 y = A*sin(ω*x + φ)
                float timer = _Time.y * _Speed;
                float waver = _ARange * sin(timer + v.vertex.x * _Frequency);
                v.vertex.y = v.vertex.y + waver;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                // just invert the colors
                //col.rgb = 1 - col.rgb;
                return col;
            }
            ENDCG
        }
    }
}


/*
    内置着色器变量
    file:///E:/UnityDocumentation_2019.7.22/Manual/SL-UnityShaderVariables.html

    _Time		自关卡加载以来的时间 (t/20, t, t*2, t*3)，用于将着色器中的内容动画化。
    _SinTime		时间正弦：(t/8, t/4, t/2, t)。
    _CosTime		时间余弦：(t/8, t/4, t/2, t)。
    unity_DeltaTime		增量时间：(dt, 1/dt, smoothDt, 1/smoothDt)。
*/