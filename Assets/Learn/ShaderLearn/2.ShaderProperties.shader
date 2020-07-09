Shader "ShaderLearn/2.ShaderProperties"
{
    Properties
    {
        //滑动条
        _TestRange ("Test Range", Range (1, 5)) = 2
        //数字
        _TestFloat ("Test Float", Float) = 1.0
        _TestInt ("Test Int", Int) = 1
        //颜色
        _TestColor ("Test Color", Color) = (1,1,1,1)
        //矢量
        _TestVector ("Test Vector", Vector) = (1,1,1,1)//Shader Lab里面的Vector是四维的
        //纹理
        _Test2D_1 ("Test 2D 1", 2D) = "white" {}
        _Test2D_2 ("Test 2D 2", 2D) = "red" {}
        _TestCube ("Test Cube", Cube) = "" {}
        _Test3D ("Test 3D", 3D) = "" {}

       
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
}
