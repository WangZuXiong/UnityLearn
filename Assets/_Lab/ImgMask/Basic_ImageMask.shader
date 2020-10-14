Shader "Gala/Basic/ImageMask"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		//_info("Info", 2D) = 0
		_ratio("Ratio", Range(0 , 1)) = 0
		_alpha("Alpha", Range(0 , 1)) = 0
	}
    SubShader
    {
        Tags
        { 
            "Queue"="Overlay" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Back
        Lighting Off
        ZWrite Off
		ZTest Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            Name "Default"
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            #pragma multi_compile __ UNITY_UI_ALPHACLIP
            
            struct appdata_t
            {
                float4 vertex   : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                half2 texcoord  : TEXCOORD0;
				//half2 center : TEXCOORD1;
				float4 bound	: TEXCOORD1;
            };
            
			float4 _info;
			float _ratio;
			fixed _alpha;

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);

                OUT.texcoord = IN.texcoord;
                
                #ifdef UNITY_HALF_TEXEL_OFFSET
                OUT.vertex.xy += (_ScreenParams.zw-1.0) * float2(-1,1) * OUT.vertex.w;
                #endif
                
				//OUT.center = float2(_info.x/_ScreenParams.x, _info.y/_ScreenParams.y);

				//OUT.border = float4(((0.5f * _info.z) + _ratio*_info.x)/_ScreenParams.x, 
				//((0.5f * _info.z) + _ratio * (_ScreenParams.x -_info.x))/_ScreenParams.x, 
				//((0.5f * _info.w) + _ratio * (_info.y))/_ScreenParams.y, 
				//((0.5f * _info.w) + _ratio * (_ScreenParams.y - _info.y))/_ScreenParams.y);

				//OUT.texcoord = _info.x
				_ratio = 1 - _ratio;
				OUT.bound = float4(
                (_info.x - 0.5 * _info.z)*_ratio/_ScreenParams.x, 
				(_info.y - 0.5 * _info.w)*_ratio/_ScreenParams.y, 
				((_info.x + 0.5 * _info.z)*_ratio + (_ScreenParams.x)*(1-_ratio))/_ScreenParams.x,
				((_info.y + 0.5 * _info.w)*_ratio + (_ScreenParams.y)*(1-_ratio))/_ScreenParams.y);

                return OUT;
            }

			sampler2D _MainTex;
			float4 _MainTex_ST;

            fixed4 frag(v2f IN) : SV_Target
            {
				float tu = (IN.texcoord.x - IN.bound.x)/(IN.bound.z - IN.bound.x);
				float ty = (IN.texcoord.y - IN.bound.y)/(IN.bound.w - IN.bound.y);
				fixed4 col = tex2D(_MainTex, float2(tu, ty));
				//half deltaX = IN.center.x - IN.texcoord.x;
				//half deltaY = IN.center.y - IN.texcoord.y;
				//half transAlpha = min(0.5,step(-deltaX, -IN.border.x) + 
				//step(deltaX, -IN.border.y) + 
				//step(-deltaY, -IN.border.z) + 
				//step(deltaY, -IN.border.w));
                return fixed4(0, 0, 0, col.a*_alpha);  
            }
        ENDCG
        }
    }
}