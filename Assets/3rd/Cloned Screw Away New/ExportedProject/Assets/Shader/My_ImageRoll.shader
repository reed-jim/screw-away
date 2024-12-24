Shader "My/ImageRoll" {
	Properties {
		_MainTex ("Main Tex", 2D) = "white" {}
		_Color ("Tint", Vector) = (1,1,1,1)
		_Width ("Width", Float) = 1
		_Height ("Height", Float) = 1
		_XDistance ("XDistance", Float) = 0
		_YDistance ("YDistance", Float) = 0
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
	Fallback "Transparent/VertexLit"
}