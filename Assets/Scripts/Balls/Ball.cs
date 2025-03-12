using System.Threading.Tasks;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("Parameters")]
    public Sprite spriteBall;
    public float weight;
    public float bounciness = 0.5f;
    public float spin = 5;
    public Rigidbody2D rigidBodyBall;
    public CircleCollider2D circleColliderBall;
    public SpriteRenderer spriteRendererBall;

    private void OnValidate()
    {
        spriteRendererBall = GetComponent<SpriteRenderer>();
        
    }

    public virtual void Start()
    {
        SetUpBall();
    }

    private void OnEnable()
    {
        rigidBodyBall.constraints = RigidbodyConstraints2D.FreezeAll;
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

        while (circleColliderBall == null) { await Task.Yield(); }

        if (circleColliderBall.sharedMaterial == null)
        {
            PhysicsMaterial2D material = new PhysicsMaterial2D("BallMaterial")
            {
                bounciness = bounciness,
                friction = 0.4f
            };
            circleColliderBall.sharedMaterial = material;
        }
        else
        {
            circleColliderBall.sharedMaterial.bounciness = bounciness;
        }
    }
}
