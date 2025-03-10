using TMPro;
using UnityEngine;

public class LevelUI : MonoBehaviour
{
    public static LevelUI Instance { get; private set; }

    private TextMeshPro levelUIText;

    public int levelNumber {  get; set; }

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
        levelUIText.text = "Level: " + levelNumber.ToString();
    }
}
