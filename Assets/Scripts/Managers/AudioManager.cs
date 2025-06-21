using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource sfxAudioSource;
    public AudioSource musicAudioSource;

    [Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
    }

    public List<Sound> sounds;


    private Dictionary<string, AudioClip> soundsDict;
    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        soundsDict = new Dictionary<string, AudioClip>();
        foreach (Sound sound in sounds)
        {
            if (!soundsDict.ContainsKey(sound.name))
            {
                soundsDict.Add(sound.name, sound.clip);
            }
        }
    }

    private void Start()
    {
        StaticLoop();
        PlayMusic("menuTheme");
    }

    private Coroutine StaticLoop()
    {
        return StartCoroutine(StaticLooping());
    }

    private IEnumerator StaticLooping()
    {
        while (true)
        {
            float waitTime = UnityEngine.Random.Range(5f, 10f);
            yield return new WaitForSeconds(waitTime);

            PlaySFX("static", new Vector2(0.9f, 1.1f), new Vector2(0.8f, 1f));
        }
    }
    public void PlaySFX(string name, Vector2 pitchRange = default, Vector2 volRange = default)
    {
        if (pitchRange == default) pitchRange = Vector2.one;
        if (volRange == default) volRange = Vector2.one;


        if (soundsDict.TryGetValue(name, out AudioClip clip))
        {
            sfxAudioSource.pitch = UnityEngine.Random.Range(pitchRange.x, pitchRange.y);
            sfxAudioSource.volume = UnityEngine.Random.Range(volRange.x, volRange.y);
            sfxAudioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.Log($"SFX `{name}` not found!");
        }
    }

    public void PlayMusic(string name, bool loop = true)
    {
        if (soundsDict.TryGetValue(name, out AudioClip clip))
        {
            musicAudioSource.clip = clip;
            musicAudioSource.loop = loop;
            musicAudioSource.Play();
        }
        else
        {
            Debug.Log($"SFX `{name}` not found!");
        }
    }
}
