using System;
using UnityEngine;
public class GameModeManager : MonoBehaviour
{
    public static GameModeManager instance;
    public GameModeData gameModeData;

    public int currentDayIndex { get; set; } = 0;
    public DayGameMode currentGameMode => gameModeData.modes[Mathf.Clamp(currentDayIndex - 1, 0, gameModeData.modes.Count - 1)];

    [Header("Menu Parameters")]
    public float menuSpeed = 0.1f;

    [Header("Lives Mode Parameters")]
    public float livesModeSpeed = 2f;

    private float lerpedTime = 0f;
    private bool completedDayFlag;
    private bool startDayFlag;

    public static event Action onPlayerBeatDay;
    public static event Action onPlayerLost;

    private int lowestAttemptScore;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        PickDay();
    }

    private void PickDay()
    {
        switch (currentDayIndex)
        {
            case 0:
            {
                completedDayFlag = false;
                startDayFlag = false;
                lerpedTime += Time.deltaTime * menuSpeed;
                float t = Mathf.Sin(lerpedTime) * 0.5f + 0.5f;
                GlobalVolumeController.instance.time = t;
            }
            break;
            case 1:
            {
                if (currentGameMode.modeData is LivesData livesData)
                {
                    if (!startDayFlag)
                    {
                        StatsManager.instance.SetHeartAttempts(livesData);
                        startDayFlag = true;
                    }
                    else
                    {
                        float target = Mathf.Clamp01(StatsManager.instance.currentScore / currentGameMode.targetScore);
                        lerpedTime = Mathf.Lerp(lerpedTime, target, Time.deltaTime * livesModeSpeed);
                        GlobalVolumeController.instance.time = lerpedTime;

                        bool playerLooses = StatsManager.instance.currentHeartAttempts <= 0;
                        CheckToEndDay(playerLooses);
                    }
                }
            }
            break;
            case 2:
            {
                if (currentGameMode.modeData is TimerData timerData)
                {
                    if (!startDayFlag)
                    {
                        StatsManager.instance.SetCountDownTime(timerData);
                        startDayFlag = true;
                    }
                    else
                    {
                        StatsManager.instance.CountDownTime();

                        float target =  1 - Mathf.Clamp01(StatsManager.instance.currentTime / timerData.time);
                        lerpedTime = Mathf.Lerp(lerpedTime, target, Time.deltaTime);
                        GlobalVolumeController.instance.time = lerpedTime;
                        bool playerLooses = StatsManager.instance.currentTime <= 0;
                        CheckToEndDay(playerLooses);
                    }
                }
            }
            break;
            case 3:
            {
                if (currentGameMode.modeData is MoneyBallData moneyballData)
                {
                    if (!startDayFlag)
                    {
                        StatsManager.instance.SetMoneyBallAttempts(moneyballData);
                        lowestAttemptScore = moneyballData.startingAttempts;
                        startDayFlag = true;
                    }
                    else
                    {
                        lowestAttemptScore = Mathf.Min(lowestAttemptScore, StatsManager.instance.currentMoneyBallAttempts);
                        float target = 1 - Mathf.Clamp01((float)lowestAttemptScore / moneyballData.startingAttempts);
                        lerpedTime = Mathf.Lerp(lerpedTime, target, Time.deltaTime * livesModeSpeed);
                        GlobalVolumeController.instance.time = target;
                        bool playerLooses = StatsManager.instance.currentMoneyBallAttempts <= 0;
                        CheckToEndDay(playerLooses);
                    }
                }
            }
            break;
            case 4:
            {
                GlobalVolumeController.instance.time = 0;
            }
            break;
        }
    }

    private void CheckToEndDay(bool playerLooses)
    {
        if (completedDayFlag) return;

        if (StatsManager.instance.currentScore >= currentGameMode.targetScore)
        {
            onPlayerBeatDay?.Invoke();
            if (currentDayIndex == 3)
            {
                GlobalVolumeController.instance.ToggleCRT(4, turnMusicOff: false);
                currentDayIndex = 4;
                completedDayFlag = true;
                return;
            }
        }
        else if (playerLooses)
        {
            onPlayerLost?.Invoke();
        }
        else
        {
            return;
        }

        currentDayIndex = 0;
        GlobalVolumeController.instance.ToggleCRT(0, turnMusicOff: false);
        completedDayFlag = true;
    }
}
