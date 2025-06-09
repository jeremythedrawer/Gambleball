using UnityEngine;

public class BallMovement : MonoBehaviour
{ 
    public Ball ball;

    private float gravity = 9.8f;
    private Vector2 mouseWorldPos;
    private float timeToPeak;

    private void OnEnable()
    {
        ball.body.constraints = RigidbodyConstraints2D.FreezeAll;
        gravity *= ball.weight;
    }

    private void Update()
    {
        PlayerInput();   
    }

    private void PlayerInput()
    {
        if (InputManager.Instance.leftMouseButtonDown && ball.body.constraints == RigidbodyConstraints2D.FreezeAll)
        {
            mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (mouseWorldPos.y > transform.position.y)
            {
                LaunchBall(mouseWorldPos);
            }
        }
        else if (ball.body.IsSleeping())
        {
            ball.body.constraints = RigidbodyConstraints2D.FreezeAll;
            ball.body.linearVelocity = Vector2.zero;
        }
    }
    private void LaunchBall(Vector2 mouseInputPos)
    {
        ball.body.constraints = RigidbodyConstraints2D.None;
        float currentHeight = ball.body.position.y;

        if (mouseInputPos.y <= currentHeight) return; // only recognises inputs above the ball

        timeToPeak = Mathf.Sqrt(2 * (mouseInputPos.y - currentHeight) / gravity);
        // -> Using y = y0 + v0t - 0.5gt^2 which is displacement equation for vertical motion which turns into t=squareroot(2(y-y0)/g)
        float adjustedWeight = Mathf.Lerp(1.5f - ball.weight, 1f, ball.weightFactor);
        float initialVerticalVelocity = (gravity * timeToPeak) * adjustedWeight;

        float horizontalDistance = mouseInputPos.x - ball.body.position.x;
        float initialHorizontalVelocity = horizontalDistance / timeToPeak;

        ball.body.linearVelocity = new Vector2(initialHorizontalVelocity, initialVerticalVelocity);
        ball.body.angularVelocity = ball.spin * 100;
    }
} 
