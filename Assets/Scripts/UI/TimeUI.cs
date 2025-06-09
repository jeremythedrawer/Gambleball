using TMPro;
using UnityEngine;

public class TimeUI : MonoBehaviour
{
    private TextMeshPro timeUIText;

    private void OnEnable()
    {
        timeUIText = GetComponent<TextMeshPro>();
    }
    private void Update()
    {
        timeUIText.text = "TIME: " + StatsManager.instance.currentTime.ToString();
    }
}
