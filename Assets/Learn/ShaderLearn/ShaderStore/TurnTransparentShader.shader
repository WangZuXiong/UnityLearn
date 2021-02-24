
Shader "UIEffect/TurnTransparentShader" 
{
	Properties
	{
		_MainTex("Main Tex", 2D) = "white" {}
		_BlurTex("Glow Blur Tex", 2D) = "white" {}
		_BlurFactor ("Blur Factor", range(0, 1)) = 0
		[Space][Toggle]_GlowEffect("Glow Effect", float) = 0
		_GlowTex("Glow Tex", 2D) = "white" {}
		_GlowParam("Glow Scale & Speed", vector) = (1, 1, 1, 1)
		_GlowColor("Glow Color", Color) = (1, 1, 1, 1)
		_OffsetY("Offest Y", float) = 0.865
		_Brightness("Brightness",float)=1
	}

	SubShader
	{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }

		ZWrite Off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata_t{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float2 uv_r1 : TEXCOORD1;
				float2 uv_r2 : TEXCOORD2;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _BlurTex;
			float _BlurFactor;
			sampler2D _GlowTex;
			float4 _GlowTex_ST;
			float _GlowEffect;
			float4 _GlowParam;
			fixed4 _GlowColor;
			float _OffsetY;
			float _Brightness;
			v2f vert(appdata_t v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv.xy * _MainTex_ST.xy + float2(0, _OffsetY);
				// rotation 1
				float theta = _GlowParam.z * _Time.x;
				float sinTheta = sin(theta);
				float cosTheta = cos(theta);
				float2x2 rotationMatrix = float2x2(cosTheta, -sinTheta, sinTheta, cosTheta);
				float2 vert = TRANSFORM_TEX(v.uv, _GlowTex) - float2(0.5, 0.5);
				o.uv_r1 = mul(vert * _GlowParam.xy, rotationMatrix)  + float2(0.5, 0.5);
				// rotation 2
				theta = _GlowParam.w * -_Time.x;
				sinTheta = sin(theta);
				cosTheta = cos(theta);
				rotationMatrix = float2x2(cosTheta, -sinTheta, sinTheta, cosTheta);
				vert = TRANSFORM_TEX(v.uv, _GlowTex) - float2(0.5, 0.5);
				o.uv_r2 = mul(vert * _GlowParam.xy, rotationMatrix)  + float2(0.5, 0.5);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 base = tex2D(_MainTex, i.uv);
				fixed4 blur = tex2D(_BlurTex, i.uv);
				fixed4 glow1 = tex2D(_GlowTex, i.uv_r1);
				fixed4 glow2 = tex2D(_GlowTex, i.uv_r2);
				fixed4 final = fixed4(0, 0, 0, 1);
				final.xyz = base.xyz * (1 - _BlurFactor) + blur.xyz * _BlurFactor;
				float blendfactor = max(glow1.w, glow2.w) * (1 - base.w) * _GlowEffect;
				final.xyz = final.xyz * (1 - blendfactor) + _GlowColor.xyz * blendfactor* _Brightness;
				return final;
			}
			ENDCG
		}
	}
}