#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

texture baseTexture;
sampler2D textureSampler = sampler_state
{
    Texture = (baseTexture);
    MagFilter = Linear;
    MinFilter = Linear;
    AddressU = Clamp;
    AddressV = Clamp;
};
    
static const int kernel_r = 6;
static const int kernel_size = 13;
static const float Kernel[kernel_size] =
{
    //0.9650329216721958, 0.03438679823918444, 0.000575755860878494, 0.000004507765256612273, 1.643468309719239e-8, 2.7801624650694922e-11, 0, 0, 0, 0, 0, 0, 0
    //0, 2.921270669120573e-16, 3.1064792295428173e-13, 1.3921654241091708e-10, 2.8987596144161485e-8, 0.0000028146222290795345, 0.00012800666215276503, 0.0027407772873145266, 0.027782778354104174, 0.13408627442026594, 0.3096200947512816, 0.3431044588656763, 0.18253476590985201
    //0.002216, 0.008764, 0.026995, 0.064759, 0.120985, 0.176033, 0.199471, 0.176033, 0.120985, 0.064759, 0.026995, 0.008764, 0.002216,
    0.06180845044852108, 0.06636680648322077, 0.07057754978516859, 0.07433522361467029, 0.07754164295423242, 0.08011016368642333, 0.08196961466010702, 0.08306765905329636, 0.08337290551172676, 0.08287559660688833, 0.08159115287746606, 0.07955599276955046, 0.07682724154872862
    //0.07120359495833936, 0.07397412616214726, 0.07632086435798878, 0.07819740313246978, 0.07956597153497354, 0.08039874300759028, 0.0806785936929819, 0.08039874300759028, 0.07956597153497354, 0.07819740313246978, 0.07632086435798878, 0.07397412616214726, 0.07120359495833936
};

float2 screenSize;

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
    VertexShaderOutput output;
    output.Position = input.Position;
    output.TextureCoordinates = input.TextureCoordinates;
    return output;
}

float4 BlurPS(in VertexShaderOutput input) : COLOR
{
    float4 finalColor = float4(0, 0, 0, 1);
    for (int x = 0; x < kernel_size; x++)
        for (int y = 0; y < kernel_size; y++)
        {
            float2 scaledTextureCoordinates = input.TextureCoordinates + float2((float) (x - kernel_r) / screenSize.x, (float) (y - kernel_r) / screenSize.y);
            finalColor += tex2D(textureSampler, scaledTextureCoordinates) * Kernel[x] * Kernel[y];
        }
    return finalColor;
}


float4 BlurHorizontal(in VertexShaderOutput input) : COLOR
{
    float4 finalColor = float4(0, 0, 0, 1);
    for (int i = 0; i < kernel_size; i++)
    {
        float2 scaledTextureCoordinates = input.TextureCoordinates + float2((float) (i - kernel_r) / screenSize.x, 0);
        finalColor += tex2D(textureSampler, scaledTextureCoordinates) * Kernel[i];
    }
    return finalColor;    
}

float4 BlurVertical(in VertexShaderOutput input) : COLOR
{
    float4 finalColor = float4(0, 0, 0, 1);
    for (int i = 0; i < kernel_size; i++)
    {
        float2 scaledTextureCoordinates = input.TextureCoordinates + float2(0, (float) (i - kernel_r) / screenSize.y);
        finalColor += tex2D(textureSampler, scaledTextureCoordinates) * Kernel[i];
    }
    return finalColor;
}


technique Blur
{
    pass Pass0
    {
        VertexShader = compile VS_SHADERMODEL MainVS();
        PixelShader = compile PS_SHADERMODEL BlurPS();
    }
};

technique BlurHorizontalTechnique
{
    pass Pass0
    {
        VertexShader = compile VS_SHADERMODEL MainVS();
        PixelShader = compile PS_SHADERMODEL BlurHorizontal();
    }
};

technique BlurVerticalTechnique
{
    pass Pass0
    {
        VertexShader = compile VS_SHADERMODEL MainVS();
        PixelShader = compile PS_SHADERMODEL BlurVertical();
    }
};