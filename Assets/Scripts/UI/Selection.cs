using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Selection : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public Image image;
    public AudioSource audioSource;
    public int loadSceneIndex;

    public bool unlocked => loadSceneIndex <= StatsManager.instance.daysUnlocked;

    protected bool isPressed = false;
    protected bool isHovered = false;

    private void Update()
    {
        if (!unlocked)
        {
            image.color = Color.grey;
        }
        else if (!isPressed)
        {
            image.color = isHovered ? Color.green : Color.white;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!unlocked) return;

        isHovered = true;
        if (!isPressed) image.color = Color.green;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!unlocked) return;

        isHovered = false;
        if (!isPressed) image.color = Color.white;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!unlocked) return;

        isPressed = true;
        image.color = Color.red;
        AudioManager.instance.PlaySFX("button");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!unlocked) return;
  
        isPressed = false;
        image.color = isHovered ? Color.green : Color.white;
        audioSource.Play();
        GameModeManager.instance.currentDayIndex = loadSceneIndex;
        GlobalVolumeController.instance.ToggleCRT(loadSceneIndex);
    }
}
