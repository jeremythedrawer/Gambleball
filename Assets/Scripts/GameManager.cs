using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public LevelData LevelData;

    public Camera cam {  get; private set; }
    public BallSpawner ballSpawner {  get; private set; }
    public BasketSpawner basketSpawner { get; private set; }

    public int attempts { get; set; } = 0;

    private void OnValidate()
    {
        cam = Camera.main;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        SetUpSpawners();
    }
    private void Update()
    {
        if (attempts > 3)
        {
            attempts = 1;
        }
    }
    private void SetUpSpawners()
    {
        ballSpawner = GetComponentInChildren<BallSpawner>();
        basketSpawner = GetComponentInChildren<BasketSpawner>();
        cam = Camera.main;
    }
}
