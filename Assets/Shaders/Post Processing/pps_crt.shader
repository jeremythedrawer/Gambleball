Shader "Unlit/pps_crt"
{
     HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"
        #include "Assets/Shaders/HLSL/CRTFunctions.hlsl"
        #include "Assets/Shaders/HLSL/HelperShaderFunctions.hlsl"

        float3 _tint;
        float _brightness;
        float _cyanToRed;
        float _magentaToGreen;
        float _bloom;
        float _chromaticAberationIntensity;
        float _scanLineScale;
        float _warpOffset;
        float _vignetteWidth;
        float _scanLineFallOff;
        float _signalRange;
        float _signalMagnitude;
        float _signalFrequency;
        float _flickerBias;

        SamplerState point_clamp_sampler;

        float4 calc(Varyings input) : SV_Target
        {
            float3 quantUV = QuantizedScreenCoords(input.texcoord, _scanLineScale, _ScreenParams.y);

            float signalSDF = SignalSDF(quantUV.yz, _Time.y, _signalRange, _signalFrequency, _signalMagnitude, _flickerBias);

            quantUV.yz *= signalSDF;

            float3 crtUV = CRTCoordsAndSDF(quantUV.yz, _ScreenParams, _vignetteWidth, _warpOffset);
            float chromABias = length(crtUV * 2 - 1) * _chromaticAberationIntensity;

            int offset = 1;

            float3 chromCol = float3(0,0,0);
            float4 oblit = SAMPLE_TEXTURE2D_X(_BlitTexture, point_clamp_sampler, crtUV);
            for (int i = -offset; i <= offset; i++)
            {
                float o = chromABias * i;
                float2 uv = crtUV.xy + o;

                float4 blit = SAMPLE_TEXTURE2D_X(_BlitTexture, point_clamp_sampler, uv);

                float3 col = YIQColorGrading(blit, _cyanToRed, _magentaToGreen, _brightness);
                
                if (i== -offset)
                {
                    chromCol.x += col.x;
                }
                else if (i == 0)
                {
                    chromCol.y += col.y;
                }
                else
                {
                    chromCol.z += col.z;
                }
            }

            chromCol = HueOffset(chromCol, signalSDF);
            //return float4(chromCol,1);

            float3 crtCol = CRTColorCorrection(chromCol, _bloom, _tint);
            float crtColValue = RGBToHSV(crtCol).z;

            float scanLines = ScanLines(quantUV.x, crtColValue, _scanLineFallOff);
            float3 final = scanLines * crtCol * signalSDF * crtUV.z;
            return float4(final, 1);


        }
    ENDHLSL

    SubShader {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }
        LOD 100
        ZWrite Off
        Cull Off

        Pass { // pass 0
            Name "CalculateCRTPass"
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment calc
            ENDHLSL
        }
    }
}
