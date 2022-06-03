#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

#define PI 3.141592653589

float4x4 WorldViewProjection;
float4x4 World;
float4x4 InverseTransposeWorld;

float3 ambientColor; // Light's Ambient Color
float3 diffuseColor; // Light's Diffuse Color
float3 specularColor; // Light's Specular Color
float KAmbient;
float KDiffuse;
float KSpecular;
float shininess;
float3 lightPosition;
float3 eyePosition; // Camera position

texture baseTexture;
sampler2D textureSampler = sampler_state
{
    Texture = (baseTexture);
    MagFilter = Linear;
    MinFilter = Linear;
    AddressU = Clamp;
    AddressV = Clamp;
};

texture ModelTexture;
sampler2D modelTextureSampler = sampler_state
{
    Texture = (ModelTexture);
    MagFilter = Linear;
    MinFilter = Linear;
    AddressU = Border;
    AddressV = Border;
};

texture floorTexture;
sampler2D floorTextureSampler = sampler_state
{
    Texture = (floorTexture);
    MagFilter = Linear;
    MinFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
};

texture rampTexture;
sampler2D rampTextureSampler = sampler_state
{
    Texture = (rampTexture);
    MagFilter = Linear;
    MinFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
};
texture boxTexture;
sampler2D boxTextureSampler = sampler_state
{
    Texture = (boxTexture);
    MagFilter = Linear;
    MinFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
};

struct VertexShaderInput
{
    float4 Position : POSITION0;
    float4 Normal : NORMAL;
    //float3 Normal3 : NORMAL0;
    float2 TextureCoordinates : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float2 TextureCoordinates : TEXCOORD0;
    float4 WorldPosition : TEXCOORD1;
    float4 Normal : TEXCOORD2;
    //float3 Normal3 : TEXCOORD3;
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput) 0;

    output.Position = mul(input.Position, WorldViewProjection);
    output.WorldPosition = mul(input.Position, World);
    output.Normal = mul(input.Normal, InverseTransposeWorld);
    output.TextureCoordinates = input.TextureCoordinates;
	
    return output;
}
VertexShaderOutput CarVS(in VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput) 0;

    output.Position = mul(input.Position, WorldViewProjection);
    output.WorldPosition = mul(input.Position, World);
    output.Normal = mul(input.Normal, InverseTransposeWorld);
    output.TextureCoordinates = input.TextureCoordinates;
	
    return output;
}
VertexShaderOutput TreeTrunkVS(in VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput) 0;
    output.Position = mul(input.Position, WorldViewProjection);
    output.WorldPosition = mul(input.Position, World);
    output.Normal = mul(input.Normal, InverseTransposeWorld);
	// Propagamos las coordenadas de textura
    output.TextureCoordinates = float2(asin(input.Normal.x) / PI + 0.5, frac(input.Position.y / 100)) / 5;

    return output;
}
VertexShaderOutput RampVS(in VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput) 0;
    output.Position = mul(input.Position, WorldViewProjection);
    output.WorldPosition = mul(input.Position, World);
    output.Normal = mul(input.Normal, InverseTransposeWorld);
    // Get how parallel the normal of this point is to the X plane
    float yAlignment = abs(dot(input.Normal.xyz, float3(0, 1, 0)));
    // Same for the Y plane
    float zAlignment = abs(dot(input.Normal.xyz, float3(0, 0, 1)));

    // Use the world position as texture coordinates 
    // Choose which coordinates we will use based on our normal
    float2 zPlane = lerp(input.Position.zx, input.Position.yx, zAlignment);
    float2 resultPlane = lerp(zPlane, input.Position.zx, yAlignment);

    // Propagamos las coordenadas de textura
    output.TextureCoordinates = resultPlane;

    return output;
}
VertexShaderOutput BridgeFloorVS(in VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput) 0;
    float4 worldPosition = mul(input.Position, World);
    output.Position = mul(input.Position, WorldViewProjection);
    output.WorldPosition = mul(input.Position, World);
    output.Normal = mul(input.Normal, InverseTransposeWorld);

    // Get how parallel the normal of this point is to the X plane
    float xAlignment = abs(dot(input.Normal.xyz, float3(1, 0, 0)));
    // Same for the Y plane
    float zAlignment = abs(dot(input.Normal.xyz, float3(0, 0, 1)));

    // Use the world position as texture coordinates 
    // Choose which coordinates we will use based on our normal
    float2 zPlane = lerp(worldPosition.zx, worldPosition.yx, zAlignment);
    float2 resultPlane = lerp(zPlane, worldPosition.yz, xAlignment);

    // Propagamos las coordenadas de textura
    output.TextureCoordinates = resultPlane;

    return output;
}
VertexShaderOutput BoxVS(in VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput) 0;
    float4 worldPosition = mul(input.Position, World);
    output.Position = mul(input.Position, WorldViewProjection);
    output.WorldPosition = mul(input.Position, World);
    output.Normal = mul(input.Normal, InverseTransposeWorld);

    // Get how parallel the normal of this point is to the X plane
    float xAlignment = abs(dot(input.Normal.xyz, float3(1, 0, 0)));
    // Same for the Y plane
    float zAlignment = abs(dot(input.Normal.xyz, float3(0, 0, 1)));

    // Use the world position as texture coordinates 
    // Choose which coordinates we will use based on our normal
    float2 zPlane = lerp(worldPosition.xz, worldPosition.xy, zAlignment);
    float2 resultPlane = lerp(zPlane, worldPosition.zy, xAlignment);

    // Propagamos las coordenadas de textura
    output.TextureCoordinates = resultPlane;

    return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
    // Base vectors
    float3 lightDirection = normalize(lightPosition - input.WorldPosition.xyz);
    float3 viewDirection = normalize(eyePosition - input.WorldPosition.xyz);
    float3 halfVector = normalize(lightDirection + viewDirection);

	// Get the texture texel
    float4 texelColor = tex2D(textureSampler, input.TextureCoordinates);
    
	// Calculate the diffuse light
    float NdotL = saturate(dot(input.Normal.xyz, lightDirection));
    float3 diffuseLight = KDiffuse/5 * diffuseColor * NdotL;

	// Calculate the specular light
    float NdotH = dot(input.Normal.xyz, halfVector);
    float3 specularLight = sign(NdotL) * KSpecular/5 * specularColor * pow(saturate(NdotH), shininess);
    
    // Final calculation
    float4 finalColor = float4(saturate(ambientColor * KAmbient + diffuseLight) * texelColor.rgb + specularLight, texelColor.a);
    return finalColor;

}
float4 CarPS(VertexShaderOutput input) : COLOR
{
    // Base vectors
    float3 lightDirection = normalize(lightPosition - input.WorldPosition.xyz);
    float3 viewDirection = normalize(eyePosition - input.WorldPosition.xyz);
    float3 halfVector = normalize(lightDirection + viewDirection);

	// Get the texture texel
    float4 texelColor = tex2D(modelTextureSampler, input.TextureCoordinates);
    
	// Calculate the diffuse light
    float NdotL = saturate(dot(input.Normal.xyz, lightDirection));
    float3 diffuseLight = KDiffuse * diffuseColor * NdotL;

	// Calculate the specular light
    float NdotH = dot(input.Normal.xyz, halfVector);
    float3 specularLight = sign(NdotL) * KSpecular * specularColor * pow(saturate(NdotH), shininess / 4);
    
    // Final calculation
    float4 finalColor = float4(saturate(ambientColor * KAmbient + diffuseLight) * texelColor.rgb + specularLight, texelColor.a);
    return finalColor;

}
float4 FloorPS(VertexShaderOutput input) : COLOR
{
    // Base vectors
    float3 lightDirection = normalize(lightPosition - input.WorldPosition.xyz);
    float3 viewDirection = normalize(eyePosition - input.WorldPosition.xyz);
    float3 halfVector = normalize(lightDirection + viewDirection);

	// Get the texture texel
    float2 coordinates = input.TextureCoordinates;
    coordinates = coordinates * 15;
    float4 texelColor = tex2D(floorTextureSampler, coordinates);
    
	// Calculate the diffuse light
    float NdotL = saturate(dot(input.Normal.xyz, lightDirection));
    float3 diffuseLight = KDiffuse * diffuseColor * NdotL;

	// Calculate the specular light
    float NdotH = dot(input.Normal.xyz, halfVector);
    float3 specularLight = sign(NdotL) * KSpecular * specularColor * pow(saturate(NdotH), shininess);
    
    // Final calculation
    float4 finalColor = float4(saturate(ambientColor * KAmbient + diffuseLight) * texelColor.rgb + specularLight, texelColor.a);
    return finalColor;

}
float4 RampPS(VertexShaderOutput input) : COLOR
{
    // Base vectors
    float3 lightDirection = normalize(lightPosition - input.WorldPosition.xyz);
    float3 viewDirection = normalize(eyePosition - input.WorldPosition.xyz);
    float3 halfVector = normalize(lightDirection + viewDirection);

	// Get the texture texel
    float4 texelColor = tex2D(rampTextureSampler, input.TextureCoordinates);
    
	// Calculate the diffuse light
    float NdotL = saturate(dot(input.Normal.xyz, lightDirection));
    float3 diffuseLight = KDiffuse*2 * diffuseColor * NdotL;

	// Calculate the specular light
    float NdotH = dot(input.Normal.xyz, halfVector);
    float3 specularLight = sign(NdotL) * KSpecular*2 * specularColor * pow(saturate(NdotH), shininess/2);
  
    // Final calculation
    float4 finalColor = float4(saturate(ambientColor * KAmbient * 1.2f + diffuseLight) * texelColor.rgb + specularLight, texelColor.a);
    return finalColor;

}
float4 BridgeFloorPS(VertexShaderOutput input) : COLOR
{
    // Base vectors
    float3 lightDirection = normalize(lightPosition - input.WorldPosition.xyz);
    float3 viewDirection = normalize(eyePosition - input.WorldPosition.xyz);
    float3 halfVector = normalize(lightDirection + viewDirection);

	// Get the texture texel
    float4 texelColor = tex2D(rampTextureSampler, input.TextureCoordinates / 75);
    
	// Calculate the diffuse light
    float NdotL = saturate(dot(input.Normal.xyz, lightDirection));
    float3 diffuseLight = KDiffuse * diffuseColor * NdotL;

	// Calculate the specular light
    float NdotH = dot(input.Normal.xyz, halfVector);
    float3 specularLight = sign(NdotL) * KSpecular * specularColor * pow(saturate(NdotH), shininess);
    
    // Final calculation
    float4 finalColor = float4(saturate(ambientColor * KAmbient + diffuseLight) * texelColor.rgb + specularLight, texelColor.a);
    return finalColor;
}
float4 BoxPS(VertexShaderOutput input) : COLOR
{
    // Base vectors
    float3 lightDirection = normalize(lightPosition - input.WorldPosition.xyz);
    float3 viewDirection = normalize(eyePosition - input.WorldPosition.xyz);
    float3 halfVector = normalize(lightDirection + viewDirection);

	// Get the texture texel
    float4 texelColor = tex2D(boxTextureSampler, input.TextureCoordinates / 75);
    
	// Calculate the diffuse light
    float NdotL = saturate(dot(input.Normal.xyz, lightDirection));
    float3 diffuseLight = KDiffuse*10 * diffuseColor * NdotL;

	// Calculate the specular light
    float NdotH = dot(input.Normal.xyz, halfVector);
    float3 specularLight = sign(NdotL) * KSpecular*10 * specularColor * pow(saturate(NdotH), shininess / 2);
    
    // Final calculation
    float4 finalColor = float4(saturate(ambientColor * KAmbient + diffuseLight) * texelColor.rgb + specularLight, texelColor.a);
    return finalColor;
}

technique BasicColorDrawing
{
    pass Pass0
    {
        VertexShader = compile VS_SHADERMODEL MainVS();
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
};

technique CarColorDrawing
{
    pass Pass0
    {
        VertexShader = compile VS_SHADERMODEL CarVS();
        PixelShader = compile PS_SHADERMODEL CarPS();
    }
};

technique FloorColorDrawing
{
    pass Pass0
    {
        VertexShader = compile VS_SHADERMODEL MainVS();
        PixelShader = compile PS_SHADERMODEL FloorPS();
    }
};

technique TreeTrunkColorDrawing
{
    pass P0
    {
        VertexShader = compile VS_SHADERMODEL TreeTrunkVS();
        PixelShader = compile PS_SHADERMODEL RampPS();
    }
};

technique RampColorDrawing
{
    pass P0
    {
        VertexShader = compile VS_SHADERMODEL RampVS();
        PixelShader = compile PS_SHADERMODEL RampPS();
    }
};

technique BridgeFloorColorDrawing
{
    pass P0
    {
        VertexShader = compile VS_SHADERMODEL BridgeFloorVS();
        PixelShader = compile PS_SHADERMODEL BridgeFloorPS();
    }
};

technique BoxColorDrawing
{
    pass P0
    {
        VertexShader = compile VS_SHADERMODEL BoxVS();
        PixelShader = compile PS_SHADERMODEL BoxPS();
    }
};