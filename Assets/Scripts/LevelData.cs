using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "LevelData", menuName = "Game/LevelData")]
public class LevelData : ScriptableObject
{
    public Bird bird;

    [System.Serializable]
    public struct Level
    {
        public Ball ball;
        public Basket basket;
        public bool spawnBird;
        public Level(Ball ball, Basket basket, bool spawnBird)
        {
            this.ball = ball;
            this.basket = basket;
            this.spawnBird = spawnBird;
        }
    }
    public List<Level> levels;
}

