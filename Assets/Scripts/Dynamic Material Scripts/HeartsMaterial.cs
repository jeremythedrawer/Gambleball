using System.Collections;
using UnityEngine;

public class HeartsMaterial : MaterialManager
{
    public bool lifeLost {  get; set; }
    private static readonly int lifeLostId = Shader.PropertyToID("_lifeLost");

    public float alpha { get;  set; } = 0;
    private static readonly int alphaId = Shader.PropertyToID("_alpha");

    private float appearTime = 3f;
    private float currentTime;
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
            mpb.SetInt(lifeLostId, lifeLost ? 1 : 0);
            mpb.SetFloat(alphaId, alpha);
            objectRenderer.SetPropertyBlock(mpb);
        }
    }

    public void ShowHeart()
    {
        StartCoroutine(ShowingHeart());
    }
    
    private IEnumerator ShowingHeart()
    {
        float t = 0;
        while (t <= 1)
        {
            currentTime += Time.deltaTime;
            t = currentTime / appearTime;
            alpha = Mathf.Lerp(0, 1, t);
            yield return null;
        }
        alpha = 1;
        currentTime = 0;
    }
}
