using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("Parameters")]
    public Sprite spriteBall;
    public float weight;
    public float bounciness = 0.5f;
    public float spin = 5;
    public float scoreDetectionRaduis = 0.1f;

    public Rigidbody2D rigidBodyBall;
    public CircleCollider2D circleColliderBall;
    public SpriteRenderer spriteRendererBall;

    private Collider2D activeBasketScoreTrigger => LevelManager.Instance.activeBasket.scoreTrigger.triggerCollider;
    private LayerMask scoreTriggerLayer;
    private bool enteredFromTop;
    public bool playerScored {  get; set; }
    private void OnValidate()
    {
        spriteRendererBall = GetComponent<SpriteRenderer>();
        
    }

    public virtual void Start()
    {
        SetUpBall();
        scoreTriggerLayer = 1 << LayerMask.NameToLayer("Score Trigger");
    }

    private void OnEnable()
    {
        rigidBodyBall.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public virtual void Update()
    {
        DetectIfScored();
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


    private void DetectIfScored()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, scoreDetectionRaduis, scoreTriggerLayer);

        if (hit != null)
        {
            Vector2 triggerCenter = hit.bounds.center;

            if (!enteredFromTop)
            {
                enteredFromTop = transform.position.y > triggerCenter.y;
            }
        }
        else if (enteredFromTop)
        {
            if (transform.position.x < activeBasketScoreTrigger.bounds.min.x || 
                transform.position.x > activeBasketScoreTrigger.bounds.max.x || 
                transform.position.y < activeBasketScoreTrigger.bounds.min.y)
            {
                if (rigidBodyBall.linearVelocityY < 0)
                {
                    rigidBodyBall.linearVelocityY *= 0.25f;

                    BasketMaterial.scoreColor = Color.green;
                    playerScored = true;
                }
            }
            enteredFromTop = false;
        }
    }

    public virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, scoreDetectionRaduis);
    }
}
