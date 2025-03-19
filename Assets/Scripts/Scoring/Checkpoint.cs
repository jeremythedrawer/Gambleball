using System.Threading.Tasks;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public float maxXPoint {  get; private set; }
    private void Awake()
    {
        SetUp();
    }

    private async void SetUp()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        while (spriteRenderer == null) await Task.Yield();

        maxXPoint = spriteRenderer.size.x * 1.5f;
    }
}
