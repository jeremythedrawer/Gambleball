using UnityEngine;

public class BallMovement : MonoBehaviour
{ 
    private Ball ball;

    private Camera cam => GameManager.Instance.cam;

    private float gravity = 9.8f;
    private Vector2 mouseWorldPos;
    private float timeToPeak;

    private void Awake()
    {
        ball = GetComponent<Ball>();
    }

    private void OnEnable()
    {
        ball.rigidBodyBall.constraints = RigidbodyConstraints2D.FreezeAll;
    }
    private void Start()
    {
        gravity *= ball.weight;
    }

    private void Update()
    {
        PlayerInput();   
    }

    private void PlayerInput()
    {
        if (Input.GetMouseButtonDown(0) && ball.rigidBodyBall.constraints == RigidbodyConstraints2D.FreezeAll)
        {
            mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
            if (mouseWorldPos.x > transform.position.x && mouseWorldPos.y > transform.position.y)
            {
                LaunchBall(mouseWorldPos);
            }
        }
        else if (ball.rigidBodyBall.IsSleeping())
        {
            ball.rigidBodyBall.constraints = RigidbodyConstraints2D.FreezeAll;
            ball.rigidBodyBall.linearVelocity = Vector2.zero;
        }
    }
    private void LaunchBall(Vector2 mouseInputPos)
    {
        ball.rigidBodyBall.constraints = RigidbodyConstraints2D.None;
        float currentHeight = ball.rigidBodyBall.position.y;

        if (mouseInputPos.y <= currentHeight) return; // only recognises inputs above the ball

        timeToPeak = Mathf.Sqrt(2 * (mouseInputPos.y - currentHeight) / gravity); 
        // -> Using y = y0 + v0t - 0.5gt^2 which is displacement equation for vertical motion which turns into t=squareroot(2(y-y0)/g)

        float initialVerticalVelocity = gravity * timeToPeak;

        float horizontalDistance = mouseInputPos.x - ball.rigidBodyBall.position.x;
        float initialHorizontalVelocity = horizontalDistance / timeToPeak;

        ball.rigidBodyBall.linearVelocity = new Vector2(initialHorizontalVelocity, initialVerticalVelocity);
        ball.rigidBodyBall.angularVelocity = ball.spin * 100;
    }
} 
