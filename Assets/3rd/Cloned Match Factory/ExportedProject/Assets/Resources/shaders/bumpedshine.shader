Shader "Peak/BumpedShine" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_TextureRect ("TextureRect", Vector) = (0,0,1,1)
		_MainTex2 ("Base (RGB)", 2D) = "white" {}
		_Color ("_Color", Vector) = (1,1,1,1)
		_Distortion ("Distortion", Range(-3, 3)) = 0
		_LightSize ("Light Size", Range(0, 1)) = 0
		_LightIntensity ("Light Intensity", Range(0, 1)) = 0
		_TextureDensity ("Texture Density", Range(0, 1)) = 0
		_LightBumpIntensity ("Light Bump Intensity", Range(-5, 5)) = 0
		_Alpha ("Alpha", Range(0, 1)) = 1
		_Rotation ("Rotation", Range(-1, 1)) = 0
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
	Fallback "Sprites/Default"
}