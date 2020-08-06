Shader "ShaderLearn/Fill Image"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Left("Left", Range(0,1)) = 1
        _Right("Right", Range(0,1)) = 1
        _Top("Top", Range(0,1)) = 1
        _Bottom("Bottom", Range(0,1)) = 1

    }
    SubShader
    {
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
            float _Right;
            float _Left;
            float _Top;
            float _Bottom;


            fixed4 frag (v2f i) : SV_Target
            {
                if (i.uv.x > _Right || 1 - i.uv.x > _Left || i.uv.y > _Top || 1 - i.uv.y > _Bottom)
                {
                    return fixed4(0, 0, 0,0);
                }

                return tex2D(_MainTex, i.uv);
            }
            ENDCG
        }
    }
    Fallback "diffuse"
}
