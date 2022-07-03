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
float4x4 View;
float4x4 Projection;
float4x4 InverseTransposeWorld;

float3 ambientColor; // Light's Ambient Color
float3 specularColor; // Light's Specular Color
float KAmbient; 
float KSpecular;
float3 carPosition;
float3 frontCarPosition;
float Alcance;
float VarAngulo;//mayor = menos angulo


texture Texture;
sampler2D textureSampler = sampler_state
{
    Texture = (Texture);
    MagFilter = Linear;
    MinFilter = Linear;
    MipFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
};

struct VertexShaderInput
{
	float4 Position : POSITION0;
    float4 Normal : NORMAL;
    float2 TextureCoordinates : TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
    float2 TextureCoordinates : TEXCOORD0;
    float4 WorldPosition : TEXCOORD1;
    float4 Normal : TEXCOORD2;    
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
	VertexShaderOutput output = (VertexShaderOutput)0;

    // Model space to World space
    float4 worldPosition = mul(input.Position, World);
    // World space to View space
    float4 viewPosition = mul(worldPosition, View);
	// View space to Projection space
    output.Position = mul(viewPosition, Projection);
    output.WorldPosition = mul(input.Position, World);
    output.Normal = mul(input.Normal, InverseTransposeWorld);
    output.TextureCoordinates = input.TextureCoordinates;
	
	return output;
}

VertexShaderOutput TreeTrunkVS(in VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput) 0;

    // Model space to World space
    float4 worldPosition = mul(input.Position, World);
    // World space to View space
    float4 viewPosition = mul(worldPosition, View);
	// View space to Projection space
    output.Position = mul(viewPosition, Projection);
    output.WorldPosition = mul(input.Position, World);
    output.Normal = mul(input.Normal, InverseTransposeWorld);
    output.TextureCoordinates = float2(asin(input.Normal.x) / PI + 0.5, frac(input.Position.y / 100)) / 5;
	
    return output;
}
VertexShaderOutput RampVS(in VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput) 0;
   // Model space to World space
    float4 worldPosition = mul(input.Position, World);
    // World space to View space
    float4 viewPosition = mul(worldPosition, View);
	// View space to Projection space
    output.Position = mul(viewPosition, Projection);
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
    // Model space to World space
    float4 worldPosition = mul(input.Position, World);
    // World space to View space
    float4 viewPosition = mul(worldPosition, View);
	// View space to Projection space
    output.Position = mul(viewPosition, Projection);
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
    // Model space to World space
    float4 worldPosition = mul(input.Position, World);
    // World space to View space
    float4 viewPosition = mul(worldPosition, View);
	// View space to Projection space
    output.Position = mul(viewPosition, Projection);
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

VertexShaderOutput WallVS(in VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput) 0;
    // Model space to World space
    float4 worldPosition = mul(input.Position, World);
    // World space to View space
    float4 viewPosition = mul(worldPosition, View);
	// View space to Projection space
    output.Position = mul(viewPosition, Projection);
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
    float3 targetPosition = input.WorldPosition.xyz;
    float distance = length(targetPosition - carPosition);
    float isInRange = smoothstep(-Alcance, 0.0, -distance);
    
    float angle = acos(dot(normalize(frontCarPosition - carPosition), normalize(targetPosition - carPosition)));
    float isInAngle = smoothstep(0.0, angle * VarAngulo, 0.3);
    
    float isInHeigth = smoothstep(-40.0, 10.0, carPosition.y - targetPosition.y);
    
	// Get the texture texel
    float4 texelColor = tex2D(textureSampler, input.TextureCoordinates);

    float3 specularLight = KSpecular * specularColor * isInRange * isInAngle * isInHeigth;
    
    float4 finalColor = float4(saturate(ambientColor * KAmbient) * texelColor.rgb + specularLight, texelColor.a);
    return finalColor;


}


float4 FloorPS(VertexShaderOutput input) : COLOR
{
    float3 targetPosition = input.WorldPosition.xyz;
    float distance = length(targetPosition - carPosition);
    float isInRange = smoothstep(-Alcance, 0.0, -distance);
    
    float angle = acos(dot(normalize(frontCarPosition-carPosition), normalize(targetPosition-carPosition)));
    float isInAngle = smoothstep(0.0,angle*VarAngulo,0.3);
    
    float isInHeigth = smoothstep(-40.0, 10.0, carPosition.y -  targetPosition.y );
    
	// Get the texture texel
    float2 coordinates = input.TextureCoordinates;
    coordinates = coordinates * 7;
    float4 texelColor = tex2D(textureSampler, coordinates);

    float3 specularLight = KSpecular * specularColor * isInRange * isInAngle * isInHeigth;
    
    float4 finalColor = float4(saturate(ambientColor * KAmbient) * texelColor.rgb + specularLight, texelColor.a);
    return finalColor;

}
float4 BridgeFloorPS(VertexShaderOutput input) : COLOR
{
    float3 targetPosition = input.WorldPosition.xyz;
    float distance = length(targetPosition - carPosition);
    float isInRange = smoothstep(-Alcance, 0.0, -distance);
    
    float angle = acos(dot(normalize(frontCarPosition - carPosition), normalize(targetPosition - carPosition)));
    float isInAngle = smoothstep(0.0, angle * VarAngulo, 0.3);
    
    float isInHeigth = smoothstep(-40.0, 10.0, carPosition.y - targetPosition.y);
    
	// Get the texture texel
    float2 coordinates = input.TextureCoordinates;
    coordinates = coordinates * 7;
    float4 texelColor = tex2D(textureSampler, input.TextureCoordinates / 75);

    float3 specularLight = KSpecular * specularColor * isInRange * isInAngle * isInHeigth;
    
    float4 finalColor = float4(saturate(ambientColor * KAmbient) * texelColor.rgb + specularLight, texelColor.a);
    return finalColor;
}

VertexShaderOutput BasicVS(in VertexShaderInput input)
{
    // Clear the output
    VertexShaderOutput output = (VertexShaderOutput) 0;
    // Model space to World space
    float4 worldPosition = mul(input.Position, World);
    // World space to View space
    float4 viewPosition = mul(worldPosition, View);
	// View space to Projection space
    output.Position = mul(viewPosition, Projection);

    return output;
}

float4 BasicPS(VertexShaderOutput input) : COLOR
{
    float3 targetPosition = input.WorldPosition.xyz;
    float distance = length(targetPosition - carPosition);
    float isInRange = smoothstep(-Alcance, 0.0, -distance);
    
    float angle = acos(dot(normalize(frontCarPosition - carPosition), normalize(targetPosition - carPosition)));
    float isInAngle = smoothstep(0.0, angle * VarAngulo, 0.3);
    
    float isInHeigth = smoothstep(-40.0, 10.0, carPosition.y - targetPosition.y);
    
	// Get the texture texel
    float4 basicColor = float4(0.0,1.0,0.0,1.0);

    float3 specularLight = KSpecular * specularColor * isInRange * isInAngle * isInHeigth;
    
    float4 finalColor = float4(saturate(ambientColor * KAmbient) * basicColor.rgb + specularLight, basicColor.a);
    return finalColor;
}


technique Floor
{
	pass Pass0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL FloorPS();
	}
};

technique TreeTrunk
{
    pass Pass0
    {
        VertexShader = compile VS_SHADERMODEL TreeTrunkVS();
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
};

technique TreeTop
{
    pass Pass0
    {
        VertexShader = compile VS_SHADERMODEL BasicVS();
        PixelShader = compile PS_SHADERMODEL BasicPS();
    }
};

technique Ramp
{
    pass P0
    {
        VertexShader = compile VS_SHADERMODEL RampVS();
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
};

technique BridgeFloor
{
    pass P0
    {
        VertexShader = compile VS_SHADERMODEL BridgeFloorVS();
        PixelShader = compile PS_SHADERMODEL BridgeFloorPS();
    }
};

technique Box
{
    pass P0
    {
        VertexShader = compile VS_SHADERMODEL BoxVS();
        PixelShader = compile PS_SHADERMODEL BridgeFloorPS();
    }
};

technique Wall
{
    pass P0
    {
        VertexShader = compile VS_SHADERMODEL BoxVS();
        PixelShader = compile PS_SHADERMODEL BridgeFloorPS();
    }
};