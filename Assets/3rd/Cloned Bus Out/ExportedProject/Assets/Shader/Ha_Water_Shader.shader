Shader "Ha/Water_Shader" {
	Properties {
		_Color ("Tint", Vector) = (1,1,1,0.5)
		_FoamC ("Foam", Vector) = (1,1,1,0.5)
		_MainTex ("Main Texture", 2D) = "white" {}
		_TextureDistort ("Texture Wobble", Range(0, 1)) = 0.1
		_NoiseTex ("Extra Wave Noise", 2D) = "white" {}
		_Speed ("Wave Speed", Range(0, 1)) = 0.5
		_Amount ("Wave Amount", Range(0, 1)) = 0.6
		_Scale ("Scale", Range(0, 1)) = 0.5
		_Height ("Wave Height", Range(0, 1)) = 0.1
		_Foam ("Foamline Thickness", Range(0, 10)) = 8
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