Shader "Custom/WorldMapBubble"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_NoiseTex ("Noise Texture", 2D) = "white" {}
		_FadeStart ("Fade Start", Range(0, 1)) = 0
		_FadeEnd ("Fade End", Range(0, 1)) = 1
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

			sampler2D _MainTex;
			sampler2D _NoiseTex;
			float _FadeStart;
			float _FadeEnd;
			
			fixed4 frag (v2f i) : SV_Target
			{
				float t = saturate(length(i.uv * 2 - 1));
				float noise = tex2D(_NoiseTex, i.uv * 0.5 + _Time.x * 0.5);

				t += (noise - 0.5) * 0.05;
				t = saturate((t - _FadeStart) / (_FadeEnd - _FadeStart));
				t = pow(t, 0.5);

				i.uv += t * 0.25 + noise * 0.005;
				i.uv.x += _SinTime.y * 0.05;
				i.uv.y += _CosTime.y * 0.05;

				fixed4 col = tex2D(_MainTex, i.uv);
				col.a = 1 - t;
				return col;
			}

			ENDCG
		}
	}
}