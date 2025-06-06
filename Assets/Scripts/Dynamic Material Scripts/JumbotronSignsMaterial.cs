using System.Collections;
using UnityEngine;

public class JumbotronSignsMaterial : MaterialManager
{
    public enum AnimationType
    { 
        Repeat,
        PingPong
    }

    [SerializeField] private AnimationType animationType;

    public float animationCycleTime = 1;
    public int animationCycles = 3;

    public bool onOff {  get; set; }

    private float time;
    private readonly int timeID = Shader.PropertyToID("_time");
    public override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        UpdateMaterial();

        if (onOff)
        {
            PlayAnimation();
        }
    }


    private void PlayAnimation()
    {
        switch (animationType)
        {
            case AnimationType.Repeat:
            {
                StartCoroutine(Repeating());
            }
            break;

            case AnimationType.PingPong:
            {
                StartCoroutine(PingPonging());
            }
            break;
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

    private IEnumerator Repeating()
    {
        onOff = false;
        int currentCycle = 0;

        while (currentCycle < animationCycles)
        {
            float elapsedTime = 0;
            while (elapsedTime < animationCycleTime)
            {
                elapsedTime += Time.deltaTime;

                time = elapsedTime / animationCycleTime;

                yield return null;
            }

            time = 0;
            currentCycle++;
            yield return null;
        }
    }

    private IEnumerator PingPonging()
    {
        onOff = false;
        int currentCycle = 0;

        while (currentCycle < animationCycles)
        {
            float elapsedTime = 0;
            while(elapsedTime < animationCycleTime)
            {
                elapsedTime += Time.deltaTime;

                time = Mathf.Sin((elapsedTime /  animationCycleTime) * Mathf.PI);
                yield return null;

            }

            time = 0;
            currentCycle++;
            yield return null;
        }
    }
}
