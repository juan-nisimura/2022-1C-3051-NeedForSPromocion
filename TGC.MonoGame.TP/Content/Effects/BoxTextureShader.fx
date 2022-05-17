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

#define PI 3.141592653589

float4x4 World;
float4x4 View;
float4x4 Projection;

texture Texture;

sampler2D textureSampler = sampler_state
{
	Texture = (Texture);
	MagFilter = Linear;
	MinFilter = Linear;
	AddressU = Wrap;
	AddressV = Wrap;
};

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float2 TextureCoordinate : TEXCOORD0;
	float3 Normal : NORMAL0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float2 TextureCoordinate : TEXCOORD0;
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

    // Get how parallel the normal of this point is to the X plane
    float xAlignment = abs(dot(input.Normal, float3(1, 0, 0)));
    // Same for the Y plane
    float zAlignment = abs(dot(input.Normal, float3(0, 0, 1)));

    // Use the world position as texture coordinates 
    // Choose which coordinates we will use based on our normal
    float2 zPlane = lerp(worldPosition.xz, worldPosition.xy, zAlignment);
    float2 resultPlane = lerp(zPlane, worldPosition.zy, xAlignment);

    // Propagamos las coordenadas de textura
    output.TextureCoordinate = resultPlane;

    return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
	return tex2D(textureSampler, input.TextureCoordinate / 75);
}

technique BasicColorDrawing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};
