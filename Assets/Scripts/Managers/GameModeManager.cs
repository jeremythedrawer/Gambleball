using System;
using UnityEngine;
public class GameModeManager : MonoBehaviour
{
    public static GameModeManager instance;
    public GameModeData gameModeData;

    public int currentDayIndex { get; set; } = 0;
    public DayGameMode currentGameMode => gameModeData.modes[Mathf.Max(currentDayIndex - 1, 0)];

    [Header("Menu Parameters")]
    public float menuSpeed = 0.1f;

    [Header("Lives Mode Parameters")]
    public float livesModeSpeed = 2f;



    private float lerpedTime = 0f;
    private bool completedDayFlag;
    private bool startDayFlag;

    public static event Action onPlayerBeatDay;
    public static event Action onPlayerLost;
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
                float target = Mathf.Clamp01(StatsManager.instance.currentScore / currentGameMode.targetScore);
                lerpedTime = Mathf.Lerp(lerpedTime, target, Time.deltaTime * livesModeSpeed);
                GlobalVolumeController.instance.time = lerpedTime;

                bool playerLooses = StatsManager.instance.attemptsLeft <= 0;
                CheckToEndDay(playerLooses);
            }
            break;
            case 2:
            {
                if (!startDayFlag)
                {
                    StatsManager.instance.SetCountDownTime();
                    startDayFlag = true;
                }
                else
                {
                    StatsManager.instance.CountDownTime();
                    if (currentGameMode.modeData is TimerData timerData)
                    {
                        float target =  1 - Mathf.Clamp01(StatsManager.instance.currentTime / timerData.time);
                        lerpedTime = Mathf.Lerp(lerpedTime, target, Time.deltaTime);
                        GlobalVolumeController.instance.time = lerpedTime;
                    }
                    bool playerLooses = StatsManager.instance.currentTime <= 0;
                    CheckToEndDay(playerLooses);
                }
            }
            break;
            case 3:
            {
                //CheckToEndDay();
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

        GlobalVolumeController.instance.ToggleCRT(0);
        completedDayFlag = true;
    }
}
