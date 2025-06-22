using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

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
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

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

    public Coroutine ToggleCRT(int sceneIndex)
    {
        return StartCoroutine(TogglingCRT(sceneIndex));
    }

    private IEnumerator TogglingCRT(int sceneIndex)
    {
        yield return TurningOffCRT();
        AsyncOperation sceneLoad = SceneManager.LoadSceneAsync(sceneIndex);
        yield return new WaitUntil(() => sceneLoad.isDone);

        if (!Bird.isDead)
        {
            if (sceneIndex > 0)
            {
                AudioManager.instance.PlayMusic("playTheme");
            }
            else
            {
                AudioManager.instance.PlayMusic("menuTheme");
            }
        }
        yield return TurningOnCRT();
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
            AudioManager.instance.musicAudioSource.volume = t;
            yield return null;
        }
        crtVolume.warpOffset.value = 5;
    }

    private IEnumerator TurningOffCRT()
    {
        yield return new WaitUntil(() => crtVolume != null);
        float elapsedTime = 0;
        crtVolume.tint.value = skyGradientOverTime.Evaluate(0f);
        while (elapsedTime < turnOnTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / turnOnTime;
            crtVolume.warpOffset.value = Mathf.Lerp(5, 0, t);
            float volume = 1 - t;
            AudioManager.instance.musicAudioSource.volume = volume;
            yield return null;
        }
        crtVolume.warpOffset.value = 0;
    }


}
