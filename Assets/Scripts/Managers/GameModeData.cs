using UnityEngine;
using System;
using System.Collections.Generic;

public interface IGameModeSpecificData { }

public enum GameMode
{
    Lives, Timer, Moneyball
};

[Serializable]
public struct LivesData : IGameModeSpecificData
{
    public int lives;
}

[Serializable]
public struct TimerData : IGameModeSpecificData
{
    public int time;
}

[Serializable]
public struct MoneyballData : IGameModeSpecificData
{
    public int startingAttempts;
}


[Serializable]
public struct DayGameMode
{
    public GameMode Mode;
    public float targetScore;
    public bool bird;
    [SerializeReference]
    public IGameModeSpecificData modeData;
}


[CreateAssetMenu(menuName = "Game/DayGameModeTable")]
public class GameModeData : ScriptableObject
{
    public List<DayGameMode> modes = new();
}
