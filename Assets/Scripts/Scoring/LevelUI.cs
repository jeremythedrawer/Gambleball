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
        int levelNumber = GameManager.Instance.currentLevelIndex;
        levelUIText.text = "Level: " + levelNumber.ToString();
    }

    private void UpdateLevelUIText()
    {
        int levelNumber = GameManager.Instance.currentLevelIndex;

        if (levelNumber > 0)
        {
            levelUIText.text = "Level: " + levelNumber.ToString();
        }
        else
        {
            levelUIText.text = "Level: Tutorial";
        }
    }
}
