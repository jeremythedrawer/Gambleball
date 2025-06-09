using UnityEngine;
using UnityEngine.Rendering;

public class CRTVolumeComponent : VolumeComponent
{

    public ColorParameter tint = new ColorParameter(Color.cyan);
    public ClampedFloatParameter brightness = new ClampedFloatParameter(0.1f, -1, 1);
    public ClampedFloatParameter cyanToRed = new ClampedFloatParameter(-0.16f, -1, 1);
    public ClampedFloatParameter magentaToGreen = new ClampedFloatParameter(-0.04f, -1, 1);
    public FloatParameter bloom = new FloatParameter(0.82f);
    public ClampedFloatParameter chromaticAberationIntensity = new ClampedFloatParameter(0.00125f, 0, 0.01f);

    public IntParameter scanLineScale = new IntParameter(3);
    public FloatParameter warpOffset = new FloatParameter(6.22f);
    public FloatParameter vignetteWidth = new FloatParameter(68.01f);
    public FloatParameter scanLineFallOff = new FloatParameter(2.37f);

    public FloatParameter signalRange = new FloatParameter(27f);
    public ClampedFloatParameter signalMagnitude = new ClampedFloatParameter(1f, 0, 1);
    public FloatParameter signalFrequency = new FloatParameter(96.1f);
    public ClampedFloatParameter flickerBias = new ClampedFloatParameter(0.621f, 0.5f, 1);
}
