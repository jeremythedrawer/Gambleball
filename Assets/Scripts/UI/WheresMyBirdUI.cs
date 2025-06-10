using UnityEngine;
using UnityEngine.UI;

public class WheresMyBirdUI : MonoBehaviour
{
    public Image image;

    private void OnEnable()
    {
        image.enabled = Bird.isDead;
    }
}
