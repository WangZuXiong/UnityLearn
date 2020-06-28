Shader "GamePlay/GamePlayShader"
{
	Properties
	{
		_MainTex ("Main Texture", 2D) = "white" {}
		_BumpMap("Normal Map", 2D) = "bump" {}
		_SpeularTex("Specular",2D)="white" {}
		//_SpecMap("Specular Map", 2D) = "white" {}

		_LightDirection("Light Direction", Vector) = (0,0,0,1)
		_LightStrength("Light Strength", Range(0 , 1)) = 1

		_DiffuseStrength("Diffuse Strength", Range(0 , 2)) = 0.5
		_SpecularColor("Specular Color", Color) = (0,0,0,0)
		_Shininess("Shininess", Range(1 , 20)) = 8

		_RimStrength("Fresnel Strength", Range(0 , 2)) = 0.5
		_FresnelExp("Fresnel Exponent", Range(0 , 20)) = 3
		_Alpha("Alpha", Range(0,1)) = 1
	}

	SubShader
	{
		Tags { "Queue" = "Transparent+1" "RenderType" = "Opaque"}
		//Tags{ "Queue" = "Transparent"  "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		//Blend SrcAlpha OneMinusSrcAlpha
		Pass
		{
			Tags { "LightMode" = "ForwardBase" }
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ AR_MODE_ON
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#pragma multi_compile_fog
			struct appdata
			{
				float4 vertex		: POSITION;
				float2 uv			: TEXCOORD0;
				float4 boneIndex	: TEXCOORD1;
				float4 boneWeight	: TEXCOORD2;
				float4 tangent		: TANGENT;
				float3 normal		: NORMAL;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 worldNormal : NORMAL;

				half3 tspace0 : TEXCOORD1; // tangent.x, bitangent.x, normal.x
				half3 tspace1 : TEXCOORD2; // tangent.y, bitangent.y, normal.y
				half3 tspace2 : TEXCOORD3; // tangent.z, bitangent.z, normal.z

				half3 worldPos :TEXCOORD4;

				UNITY_FOG_COORDS(5)
			};
            uniform float4x4 _StadiumTRS;
			uniform float4x4 _SkeletonMatrices[17];
			uniform sampler2D _MainTex;
			sampler2D _SpeularTex;
			uniform float4 _MainTex_ST;

			uniform sampler2D _BumpMap;
			//uniform sampler2D _SpecMap;
			uniform half4 _LightDirection;
			uniform half _LightStrength;
			uniform half4 _SpecularColor;
			uniform half _RimStrength;
			uniform half _Shininess;
			uniform half _DiffuseStrength;
			uniform half _FresnelExp;
            float _Alpha;
			uniform fixed4 _CorrectedMainTeamColor;
			uniform fixed4 _CorrectedTunnelLightColor;
			uniform fixed4 _MainLightColor;

			v2f vert(appdata v)
			{
				v2f o;
				float4x4 skinTransform = 0;
				skinTransform += _SkeletonMatrices[(int)v.boneIndex.x] * v.boneWeight.x;
				skinTransform += _SkeletonMatrices[(int)v.boneIndex.y] * v.boneWeight.y;
				skinTransform += _SkeletonMatrices[(int)v.boneIndex.z] * v.boneWeight.z;
				skinTransform += _SkeletonMatrices[(int)v.boneIndex.w] * v.boneWeight.w;

				float4 worldPos = mul(skinTransform, v.vertex);
				float3 worldNormal = mul(skinTransform, v.normal);
#ifdef AR_MODE_ON
		        worldPos = mul(_StadiumTRS, worldPos);
		        worldNormal = mul(_StadiumTRS, worldNormal);
#endif
				o.vertex = mul(UNITY_MATRIX_VP, worldPos);
				o.worldNormal = worldNormal;
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);


				half3 worldTangent = mul(skinTransform, v.tangent);
				// compute bitangent from cross product of normal and tangent
				half3 worldBitangent = cross(o.worldNormal, worldTangent);
				// output the tangent space matrix
				o.tspace0 = half3(worldTangent.x, worldBitangent.x, o.worldNormal.x);
				o.tspace1 = half3(worldTangent.y, worldBitangent.y, o.worldNormal.y);
				o.tspace2 = half3(worldTangent.z, worldBitangent.z, o.worldNormal.z);
				UNITY_TRANSFER_FOG(o, o.vertex);
				o.worldPos = worldPos;
				return o;
			}
			
			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 albedo = tex2D(_MainTex, i.uv);
				fixed4 specular=tex2D(_SpeularTex,i.uv);
				//fixed4 specular = tex2D(_SpecMap, i.uv);

				half3 tnormal = normalize(UnpackNormal(tex2D(_BumpMap, i.uv))*.15 + half3(0,0,1));
				// transform normal from tangent to world space
				half3 worldNormal;
				worldNormal.x = dot(i.tspace0, tnormal);
				worldNormal.y = dot(i.tspace1, tnormal);
				worldNormal.z = dot(i.tspace2, tnormal);
				worldNormal = normalize(worldNormal);

				float3 worldViewDir = normalize(_WorldSpaceCameraPos - i.worldPos);
				float3 worldLightDir = normalize(_LightDirection.xyz);


				float3 H = reflect(-worldLightDir, worldNormal);
				half NH = max(0, dot(H, worldViewDir));

				half nl = max(0, dot(worldNormal, worldLightDir));

				half3 ambient = ShadeSH9(half4(worldNormal, 1));
				//_MainTeamColor = lerp(_MainTeamColor, half4(1,1,1,1), 0.5);

				ambient = ambient.r*_CorrectedMainTeamColor.rgb + ambient.g*_CorrectedTunnelLightColor.rgb + ambient.b*_MainLightColor;

				half4 col = half4(0, 0, 0, _Alpha);

				//diffuse term
				col.rgb += _DiffuseStrength* albedo.rgb *ambient;

				//specular term
				col.rgb += specular.r * pow(NH, _Shininess) * 0.33;
				
				//Fresnel term
				half rim = saturate(pow(1.0 - DotClamped(worldViewDir, worldNormal), _FresnelExp))   *_RimStrength;
				col.rgb += 0.33 * ambient * rim;
				//col.a = 1;

				//col.rgb = ambient * 0.5;
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}

		
	}
	FallBack "GamePlay/GamePlayer"
	}