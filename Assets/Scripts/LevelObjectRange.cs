using UnityEngine;

public class LevelObjectRange : MonoBehaviour
{
    [Header("Parameters")]
    [Range(0, 1)]
    public float resetPosWidth = 0;
    [Range(0, 1)]
    public float resetPosHeight = 0;

    public float maxResetPosY { get; protected set; }
    public float minResetPosY { get; protected set; }
}
