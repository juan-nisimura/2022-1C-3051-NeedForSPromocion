#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float4x4 World;
float4x4 View;
float4x4 Projection;
float3 DiffuseColor;


texture baseTexture;
sampler2D textureSampler = sampler_state
{
    Texture = (baseTexture);
    MagFilter = Linear;
    MinFilter = Linear;
    MipFilter = Linear;
    AddressU = Border;
    AddressV = Border;
};

texture bloomTexture;
sampler2D bloomTextureSampler = sampler_state
{
    Texture = (bloomTexture);
    MagFilter = Linear;
    MinFilter = Linear;
    AddressU = Clamp;
    AddressV = Clamp;
};
    
struct VertexShaderInput
{
    float4 Position : POSITION0;
    float2 TextureCoordinates : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float2 TextureCoordinates : TEXCOORD0;
};


VertexShaderOutput MainVS(in VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput) 0;
    // Model space to World space
    float4 worldPosition = mul(input.Position, World);
    // World space to View space
    float4 viewPosition = mul(worldPosition, View);
	// View space to Projection space
    output.Position = mul(viewPosition, Projection);
    output.TextureCoordinates = input.TextureCoordinates;
	
    return output;
}

float4 BloomPS(VertexShaderOutput input) : COLOR
{
    float4 color = tex2D(textureSampler, input.TextureCoordinates);
    
    float distanceToTargetColor = distance(color.rgb, float3(0.0, 0.0, 0.0));
    
    float filter = step(distanceToTargetColor, 1);
    
    return float4(color.rgb * filter, 1);
}

float4 BloomPrimitivePS(VertexShaderOutput input) : COLOR
{
    float4 color = tex2D(textureSampler, input.TextureCoordinates);
    
    float distanceToTargetColor = distance(color.rgb, float3(1.0, 1.0, 0.0));
    
    float filter = step(distanceToTargetColor, 1);
    
    return float4(color.rgb * filter, 1) * 0.0000001 + float4(DiffuseColor, 1);
}

VertexShaderOutput PostProcessVS(in VertexShaderInput input)
{
    VertexShaderOutput output;
    output.Position = input.Position;
    output.TextureCoordinates = input.TextureCoordinates;
    return output;
}

float4 BloomIntegratePS(in VertexShaderOutput input) : COLOR
{    
    float4 bloomColor = tex2D(bloomTextureSampler, input.TextureCoordinates);
    float4 sceneColor = tex2D(textureSampler, input.TextureCoordinates);
    
    return sceneColor * 0.8 + bloomColor;
}

technique BloomPass
{
    pass Pass0
    {
        VertexShader = compile VS_SHADERMODEL MainVS();
        PixelShader = compile PS_SHADERMODEL BloomPS();
    }
};

technique BloomPassPrimitive
{
    pass Pass0
    {
        VertexShader = compile VS_SHADERMODEL MainVS();
        PixelShader = compile PS_SHADERMODEL BloomPrimitivePS();
    }
};


technique Integrate
{
    pass Pass0
    {
        VertexShader = compile VS_SHADERMODEL PostProcessVS();
        PixelShader = compile PS_SHADERMODEL BloomIntegratePS();
    }
};