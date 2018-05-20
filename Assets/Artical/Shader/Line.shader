Shader "Unlit/Line"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_ScrollSpeed("ScrollSpeed",float) = 0
		_Color("Color",Color) = (1,1,1,1)
		
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" }
		LOD 100
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _ScrollSpeed;
			float4 _Color;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				float2 uvScrolled = float2(_ScrollSpeed * _Time.y, 0);
				o.uv = TRANSFORM_TEX(v.uv + uvScrolled, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
			    
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);

				return col * _Color;
			}
			ENDCG
		}
	}
}
