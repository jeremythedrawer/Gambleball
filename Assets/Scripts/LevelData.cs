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
        public bool checkpoint;
        public Level(Ball ball, Basket basket, bool spawnBird, bool checkpoint)
        {
            this.ball = ball;
            this.checkpoint = checkpoint;
        }
    }
    public List<Level> levels;
}

