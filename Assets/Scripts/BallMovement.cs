using UnityEngine;

public class BallMovement : MonoBehaviour
{
    public float burstFactor = 2f;
    private Rigidbody2D body2D;
    private CircleCollider2D circleCollider2D;

    private Camera cam;
    private Vector2 startPos;

    private float gravity = 9.8f;
    private float initGrav;
    private Vector2 mouseWorldPos;
    private float timeToPeak;

    private void Awake()
    {
        body2D = GetComponent<Rigidbody2D>();
        circleCollider2D = GetComponent<CircleCollider2D>();
        cam = Camera.main;
    }

    private void Start()
    {
        startPos = transform.position;
        initGrav = body2D.gravityScale;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
            body2D.gravityScale = initGrav; 
            LaunchBall(mouseWorldPos);
            body2D.constraints = RigidbodyConstraints2D.None;
        }
        else if (body2D.IsSleeping())
        {
            body2D.gravityScale = 0;
            body2D.constraints = RigidbodyConstraints2D.FreezePosition;
            body2D.linearVelocity = Vector2.zero;
        }
    }
    private void LaunchBall(Vector2 mouseInputPos)
    {
        gravity *= initGrav;
        float currentHeight = body2D.position.y;

        if (mouseInputPos.y <= currentHeight) return; // only recognises inputs above the ball

        timeToPeak = Mathf.Sqrt(2 * (mouseInputPos.y - currentHeight) / gravity);
        float initialVerticalVelocity = gravity * timeToPeak;

        float horizontalDistance = mouseInputPos.x - body2D.position.x;
        float initialHorizontalVelocity = horizontalDistance / timeToPeak;

        body2D.linearVelocity = new Vector2(initialHorizontalVelocity, initialVerticalVelocity);
    }
} 
