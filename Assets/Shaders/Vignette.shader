Shader "Custom/RainbowVignette"
{
	Properties
	{
		[NoScaleOffset] _NormalTex("Normal Texture", 2D) = "white" {}
		[NoScaleOffset] _TunnelTex("Tunnel Texture", 2D) = "white" {}
		[NoScaleOffset] _RainbowTex("Rainbow Texture", 2D) = "white" {}
		[NoScaleOffset] _SpotsTex("Spots Texture", 2D) = "white" {}
		_BandingStrength("Banding Strength", Float) = 0
		_Alpha("Alpha", Range(0, 1)) = 0
		_GlobalTime("Global Time", Float) = 0
	}

	SubShader
	{
		Tags
		{
			"RenderType" = "Transparent"
			"Queue" = "Transparent-10"
		}

        ZWrite Off
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
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			sampler2D _NormalTex;
			sampler2D _TunnelTex;
			sampler2D _RainbowTex;
			sampler2D _SpotsTex;
			float _BandingStrength;
			float _Alpha;
			float _GlobalTime;
			
			fixed4 frag (v2f i) : SV_Target
			{
				float t = _GlobalTime / 20;

				// Speedlines at edge

				fixed2 off = i.uv - fixed2(0.5, 0.5);
				fixed2 dir = normalize(off);
				fixed len = length(off) / (sqrt(2) * 0.5);
				fixed2 coords = fixed2(t, t) * 0.5 + dir * pow(len, 0.01);
				fixed edges = tex2D(_SpotsTex, coords).r * pow(len, 2);

				// Iridescent normals

				fixed2 aspect = fixed2(_ScreenParams.x / _ScreenParams.y, 1);
				fixed2 uv1 = i.uv * aspect * 0.6 + fixed2(t * 0.6, t * 0.9);
				fixed2 uv2 = i.uv * aspect * 0.3 + fixed2(t * 0.9, t * 0.6);

				fixed3 normal1 = UnpackNormal(tex2D(_NormalTex, uv1));
				fixed3 normal2 = UnpackNormal(tex2D(_NormalTex, uv2));
				fixed3 wobble = lerp(normal1, normal2, 0.5);

				fixed3 normal3 = normalize(fixed3(0.5, 0.5, 0.25) - fixed3(i.uv, 0));
				fixed3 normal4 = UnpackNormal(tex2D(_TunnelTex, i.uv));
				fixed3 tunnel = lerp(normal3, normal4, 0.25);
				
				fixed3 norm = lerp(wobble, tunnel, 0.8);

				// Color

				fixed b = length(norm - fixed3(0, 0, 1)) * _BandingStrength;
				fixed3 color = tex2D(_RainbowTex, fixed2(b - t * 4, 0));
				color = pow(color, len * len - edges * len);

				// Alpha

				fixed alpha = saturate((len - (1 - _Alpha) + edges * 0.75) * 5);
				alpha = lerp(alpha, 1, pow(_Alpha, 8));
				alpha *= pow(_Alpha, 0.1);

				return fixed4(color.rgb, alpha);
			}

			ENDCG
		}
	}
}