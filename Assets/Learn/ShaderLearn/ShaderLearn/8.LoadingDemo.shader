Shader "ShaderLearn/8.LoadingDemo"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Speed("Speed", float) = 1
    }
    SubShader
    {
        // No culling or depth
        //Cull Off ZWrite Off ZTest Always

        //当前要渲染的纹理和已经Gbuffer像素进行混合 把这个打开就可以把alpha通道打开
        Blend SrcAlpha OneMinusSrcAlpha 

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
            float _Speed;

            fixed4 frag (v2f i) : SV_Target
            {

                
                float2 angle = _Time.x * _Speed;

                float2 tempUv = i.uv;
                //1.将uv中心移动到原点
                tempUv -= float2(0.5,0.5);
                //截取一个圆形
               if(length(tempUv) > 0.5)
               {
                    return fixed4(0,0,0,0);
               }
                //2.实现uv的旋转
                float2 finalUv = 0;
                finalUv.x = tempUv.x * cos(angle) - tempUv.y * sin(angle);
                finalUv.y = tempUv.x * sin(angle) + tempUv.y * cos(angle);
                //3.再将uv中心移动到原来的位置
                finalUv += float2(0.5,0.5);

                fixed4 col = tex2D(_MainTex, finalUv);
                // just invert the colors
                //col.rgb = 1 - col.rgb;
                return col;
            }
            ENDCG
        }
    }
}
