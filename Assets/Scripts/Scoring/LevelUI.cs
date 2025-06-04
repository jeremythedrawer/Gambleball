using TMPro;
using UnityEngine;

public class LevelUI : MonoBehaviour
{
    public static LevelUI Instance { get; private set; }

    private TextMeshPro levelUIText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        levelUIText = GetComponent<TextMeshPro>();
    }

    private void Update()
    {
        int levelNumber = LevelManager.instance.currentLevelIndex + 1;
        levelUIText.text = "Level: " + levelNumber.ToString();
    }
}
