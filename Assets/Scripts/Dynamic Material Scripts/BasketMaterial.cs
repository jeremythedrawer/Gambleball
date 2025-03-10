using UnityEngine;
using System.Collections;
public class BasketMaterial : MaterialManager
{
    public Color scoreColor {  get; set; } = Color.white;
    private float lerpTime = 0.5f;
    public override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        material.SetColor("_ScoreColor", scoreColor);
        if (scoreColor == Color.green)
        {
            StartCoroutine(ChangingColorToWhite());
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
