using UnityEngine;

public class RangeMaterialController : MonoBehaviour
{
    private Material material;

    private Vector3 color;
    private Vector2 position;
    private void Start()
    {
        Renderer renderer = GetComponent<Renderer>();

        material = renderer.material;

        color = new Vector3(0,1,0);
    }

    private void Update()
    {
        int attempts = GameManager.Instance.attempts;

        if (attempts == 3)
        {
            color = new Vector3(0,1,0);
        }
        else if (attempts == 2)
        {
            color = new Vector3(1,1,0);
        }
        else
        {
            color = new Vector3(1,0,0);
        }

        material.SetVector("_Color", color);

        float maxY = BallSpawner.Instance.maxResetBallPosY;
        float minY = BallSpawner.Instance.minResetBallPosY;
        float maxX = BallSpawner.Instance.currentMaxThreshold;
        float minX = BallSpawner.Instance.currentMinThreshold;

        float currentPosX = minX + ((maxX - minX) / 2);
        float currentPosY = minY + ((maxY - minY) / 2);
        transform.position = new Vector2(currentPosX, currentPosY);

    }
}
