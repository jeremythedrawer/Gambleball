using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Collections;

public class DaySelection : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public Image image;
    public int loadSceneIndex;

    public bool unlocked => loadSceneIndex <= StatsManager.instance.daysUnlocked;

    private bool isPressed = false;
    private bool isHovered = false;

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
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!unlocked) return;

        isPressed = false;
        image.color = isHovered ? Color.green : Color.white;
        GameModeManager.instance.currentDayIndex = loadSceneIndex;
        GlobalVolumeController.instance.ToggleCRT(loadSceneIndex);
    }
}
