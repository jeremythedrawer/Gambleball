using UnityEngine;

public class BallAudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip score;
    public AudioClip miss;

    private void OnEnable()
    {
        BallSpawner.onInBasket += PlayScore;
        BallSpawner.onPlayerNotScored += PlayMiss;
    }

    private void OnDisable()
    {
        BallSpawner.onInBasket -= PlayScore;
        BallSpawner.onPlayerNotScored -= PlayMiss;
    }

    private void PlayScore()
    {
        audioSource.pitch = Random.Range(1.1f, 0.9f);
        audioSource.volume = Random.Range(0.4f, 0.5f);
        audioSource.PlayOneShot(score);
    }

    private void PlayMiss()
    {
        audioSource.pitch = Random.Range(1.1f, 0.9f);
        audioSource.volume = Random.Range(0.8f, 1f);
        audioSource.PlayOneShot(miss);
    }
}
