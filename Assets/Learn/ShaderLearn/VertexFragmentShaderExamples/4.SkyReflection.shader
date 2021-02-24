// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "VertexFragmentShaderExamples/4.SkyReflection"
{
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct v2f
            {
                half3 worldRefl : TEXCOORD0;
                float4 pos : SV_POSITION;
            };

            v2f vert (float4 vertex : POSITION,float3 normal : NORMAL)
            {
               v2f o;
                o.pos = UnityObjectToClipPos(vertex);
                // 计算顶点的世界空间位置
                float3 worldPos = mul(unity_ObjectToWorld, vertex).xyz;
                // 计算世界空间视图方向
                float3 worldViewDir = normalize(UnityWorldSpaceViewDir(worldPos));
                // 世界空间法线
                float3 worldNormal = UnityObjectToWorldNormal(normal);
                // 世界空间反射矢量
                o.worldRefl = reflect(-worldViewDir, worldNormal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
              // 使用反射矢量对默认反射立方体贴图进行采样
                half4 skyData = UNITY_SAMPLE_TEXCUBE(unity_SpecCube0, i.worldRefl);
                // 将立方体贴图数据解码为实际颜色
                half3 skyColor = DecodeHDR (skyData, unity_SpecCube0_HDR);
                // 将其输出！
                fixed4 c = 0;
                c.rgb = skyColor;
                return c;
            }
            ENDCG
        }
    }
}
