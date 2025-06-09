using UnityEngine;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System;
using UnityEngine.Experimental.Rendering;

public class CRTRendererFeature : ScriptableRendererFeature
{
    [SerializeField] private Shader shader;
    [SerializeField] private RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
    [SerializeField] int passEventOrder = 0;
    [SerializeField] internal DefaultCRTSettings settings;

    internal Material material;
    private CRTPass crtPass;
    private RTHandle crtFeatureRTHandle;
    private bool isInitialized = false;

    public override void Create()
    {
        material = CoreUtils.CreateEngineMaterial(shader);

        crtPass = new CRTPass(this);
        crtPass.renderPassEvent = (RenderPassEvent)((int)renderPassEvent + passEventOrder);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (renderingData.cameraData.cameraType != CameraType.Game) return;

        if (crtFeatureRTHandle == null || !isInitialized)
        {
            ReleaseCRTFeatureRTHandle();

            crtFeatureRTHandle = RTHandles.Alloc
            (
                renderingData.cameraData.cameraTargetDescriptor.width,
                renderingData.cameraData.cameraTargetDescriptor.height,
                slices: 1,
                depthBufferBits: DepthBits.None,
                colorFormat: GraphicsFormat.R16G16B16A16_SFloat,
                filterMode: FilterMode.Point,
                wrapMode: TextureWrapMode.Clamp,
                dimension: TextureDimension.Tex2D,
                enableRandomWrite: false,
                useMipMap: false,
                autoGenerateMips: false,
                isShadowMap: false,
                anisoLevel: 1,
                useDynamicScale: true,
                name: "CRTFeatureRTHandle"
            );

            isInitialized = true;
        }

        crtPass.ConfigureInput(ScriptableRenderPassInput.Color);
        renderer.EnqueuePass(crtPass);
    }

    private void ReleaseCRTFeatureRTHandle()
    {
        if (crtFeatureRTHandle != null)
        {
            crtFeatureRTHandle.Release();
            crtFeatureRTHandle = null;
        }
    }

}


public class CRTPass : ScriptableRenderPass
{
    private readonly CRTRendererFeature rendererFeature;

    private static readonly int tintID = Shader.PropertyToID("_tint");
    private static readonly int brightnessID = Shader.PropertyToID("_brightness");
    private static readonly int cyanToRedID = Shader.PropertyToID("_cyanToRed");
    private static readonly int magentaToGreenID = Shader.PropertyToID("_magentaToGreen");
    private static readonly int bloomID = Shader.PropertyToID("_bloom");
    private static readonly int chromaticAberationIntensityID = Shader.PropertyToID("_chromaticAberationIntensity");
    private static readonly int scanLineScaleID = Shader.PropertyToID("_scanLineScale");
    private static readonly int warpOffsetID = Shader.PropertyToID("_warpOffset");
    private static readonly int vignetteWidthID = Shader.PropertyToID("_vignetteWidth");
    private static readonly int scanLineFallOffID = Shader.PropertyToID("_scanLineFallOff");
    private static readonly int signalRangeID = Shader.PropertyToID("_signalRange");
    private static readonly int signalMagnitudeID = Shader.PropertyToID("_signalMagnitude");
    private static readonly int signalFrequencyID = Shader.PropertyToID("_signalFrequency");
    private static readonly int flickerBiasID = Shader.PropertyToID("_flickerBias");
    public CRTPass(CRTRendererFeature feature) => this.rendererFeature = feature;

    private class CRTPassData
    {
        internal Material material;
        internal TextureHandle sourceColor;
        internal CRTVolumeComponent vc;
        internal TextureHandle targetColor;
        internal DefaultCRTSettings defaultSettings;
    }

    public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
    {
        UniversalResourceData resourceData = frameData.Get<UniversalResourceData>();
        UniversalCameraData cameraData = frameData.Get<UniversalCameraData>();

        TextureHandle sourceCamColor = resourceData.cameraColor;
        TextureDesc textDesc = resourceData.cameraColor.GetDescriptor(renderGraph);
        textDesc.name = "CRTTexture";
        textDesc.clearBuffer = false;
        TextureHandle dest = renderGraph.CreateTexture(textDesc);

        CRTPassData passData;
        using (var builder = renderGraph.AddRasterRenderPass<CRTPassData>("CRT Render Pass", out passData))
        {
            passData.targetColor = dest;
            passData.sourceColor = resourceData.cameraColor;
            passData.material = rendererFeature.material;
            passData.vc = VolumeManager.instance.stack.GetComponent<CRTVolumeComponent>();
            passData.defaultSettings = rendererFeature.settings;

            builder.UseTexture(passData.sourceColor); // getting souce color
            builder.SetRenderAttachment(passData.targetColor, 0, AccessFlags.WriteAll); // setting source color via the target that is a copy

            builder.SetRenderFunc((CRTPassData data, RasterGraphContext ctx) => ExecuteCRTPass(passData, ctx));
        }
        resourceData.cameraColor = passData.targetColor;
    }

    private static void ExecuteCRTPass(CRTPassData passData, RasterGraphContext ctx)
    {
        UpdateCRTSettings(passData);
        Blitter.BlitTexture(ctx.cmd, passData.sourceColor, Vector2.one, passData.material, 0);
    }

    private static void UpdateCRTSettings(CRTPassData data)
    {
        Color tint = data.vc.active && data.vc.tint.overrideState ? data.vc.tint.value : data.defaultSettings.tint;
        float brightness = data.vc.active && data.vc.brightness.overrideState ? data.vc.brightness.value : data.defaultSettings.brightness;
        float cyanToRed = data.vc.active && data.vc.cyanToRed.overrideState ? data.vc.cyanToRed.value : data.defaultSettings.cyanToRed;
        float magentaToGreen = data.vc.active && data.vc.magentaToGreen.overrideState ? data.vc.magentaToGreen.value : data.defaultSettings.magentaToGreen;
        float bloom = data.vc.active && data.vc.bloom.overrideState ? data.vc.bloom.value : data.defaultSettings.bloom;
        float chromaticAberationIntensity = data.vc.active && data.vc.chromaticAberationIntensity.overrideState ? data.vc.chromaticAberationIntensity.value : data.defaultSettings.chromaticAberationIntensity;
        int scanLineScale = data.vc.active && data.vc.scanLineScale.overrideState ? data.vc.scanLineScale.value : data.defaultSettings.scanLineScale;
        float warpOffset = data.vc.active && data.vc.warpOffset.overrideState ? data.vc.warpOffset.value : data.defaultSettings.warpOffset;
        float vignetteWidth = data.vc.active && data.vc.vignetteWidth.overrideState ? data.vc.vignetteWidth.value : data.defaultSettings.vignetteWidth;
        float scanLineFallOff = data.vc.active && data.vc.scanLineFallOff.overrideState ? data.vc.scanLineFallOff.value : data.defaultSettings.scanLineFallOff;
        float signalRange = data.vc.active && data.vc.signalRange.overrideState ? data.vc.signalRange.value : data.defaultSettings.signalRange;
        float signalMagnitude = data.vc.active && data.vc.signalMagnitude.overrideState ? data.vc.signalMagnitude.value : data.defaultSettings.signalMagnitude;
        float signalFrequency = data.vc.active && data.vc.signalFrequency.overrideState ? data.vc.signalFrequency.value : data.defaultSettings.signalFrequency;
        float flickerBias = data.vc.active && data.vc.flickerBias.overrideState ? data.vc.flickerBias.value : data.defaultSettings.flickerBias;

        data.material.SetColor(tintID,tint);
        data.material.SetFloat(brightnessID , brightness);
        data.material.SetFloat(cyanToRedID , cyanToRed);
        data.material.SetFloat(magentaToGreenID , magentaToGreen);
        data.material.SetFloat(bloomID , bloom);
        data.material.SetFloat(chromaticAberationIntensityID , chromaticAberationIntensity);
        data.material.SetFloat(scanLineScaleID , scanLineScale);
        data.material.SetFloat(warpOffsetID , warpOffset);
        data.material.SetFloat(vignetteWidthID , vignetteWidth);
        data.material.SetFloat(scanLineFallOffID , scanLineFallOff);
        data.material.SetFloat(signalRangeID , signalRange);
        data.material.SetFloat(signalMagnitudeID , signalMagnitude);
        data.material.SetFloat(signalFrequencyID , signalFrequency);
        data.material.SetFloat(flickerBiasID, flickerBias);
    }
}

[Serializable]
public class DefaultCRTSettings
{
    public Color tint = Color.cyan;
    public float brightness = 0.1f;
    public float cyanToRed = -0.16f;
    public float magentaToGreen = -0.04f;
    public float bloom = 0.82f;
    public float chromaticAberationIntensity = 0.00125f;

    public int scanLineScale = 3;
    public float warpOffset = 6.22f;
    public float vignetteWidth = 68.01f;
    public float scanLineFallOff = 2.37f;

    public float signalRange = 27f;
    public float signalMagnitude = 1f;
    public float signalFrequency = 96.1f;
    public float flickerBias = 0.621f;
}
