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
        int levelNumber = LevelManager.Instance.levelCount;
        levelUIText.text = "Level: " + levelNumber.ToString();
    }
}
