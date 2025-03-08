using System.Runtime.CompilerServices;
using UnityEngine;

public class BallMovement : MonoBehaviour
{ 
    private Rigidbody2D body2D;
    private CircleCollider2D circleCollider2D;

    private Camera cam => CamData.Instance.cam;

    public Vector2 newBallPos {  get; private set; }

    private float gravity = 9.8f;
    private Vector2 mouseWorldPos;
    private float timeToPeak;


    private Vector2 bottomLeftWorldPos;
    private Vector2 topRightWoldPos;

    private void Awake()
    {
        body2D = GetComponent<Rigidbody2D>();
        circleCollider2D = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        newBallPos = transform.position;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
            LaunchBall(mouseWorldPos);
            body2D.constraints = RigidbodyConstraints2D.None;
        }
        else if (body2D.IsSleeping())
        {
            body2D.constraints = RigidbodyConstraints2D.FreezePosition;
            body2D.linearVelocity = Vector2.zero;
        }
        ResetBallPos(cam, transform.position);
    }
    private void LaunchBall(Vector2 mouseInputPos)
    {
        gravity *= body2D.gravityScale;
        float currentHeight = body2D.position.y;

        if (mouseInputPos.y <= currentHeight) return; // only recognises inputs above the ball

        timeToPeak = Mathf.Sqrt(2 * (mouseInputPos.y - currentHeight) / gravity); 
        // -> Using y = y0 + v0t - 0.5gt^2 which is displacement equation for vertical motion which turns into t=squareroot(2(y-y0)/g)

        float initialVerticalVelocity = gravity * timeToPeak;

        float horizontalDistance = mouseInputPos.x - body2D.position.x;
        float initialHorizontalVelocity = horizontalDistance / timeToPeak;

        body2D.linearVelocity = new Vector2(initialHorizontalVelocity, initialVerticalVelocity);
        gravity /= body2D.gravityScale;
    }

    private void ResetBallPos(Camera cam, Vector2 ballPos)
    {
        Vector2 camWorldPointBottomLeft = CamData.Instance.bottomLeftWorldPos;
        Vector2 camWorldPointTopRight = CamData.Instance.topRightWoldPos;
        float camBoundBottom = camWorldPointBottomLeft.y;
        float camBoundRight = camWorldPointTopRight.x;
        float camBoundsLeft = camWorldPointBottomLeft.x;

        if (ballPos.y < camBoundBottom || ballPos.x > camBoundRight)
        {
            float maxX = CamData.Instance.maxResetBallPosX;
            float maxY = CamData.Instance.maxResetBallPosY;

            float randomX = 0;
            float randomY = 0;
            if (randomX == 0 || randomY == 0)
            {
                randomX = Random.Range(camBoundsLeft, maxX);
                randomY = Random.Range(camBoundBottom, maxY);
            }
            newBallPos = new Vector2(randomX, randomY);
            transform.position = newBallPos;
            body2D.constraints = RigidbodyConstraints2D.FreezePosition;
        }
    }
} 
