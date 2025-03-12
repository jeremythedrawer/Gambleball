using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "LevelData", menuName = "Game/LevelData")]
public class LevelData : ScriptableObject
{
    [System.Serializable]
    public struct Level
    {
        public Ball ball;
        public Basket basket;

        public Level(Ball ball, Basket basket)
        {
            this.ball = ball;
            this.basket = basket;
        }
    }
    public List<Level> levels;
}

