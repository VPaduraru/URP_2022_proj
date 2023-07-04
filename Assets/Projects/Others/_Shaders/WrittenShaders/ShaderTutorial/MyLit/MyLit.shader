Shader "MyLit" {
	Properties {
		[Header(Surface options)]
		[MainColor]_ColorTint("_Tint", Color) = (1,1,1,1)
		[MainTexture] _ColorMap("Color", 2D) = "white" {}
	}

	SubShader {
		Tags{"RenderPipeline"="UniversalPipeline"}

		Pass {
			Name "ForwardLit"
			Tags{"LightMode"="UniversalForward"}

			HLSLPROGRAM
			#pragma vertex Vertex
			#pragma fragment Fragment

			#include "MyLitForwardLitPass.hlsl"
			ENDHLSL
		}
	}
}