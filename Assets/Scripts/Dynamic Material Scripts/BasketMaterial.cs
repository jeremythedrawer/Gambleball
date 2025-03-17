using UnityEngine;
using System.Collections;
public class BasketMaterial : MaterialManager
{
    public Color scoreColor {  get; set; } = Color.white;
    private float lerpTime = 0.5f;

    private MaterialPropertyBlock mpb;
    private static readonly int scoreColorID = Shader.PropertyToID("_ScoreColor");
    public override void Start()
    {
        base.Start();
        mpb = new MaterialPropertyBlock();
    }

    private void OnEnable()
    {
        scoreColor = Color.white;
        UpdateMaterial();
    }
    private void Update()
    {
        UpdateMaterial();

        if (scoreColor == Color.green)
        {
            StartCoroutine(ChangingColorToWhite());
        }
    }

    private void UpdateMaterial()
    {
        if (material != null)
        {
            objectRenderer.GetPropertyBlock(mpb);
            mpb.SetColor(scoreColorID, scoreColor);
            objectRenderer.SetPropertyBlock(mpb);
        }
    }
    private IEnumerator ChangingColorToWhite()
    {
        float elapsedTime = 0;
        Color cachedScoreColor = scoreColor;
        while (elapsedTime < lerpTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / lerpTime;
            t = Mathf.Pow(t, 2);
            scoreColor = Color.Lerp(cachedScoreColor, Color.white, t);
            yield return null;
        }
    }
}
