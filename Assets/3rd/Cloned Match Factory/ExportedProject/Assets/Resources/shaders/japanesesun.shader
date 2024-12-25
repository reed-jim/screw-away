Shader "Peak/JapaneseSun" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_InnerColor ("Inner Color", Vector) = (1,1,1,1)
		_MiddleColor ("Middle Color", Vector) = (1,1,1,0.85)
		_OuterColor ("Outer Color", Vector) = (1,1,1,0.6)
		_ColorMiddlePoint ("Color Middle Point", Range(0.001, 1)) = 0.535
		_InnerColorBias ("Inner Color Bias", Range(0, 1)) = 0.521
		_OuterColorBias ("Outer Color Bias", Range(0, 1)) = 0.281
		_Dimensions ("Dimensions(x,y,w,h)", Vector) = (0,0,1,1)
		_Splits ("Ray Count", Float) = 18
		_RayThickness ("Ray Thickness", Range(0, 5)) = 1
		_Rotation ("Rotation", Range(1, 1.5)) = 0
		[TOGGLE] _EnableRotation ("Enable Rotation", Float) = 0
		_RotationSpeed ("Rotation Speed", Float) = 0
		[TOGGLE] _CircularClipping ("Enable Circular Clipping", Float) = 0
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
	Fallback "Sprites/Default"
}