Shader "KIM/3D/Character" {
	Properties {
		[Toggle] _PlayAnimation ("Play Animation", Float) = 0
		[Toggle] _VipPerson ("Vip Person", Float) = 0
		[NoScaleOffset] _MainTex ("Texture", 2D) = "white" {}
		_Color ("Main Color", Vector) = (1,1,1,1)
		_HSV ("HSV Control", Vector) = (0,1,0.5,3)
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		fixed4 _Color;
		struct Input
		{
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
}