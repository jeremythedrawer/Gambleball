using UnityEngine;

public abstract class Spawner : MonoBehaviour
{
    public Vector2 newPos { get; protected set; }

    protected Vector2 NewPos(Vector2 minPos, Vector2 maxPos)
    {
        float randomX = Random.Range(minPos.x, maxPos.x);
        float randomY = Random.Range(minPos.y, maxPos.y);
        return newPos = new Vector2(randomX, randomY);
    }
}
