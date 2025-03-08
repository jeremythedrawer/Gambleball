using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Game/LevelData")]
public class LevelData : ScriptableObject
{
    [System.Serializable]
    public struct Levels
    {
        public Object ball;
        public Object basket;

        public Levels(Object ball, Object basket)
        {
            this.ball = ball;
            this.basket = basket;
        }
    }
    public List<Levels> levels;
}
