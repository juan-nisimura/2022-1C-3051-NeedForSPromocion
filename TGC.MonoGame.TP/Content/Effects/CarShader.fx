#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

// Custom Effects - https://docs.monogame.net/articles/content/custom_effects.html
// High-level shader language (HLSL) - https://docs.microsoft.com/en-us/windows/win32/direct3dhlsl/dx-graphics-hlsl
// Programming guide for HLSL - https://docs.microsoft.com/en-us/windows/win32/direct3dhlsl/dx-graphics-hlsl-pguide
// Reference for HLSL - https://docs.microsoft.com/en-us/windows/win32/direct3dhlsl/dx-graphics-hlsl-reference
// HLSL Semantics - https://docs.microsoft.com/en-us/windows/win32/direct3dhlsl/dx-graphics-hlsl-semantics

float4x4 World;
float4x4 View;
float4x4 Projection;
float4x4 InverseTransposeWorld;

float3 eyePosition;

struct VertexShaderInput
{
	float4 Position : POSITION0;
    float3 Normal : NORMAL;
	float2 TextureCoordinate: TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float2 TextureCoordinate: TEXCOORD1;
    float4 WorldPosition : TEXCOORD2;
    float4 Normal : TEXCOORD3;
};

texture ModelTexture;
sampler2D textureSampler = sampler_state
{
	Texture = (ModelTexture);
	MagFilter = Linear;
	MinFilter = Linear;
    MipFilter = Linear;
	AddressU = Border;
	AddressV = Border;
};

texture environmentMap;
samplerCUBE environmentMapSampler = sampler_state
{
    Texture = (environmentMap);
    MagFilter = Linear;
    MinFilter = Linear;
    AddressU = Clamp;
    AddressV = Clamp;
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
    // Clear the output
	VertexShaderOutput output = (VertexShaderOutput)0;
    // Model space to World space
    float4 worldPosition = mul(input.Position, World);
    // World space to View space
    float4 viewPosition = mul(worldPosition, View);	
	// View space to Projection space
    output.Position = mul(viewPosition, Projection);
    output.WorldPosition = mul(input.Position, World);
    output.Normal = mul(float4(normalize(input.Position.xyz), 1.0), InverseTransposeWorld);
    output.TextureCoordinate = input.TextureCoordinate;
	
    return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
	//Normalizar vectores
    float3 normal = normalize(input.Normal.xyz);
    
	// Get the texel from the texture
    float3 baseColor = tex2D(textureSampler, input.TextureCoordinate).rgb;
	
    // Not part of the mapping, just adjusting color
    //baseColor = lerp(baseColor, float3(1, 1, 1), step(length(baseColor), 0.01));
    
	//Obtener texel de CubeMap
    float3 view = normalize(eyePosition.xyz - input.WorldPosition.xyz);
    float3 reflection = reflect(view, normal);
    float3 reflectionColor = texCUBE(environmentMapSampler, reflection).rgb;

    float fresnel = saturate((0.5 - dot(normal, view)));

    //return float4(lerp(baseColor, reflectionColor, fresnel), 1);
    return float4(baseColor * 0.8 + reflectionColor * 0.2, 1.0);

}

technique BasicColorDrawing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};
