using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Game/LevelData")]
public class LevelData : ScriptableObject
{
    [System.Serializable]
    public struct Levels
    {
        public Ball ball;
        public Basket basket;

        public Levels(Ball ball, Basket basket)
        {
            this.ball = ball;
            this.basket = basket;
        }
    }
    public List<Levels> levels;
}
