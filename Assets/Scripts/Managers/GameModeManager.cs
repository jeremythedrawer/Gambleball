using UnityEngine;
public class GameModeManager : MonoBehaviour
{
    public static GameModeManager instance;
    public GameModeData gameModeData;

    private int currentDayIndex;
    public DayGameMode currentGameMode => gameModeData.modes[currentDayIndex];

    [Header("Lives Mode Parameters")]
    public float lerpSpeed = 2f;
    
    private float lerpedTime = 0f;
    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        PickDay();
    }

    private void PickDay()
    {
        switch(currentDayIndex)
        { 
            case 0:
            {
                float target = Mathf.Clamp01(StatsManager.instance.currentScore / currentGameMode.targetScore);
                lerpedTime = Mathf.Lerp(lerpedTime, target, Time.deltaTime * lerpSpeed);
                GlobalVolumeController.instance.time = lerpedTime;
            }
            break;
            case 1:
            {
               
            }
            break;
            case 2:
            {

            }
            break;
        }
    }
}
