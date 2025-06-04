using System.Threading.Tasks;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("Parameters")]
    public Sprite spriteBall;
    public float weight;
    public float bounciness = 0.5f;
    public float spin = 5;
    public float scoreDetectionRadius = 0.1f;
    public float birdDetectRadius = 0.2f;
    [Range(0f, 1f)]
    public float weightFactor = 0.85f;


    [Header("References")]
    public Rigidbody2D rigidBodyBall;
    public CircleCollider2D circleColliderBall;
    public SpriteRenderer spriteRendererBall;
    public Basket basket;

    [Header("Out Of Bounds")]
    public Transform outOfBoundsTransform;


    private LayerMask scoreTriggerLayer;
    private bool enteredFromTop;
    public bool playerScored {  get; set; }
    public bool outOfBounds { get; private set; }
    public virtual void Start()
    {
        SetUpBall();
        scoreTriggerLayer = 1 << LayerMask.NameToLayer("Score Trigger");
    }

    private void OnEnable()
    {
        rigidBodyBall.constraints = RigidbodyConstraints2D.FreezeAll;
        playerScored = false;
    }

    public virtual void Update()
    {
        DetectIfScored();
        outOfBounds = transform.position.y < outOfBoundsTransform.position.y;
    }
    private void SetUpBall()
    {
        rigidBodyBall.gravityScale = weight;
        spriteRendererBall.sprite = spriteBall;
        circleColliderBall.sharedMaterial.bounciness = bounciness;
    }


    private void DetectIfScored()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, scoreDetectionRadius, scoreTriggerLayer);

        if (hit != null && !enteredFromTop)
        {
            enteredFromTop = transform.position.y > basket.scoreTrigger.worldPoints[0].y;
            playerScored = false;
        }
        else if (enteredFromTop)
        {
            if (transform.position.y < basket.scoreTrigger.worldPoints[3].y && transform.position.x > basket.scoreTrigger.worldPoints[2].x)
            {
                BasketMaterial.scoreColor = Color.green;

                basket.plusScoreMaterial.alpha = 1;

                playerScored = true;
                enteredFromTop = false;
            }
        }
        else
        {
            playerScored = false;
        }
    }

    public virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, scoreDetectionRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, birdDetectRadius);
    }
}
