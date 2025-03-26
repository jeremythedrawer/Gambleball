using UnityEngine;

public class ArrowMaterial : MaterialManager
{
    public static bool onOff { get; set; }
    private static readonly int onOffID = Shader.PropertyToID("_onOff");

    public override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        UpdateMaterial();
    }
    public override void UpdateMaterial()
    {
        if (material != null)
        {
            objectRenderer.GetPropertyBlock(mpb);
            mpb.SetInt(onOffID, onOff ? 1 : 0);
            objectRenderer.SetPropertyBlock(mpb);
        }
    }
}
