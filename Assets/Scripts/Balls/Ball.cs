using System.Threading.Tasks;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("Parameters")]
    public Sprite spriteBall;
    public float weight;

    public Rigidbody2D rigidBodyBall {  get; private set; }
    public CircleCollider2D circleColliderBall { get; private set; }
    public SpriteRenderer spriteRendererBall { get; private set; }

    private void OnValidate()
    {
        spriteRendererBall = GetComponent<SpriteRenderer>();
        
    }

    public virtual void Awake()
    {
        SetUpBall();
    }

    private async void SetUpBall()
    {
        rigidBodyBall = GetComponent<Rigidbody2D>();
        circleColliderBall = GetComponent<CircleCollider2D>();
        spriteRendererBall = GetComponent<SpriteRenderer>();

        while (rigidBodyBall == null) {  await Task.Yield(); }
        rigidBodyBall.gravityScale = weight;
        while (spriteRendererBall == null) { await Task.Yield(); }
        spriteRendererBall.sprite = spriteBall;
    }
}
