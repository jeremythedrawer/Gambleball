#include "Assets/Shaders/HLSL/NoiseFunctions.hlsl"

float3 QuantizedScreenCoords(float2 screenPosCoords, float scale, float screenHeight)
{
    float raw = (1 / pow(floor(scale), 2)) * screenHeight;
    float rawScale = raw * screenPosCoords.y;
    float2 rowIDCoords = float2(screenPosCoords.x, round(rawScale) / raw);
    
    return float3(rawScale, rowIDCoords.x, rowIDCoords.y);
}

float3 CRTCoordsAndSDF(float2 p, float2 screenParams, float vignetteWidth, float warpOffset)
{
    float2 centredUV = p * 2 - 1;
    float2 warpUV = centredUV.yx / warpOffset;
    float2 uv = centredUV + centredUV * warpUV * warpUV;
    uv = uv * 0.5 + 0.5;
    
    if (uv.x <= 0.0f || 1.0f <= uv.x || uv.y <= 0.0f || 1.0f <= uv.y) uv = 0;
    
    float2 vignette = vignetteWidth / screenParams.xy;
    vignette = smoothstep(0, vignette, 1 - abs(uv * 2.0f - 1.0f));
    
    float vignetteSDF = saturate(vignette.x * vignette.y);
    
    return float3(uv, vignetteSDF);
}

float SignalSDF(float2 p, float time, float range, float freq, float mag ,float bias)
{
    float mask = 1 - saturate(range * distance(p.g, 1 - frac(time * 0.5)));
    
    float sinIn = freq * p.g;

    float b = 1 + (mask * mag * sin(sinIn));

    float wave = 1 - saturate(distance(p.r, b));
    
    float flooredTime = floor(time * 10);
    float normRandRange = Hash21(flooredTime.xx);
    float flicker = round(bias * normRandRange);
    
    float t = mask * wave * flicker;
    
    float sdf = lerp(1, 0.9, t);
    return sdf;
}

float ScanLines(float rawScale, float colValue, float scanLineFallOff)
{
    return colValue * pow(distance(frac(rawScale), 0.5), scanLineFallOff);
}

float3 YIQColorGrading(float3 color, float cyanToRed, float magentaToGreen, float brightness)
{
    float3x3 RGBToYIQ = float3x3(0.299, 0.596, 0.211,
                                 0.587, -0.274, -0.523,
                                 0.114, -0.322, 0.312);
    
    float3x3 YIQToRGB = float3x3(1, 1, 1,
                                 0.956, -0.272, -1.106,
                                 0.621, -0.647, 1.703);
    
    float3 colParams = float3(brightness, cyanToRed, magentaToGreen);
    float3 crtConversion = float3(0.9, 1.1, 1.5);
    
    float3 final = mul(color, RGBToYIQ);
    final = colParams + (crtConversion * final);
    
    return mul(final, YIQToRGB);
}

float3 CRTColorCorrection(float3 col, float bloom, float3 tint)
{
    float3 luminValues = float3(0.21, 0.71, 0.07);
    return tint + (col * bloom * (col / max(dot(luminValues, col), 0.001)));
}