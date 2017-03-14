Shader "Hidden/Fog" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_bwBlend ("Black & White blend", Range (0, 1)) = 0
	}
	SubShader {
		Pass {
			CGPROGRAM
			#pragma vertex verti
			#pragma fragment frag

			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			uniform float _bwBlend;
			uniform sampler2D_float _CameraDepthTexture;

			struct vertInput {
				float4 pos : POSITION;
			};

			struct vertOut {
				float4 pos : SV_POSITION;
				float poss : QQ;
			};

			vertOut verti(vertInput input) {
				vertOut o;
				o.poss = input.pos.y;
				o.pos = mul(UNITY_MATRIX_MVP, input.pos);
				return o;
			}

			float4 frag(vertOut i) : COLOR {
				//float4 c = tex2D(_MainTex, i.uv);
				//float lum = c.r*.3 + c.g*.59 + c.b*.11;
				//float3 bw = float3( lum, lum, lum );
				//float4 result = c;
				//result.rgb = lerp(c.rgb, bw, _bwBlend);
				//float dist = length(float3(i.pos.x) / 4000.0f;
				return float4(.2f, 0, 0, 1); //float4(result.x - dist, result.y - dist, result.z - dist, result.w);
			}
			ENDCG
		}
	}
}