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

float Time = 0;
float HealthPercentage = 0.5;

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float2 TextureCoordinate : TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float2 TextureCoordinate : TEXCOORD0;
	float4 Color: COLOR0;
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

	// Propagamos las coordenadas de textura
	output.TextureCoordinate = input.TextureCoordinate;

    return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
	float2 coordinates = input.TextureCoordinate;

    /*
	float esLaSegundaMitad = floor(coordinates.x + 0.5);	// no = 0, si = 1

	// Espejo las coordenadas en x si es la segunda mitad
	coordinates = float2(coordinates.x * (1 - esLaSegundaMitad) + (1 - coordinates.x) * esLaSegundaMitad, coordinates.y);

	coordinates = coordinates * 8;

	float2 cell = floor(coordinates);
	coordinates = frac(coordinates);

	float cuadradoPar = (cell.x + cell.y) % 2;	// par = 0, impar = 1

	float verde = (floor(coordinates.x + coordinates.y) + cuadradoPar) % 2;

    return float4(0.1, verde * 0.9, 0.1, 1);
    */
	float2 distanciaAlCentro = 2 * abs(coordinates - 0.5);

	float radio = pow(distanciaAlCentro.x, 2)  + pow(distanciaAlCentro.y, 2);

	float alpha = floor(radio);

    return float4(1 - radio, 1 - radio, 1 - radio, 0);
}

technique BasicColorDrawing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};