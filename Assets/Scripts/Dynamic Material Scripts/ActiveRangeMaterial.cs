using UnityEngine;

public class ActiveRangeMaterial : MaterialManager
{
    public static Color color { get; set; }
    public static readonly int colorId = Shader.PropertyToID("_Color");
 
    public override void Start()
    {
        base.Start();
        color = Color.green;
    }

    private void Update()
    {
        UpdateMaterial();

        int attempts = GameManager.Instance.attempts;

        if (attempts == 3)
        {
            color = Color.green;
        }
        else if (attempts == 2)
        {
            color = Color.yellow;
        }
        else
        {
            color = Color.red;
        }

        material.SetVector("_Color", color);
    }

    public override void UpdateMaterial()
    {
        if (material != null)
        {
            objectRenderer.GetPropertyBlock(mpb);
            mpb.SetColor(colorId, color);
            objectRenderer.SetPropertyBlock(mpb);
        }
    }
}
