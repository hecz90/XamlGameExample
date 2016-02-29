float2 MousePosition;
float2 Size;
float RadiusSqr;
float Factor;

// сэмплер Color-карты (текстура)
sampler TextureSampler : register(s0);

float4 MagnifierPS(float4 position : SV_Position, float4 color : COLOR0, float2 texCoords : TEXCOORD0) : COLOR
{
	float2 v = texCoords - MousePosition;
	
	float2 worldV = Size * v;

    float f = worldV.x * worldV.x + worldV.y * worldV.y;
	f = step(f, RadiusSqr);
	
	v = texCoords - Factor  * f * v;
            
    return tex2D(TextureSampler, v);
}

technique Deferred
{
    pass Pass0
    {
        PixelShader = compile ps_4_0 MagnifierPS();
    }
}