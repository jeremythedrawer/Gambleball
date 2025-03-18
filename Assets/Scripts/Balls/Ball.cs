using System.Linq;
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
    public float scoreDetectionRadius = 0.1f;

    public Rigidbody2D rigidBodyBall;
    public CircleCollider2D circleColliderBall;
    public SpriteRenderer spriteRendererBall;

    private ScoreTrigger activeBasketScoreTrigger => LevelManager.Instance.activeBasket.scoreTrigger;
    private LayerMask scoreTriggerLayer;
    private LayerMask birdLayer;
    private bool enteredFromTop;
    public bool playerScored {  get; set; }
    public bool playerHitBird { get; set; }
    private void OnValidate()
    {
        spriteRendererBall = GetComponent<SpriteRenderer>();
        
    }

    public virtual void Start()
    {
        SetUpBall();
        scoreTriggerLayer = 1 << LayerMask.NameToLayer("Score Trigger");
        birdLayer = 1 << LayerMask.NameToLayer("Bird");
    }

    private void OnEnable()
    {
        rigidBodyBall.constraints = RigidbodyConstraints2D.FreezeAll;
        playerScored = false;
        playerHitBird = false;
    }

    public virtual void Update()
    {
        DetectIfScored();
        DetectBirdHit();
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
        Collider2D hit = Physics2D.OverlapCircle(transform.position, scoreDetectionRadius, scoreTriggerLayer);

        if (hit != null && !enteredFromTop)
        {
            enteredFromTop = transform.position.y > activeBasketScoreTrigger.worldPoints[0].y;
        }
        else if (enteredFromTop)
        {
            if (transform.position.y < activeBasketScoreTrigger.worldPoints[3].y)
            {
                if (transform.position.x > activeBasketScoreTrigger.worldPoints[2].x)
                {
                    if (!playerHitBird)
                    {
                        PlusScoreMaterial.usePlusOne = true;
                        BasketMaterial.scoreColor = Color.green;
                    }
                    else
                    {
                        PlusScoreMaterial.usePlusOne = false;
                        BasketMaterial.scoreColor = Color.yellow;
                    }

                    if (LevelManager.Instance.currentLevelIndex > 0)
                    {
                        PlusScoreMaterial.alpha = 1;
                    }
                    playerScored = true;
                    enteredFromTop = false;
                }
                else
                {
                    enteredFromTop = false;
                }
            }
        }
    }

    private void DetectBirdHit()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, scoreDetectionRadius, birdLayer);

        if (hit != null) playerHitBird = true;
    }

    public virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, scoreDetectionRadius);
    }
}
