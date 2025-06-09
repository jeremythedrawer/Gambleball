using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class GlobalVolumeController : MonoBehaviour
{
    public static GlobalVolumeController instance;
    public VolumeProfile globalVolume;
    private CRTVolumeComponent crtVolume;

    public float turnOnTime = 1f;
    public float dayTime = 60f;
    public Gradient skyGradientOverTime;

    public float time { private get; set; }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        globalVolume.TryGet<CRTVolumeComponent>(out crtVolume);
        StartCoroutine(TurningOnCRT());
    }

    private void Update()
    {   
        crtVolume.tint.value = skyGradientOverTime.Evaluate(time);
    }

    private IEnumerator TurningOnCRT()
    {
        yield return new WaitUntil(()=> crtVolume != null);
        float elapsedTime = 0;
        crtVolume.tint.value = skyGradientOverTime.Evaluate(0f);
        while (elapsedTime < turnOnTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / turnOnTime;
            crtVolume.warpOffset.value = Mathf.Lerp(0, 5, t);
            yield return null;
        }
        crtVolume.warpOffset.value = 5;
    }

}
