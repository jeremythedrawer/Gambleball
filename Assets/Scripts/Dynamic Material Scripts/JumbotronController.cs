using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class JumbotronController : MonoBehaviour
{
    public JumbotronSignsMaterial fromDowntown;
    public JumbotronSignsMaterial onFire;
    public JumbotronSignsMaterial moneyBall;
    public JumbotronSignsMaterial attemptBoost;
    public AudioSource audioSource;

    [Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
    }

    public List<Sound> sounds;


    private Dictionary<string, AudioClip> soundsDict;

    private void Awake()
    {
        soundsDict = new Dictionary<string, AudioClip>();
        foreach (Sound sound in sounds)
        {
            if (!soundsDict.ContainsKey(sound.name))
            {
                soundsDict.Add(sound.name, sound.clip);
            }
        }
    }

    private void OnEnable()
    {
        BallSpawner.onInBasket += ChooseSign; 
    }
    private void OnDisable()
    {
        BallSpawner.onInBasket -= ChooseSign;
    }

    private void ChooseSign()
    {
        if (BallSpawner.instance.type == BallType.Moneyball)
        {
            moneyBall.onOff = true;
            PlaySFX("moneyball");
        }
        else if (BallSpawner.instance.type == BallType.AttemptBoost)
        {
            attemptBoost.onOff = true;
            PlaySFX("attemptBoost");
        }
        else if (StatsManager.instance.onFire)
        {
            onFire.onOff = true;
            PlaySFX("onFire");
        }
        else if (StatsManager.instance.fromDowntown)
        {
            fromDowntown.onOff = true;
            PlaySFX("fromDowntown");
        }
    }

    public void PlaySFX(string name)
    {
        if (soundsDict.TryGetValue(name, out AudioClip clip))
        {
            audioSource.pitch = UnityEngine.Random.Range(1.2f, 0.8f);
            audioSource.volume = UnityEngine.Random.Range(0.8f, 1f);
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.Log($"SFX `{name}` not found!");
        }
    }
}
