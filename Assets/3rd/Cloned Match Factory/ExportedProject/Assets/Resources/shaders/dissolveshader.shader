Shader "Unlit/DissolveShader" {
	Properties {
		_MainTex ("Sprite Texture", 2D) = "white" {}
		_DissolveTex ("Dissolve (R)", 2D) = "white" {}
		_DissolveRatio ("Dissolve Ratio", Range(0, 1)) = 1
		_DissolveOffset ("Dissolve Ratio", Range(0, 0.35)) = 0.15
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
}