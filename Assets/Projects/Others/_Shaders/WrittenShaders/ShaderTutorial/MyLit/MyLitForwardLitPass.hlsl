#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

struct Attributes
{
    float3 positionOS : POSITION;
    float2 uv : TEXCOORD0;
    float3 normalOS : NORMAL;
};

struct Interpolators
{
    float4 positionCS : SV_POSITION;
    float2 uv : TEXCOORD0;
    float3 normalWS : TEXCOORD1;
};

float4 _ColorTint;
TEXTURE2D(_ColorMap); SAMPLER(sampler_ColorMap);
float4 _ColorMap_ST;
Interpolators Vertex(Attributes input)
{
    Interpolators output;
    
    VertexPositionInputs posnInputs = GetVertexPositionInputs(input.positionOS);
    VertexNormalInputs normInputs = GetVertexNormalInputs(input.normalOS);
    
    output.positionCS = posnInputs.positionCS;
    output.uv = TRANSFORM_TEX(input.uv, _ColorMap);
    output.normalWS = normInputs.normalWS;
    
    return output;
}

float4 Fragment(Interpolators input) : SV_TARGET
{
    float2 uv = input.uv;
    //Sample the color map
    float4 colorSample = SAMPLE_TEXTURE2D(_ColorMap, sampler_ColorMap, uv);
    //return colorSample * _ColorTint;
    
    InputData lightingInput = (InputData) 0;
    lightingInput.normalWS = normalize(input.normalWS);
    
    SurfaceData surfaceInput = (SurfaceData) 0;
    surfaceInput.albedo = colorSample.rgb * _ColorTint.rgb;
    surfaceInput.alpha = colorSample.a * _ColorTint.a;
    return UniversalFragmentBlinnPhong(lightingInput, surfaceInput);
}