Shader "Custom/ReplaceColorShader"
{
    Properties
    {
        _MainTex ("MainTex", 2D) = "white" {}
        _OriginalColor("Original Color", Color) = (1,1,1,1)
        _NewColor("New Color", Color) = (1,1,1,1)
        _Tolerance("容差", Range(0,1)) = 0.1
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
            float4  _OriginalColor;
            float4  _NewColor;
            float _Tolerance;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                if (abs(col.r - _OriginalColor.r) < _Tolerance
                && abs(col.g - _OriginalColor.g) < _Tolerance
                && abs(col.b - _OriginalColor.b) < _Tolerance)
                {
                    return _NewColor;
                }
                else
                {
                    return col;
                }
              
            }
            ENDCG
        }
    }
}
