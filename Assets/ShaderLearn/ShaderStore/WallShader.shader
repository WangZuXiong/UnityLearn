Shader "Custom/WallShader"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_BumpTex ("法线贴图", 2D) = "bump" {}
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM

        sampler2D _MainTex;
		sampler2D _BumpTex;

        struct Input
        {
            float2 uv_MainTex;
        };

		inline fixed4 LightingMyLamber (SurfaceOutput s,fixed3 lightDir,fixed atten)
		{
			fixed4 diff = max (0,dot,(s.Normal,lightDir));
			fixed4 c;
			c.rgb = s.Albedo * _LightColor0.rgb * (diff * atten *2);
			c.a = s.Alpha;
			return c;
		}

        void surf (Input IN, inout SurfaceOutput o)
        {
            half4 c = tex2D(_MainTex,IN.uv_MainTex);
			o.Normal = UnpackNormal(tex2D(_BumpTex,IN.uv_MainTex));
			o.Albedo = c.rgb;
			o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
