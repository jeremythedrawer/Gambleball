using System.Collections;
using UnityEngine;

public class JumbotronSignsMaterial : MaterialManager
{
    public float flashingTime = 1;
    public int flashingReps = 3;

    private bool on;
    private int currentFlashingReps;
    private float time;
    private readonly int timeID = Shader.PropertyToID("_time");
    public override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        UpdateMaterial();

        if (!on)
        {
            if (currentFlashingReps <= flashingReps)
            {
                StartCoroutine(Flashing());
            }
            else
            {
                currentFlashingReps = 0;
            }
        }
    }

    public override void UpdateMaterial()
    {
        if (material != null)
        {
            objectRenderer.GetPropertyBlock(mpb);
            mpb.SetFloat(timeID, time);
            objectRenderer.SetPropertyBlock(mpb);
        }
    }

    private IEnumerator Flashing()
    {
        on = true;
        float currentTime = 0;
        while (currentTime < flashingTime)
        {
            currentTime += Time.deltaTime;
            time = currentTime / flashingTime;
            yield return null;
        }

        currentFlashingReps++;
        on = false;
        time = 0;
    }
}
