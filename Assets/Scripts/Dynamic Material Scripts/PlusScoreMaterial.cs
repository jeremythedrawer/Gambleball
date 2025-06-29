using System.Collections;
using UnityEngine;

public class PlusScoreMaterial : MaterialManager
{
    public bool usePlusTwo { get; set; } = true;
    private static readonly int usePlusTwoID = Shader.PropertyToID("_usePlusTwo");

    public float alpha { get; set; }
    private static readonly int alphaID = Shader.PropertyToID("_alpha");

    public float vertOffset { get; set; }
    private static readonly int vertOffsetID = Shader.PropertyToID("_vertexOffset");

    private float lerpTime = 0.5f;

    public override void OnEnable()
    {
        base.OnEnable();

        UpdateMaterial();
        alpha = 0;
    }
    private void Update()
    {
        UpdateMaterial();

        if (alpha == 1)
        {
            StartCoroutine(AnimatingMaterial());
        }
    }
    public override void UpdateMaterial()
    {
        if (material != null)
        {
            objectRenderer.GetPropertyBlock(mpb);
            mpb.SetInt(usePlusTwoID, usePlusTwo ? 1 : 0);
            mpb.SetFloat(alphaID, alpha);
            mpb.SetFloat(vertOffsetID, vertOffset);
            objectRenderer.SetPropertyBlock(mpb);
        }
    }

    private IEnumerator AnimatingMaterial()
    {
        float elapsedTime = 0;
        while (elapsedTime < lerpTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / lerpTime;
            t = Mathf.Pow(t, 2);
            alpha = Mathf.Lerp(1f, 0f, t);
            vertOffset = Mathf.Lerp(0f, 0.2f, t);
            yield return null;
        }
        vertOffset = 0f;
    }

}
