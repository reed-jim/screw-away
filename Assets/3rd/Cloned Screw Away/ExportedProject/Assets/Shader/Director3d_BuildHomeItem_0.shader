Shader "Director3d/BuildHomeItem" {
	Properties {
		_Fill ("Fill", Float) = 1
		_Cutoff ("Mask Clip Value", Float) = 0.5
		_DissolveDirection ("Dissolve Direction", Float) = 1
		_DissolveColorMode ("_DissolveColorMode", Float) = 1
		[HDR] _Borderwidth ("Border width", Float) = 0
		[HDR] _Bordercolor ("Border color", Vector) = (0,0,0,0)
		[HDR] _Fillcolor ("Fill color", Vector) = (1,0.5514706,0.5514706,1)
		_Noisescale ("Noise scale", Range(0, 20)) = 0
		_Noisespeed ("Noise speed", Vector) = (0,0,0,0)
		[Toggle] _Layernoise ("Layer noise", Float) = 0
		[Toggle] _Worldcoordinates ("World coordinates", Float) = 0
		[Toggle] _Invertmask ("Invert mask", Float) = 0
		[Toggle] _Enablerimlight ("Enable rimlight", Float) = 0
		[HDR] _Rimlightcolor ("Rimlight color", Vector) = (0,0.8344827,1,1)
		_Rimlightpower ("Rimlight power", Float) = 3.5
		_Rimlightscale ("Rimlight scale", Float) = 1
		_Rimlightbias ("Rimlight bias", Float) = 0
		_Wave1amplitude ("Wave1 amplitude", Range(0, 1)) = 0
		_Wave1frequency ("Wave1 frequency", Range(0, 50)) = 0
		_Wave1offset ("Wave1 offset", Float) = 0
		_Wave2amplitude ("Wave2 amplitude", Range(0, 1)) = 0
		_Wave2Frequency ("Wave2 Frequency", Range(0, 50)) = 0
		_Wave2offset ("Wave 2 offset", Float) = 0
		_Albedo ("Albedo", 2D) = "black" {}
		_Normal ("Normal", 2D) = "bump" {}
		_Emission ("Emission", 2D) = "white" {}
		[HDR] _Basecolor ("Base color", Vector) = (0,0,0,1)
		_Emissiontexspeed ("Emission tex speed", Vector) = (0,0,0,0)
		_Emissiontextiling ("Emission tex tiling", Vector) = (1,1,0,0)
		_Mainoffset ("Mainoffset", Vector) = (0,0,0,0)
		_Maintiling ("Maintiling", Vector) = (1,1,0,0)
		[Toggle] _Activatesecondaryemission ("Activate secondary emission", Range(0, 1)) = 0
		_Secondaryemission ("Secondary emission", 2D) = "black" {}
		[HDR] _Secondaryemissioncolor ("Secondary emission color", Vector) = (1,0.9724138,0,1)
		_Secondaryemissionspeed ("Secondary emission speed", Vector) = (0,0,0,0)
		_Secondaryemissiontiling ("Secondary emission tiling", Vector) = (1,1,0,0)
		_SecondaryEmissionNoiseDesaturation ("SecondaryEmissionNoiseDesaturation", Range(0, 1)) = 0
		_SecondaryEmissionDesaturation ("SecondaryEmissionDesaturation", Range(0, 1)) = 0
		_Secondaryemissionnoise ("Secondary emission noise", 2D) = "white" {}
		_Noisetexspeed ("Noise tex speed", Vector) = (0,0,0,0)
		_Noisetextiling ("Noise tex tiling", Vector) = (1,1,0,0)
		_Noisetexopacity ("Noise tex opacity", Range(0, 1)) = 1
		_Specular ("Specular", 2D) = "black" {}
		_Smoothness ("Smoothness", Range(0, 1)) = 0
		_Occlusion ("Occlusion", 2D) = "white" {}
		_Bordertexture ("Border texture", 2D) = "white" {}
		[Toggle] _Tintinsidecolor ("Tint inside color", Range(0, 1)) = 1
		[Toggle] _Activateemission ("Activate emission", Range(0, 1)) = 0
		[HideInInspector] _texcoord ("", 2D) = "white" {}
		[HideInInspector] __dirty ("", Float) = 1
		_Color ("Albedo Color", Vector) = (1,1,1,1)
		_DiffuseColor ("Diffuse Color", Vector) = (1,1,1,1)
		_BorderWorldY ("BorderWorldY", Float) = 0
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		fixed4 _Color;
		struct Input
		{
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			o.Albedo = _Color.rgb;
			o.Alpha = _Color.a;
		}
		ENDCG
	}
	Fallback "Diffuse"
	//CustomEditor "AdultLink.VerticalDissolveEditor"
}