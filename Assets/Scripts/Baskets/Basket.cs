using System.Collections;
using UnityEngine;

public class Basket : MonoBehaviour
{
    [Header("Parameters")]
    public bool isMoving;
    [Range(0f, 1f)]
    public float moveScale;

    [Header("References")]
    public Collider2D[] colliders;
    public Rigidbody2D backboardRB;
    public PlusScoreMaterial plusScoreMaterial;
    public ScoreTrigger scoreTrigger;

    [Header("Bounds")]
    public Transform topRight;
    public Transform bottomLeft;


    public Vector2 backboardStartPos { get; private set; }

    private float elepsedTime;
    private float minMovingHeight;
    private float maxMovingHeight;
    public void Start()
    {
        if (backboardStartPos == Vector2.zero)
        {
            backboardStartPos = backboardRB.transform.localPosition;
        }
    }

    private void OnEnable()
    {
        BallSpawner.onOutOfBounds += ResetBasketPos;
    }
    public void MoveBasket(Vector2 middlePos)
    {
        StartCoroutine(MovingBasket(middlePos));
    }
    private IEnumerator MovingBasket(Vector2 middlePos)
    {
        maxMovingHeight = middlePos.y + moveScale;
        minMovingHeight = middlePos.y - moveScale;
        while (true)
        {
            elepsedTime += Time.deltaTime;
            float t = Mathf.Sin(elepsedTime) * 0.5f + 0.5f;
            float yPos = Mathf.Lerp(minMovingHeight, maxMovingHeight, t);

            transform.position = new Vector2 (transform.position.x, yPos);
            yield return null;
        }
    }

    private void ResetBackboard()
    {
        backboardRB.gameObject.transform.localPosition = backboardStartPos;
        backboardRB.gameObject.transform.eulerAngles = Vector2.zero;
        backboardRB.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private void ResetBasketPos()
    {
        transform.position = NewPos(bottomLeft.position, topRight.position);
    }
    protected Vector2 NewPos(Vector2 minPos, Vector2 maxPos)
    {
        float randomX = Random.Range(minPos.x, maxPos.x);
        float randomY = Random.Range(minPos.y, maxPos.y);
        return new Vector2(randomX, randomY);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector3 bottomRight = new Vector3(topRight.position.x, bottomLeft.position.y);
        Vector3 topLeft = new Vector3(bottomLeft.position.x, topRight.position.y);
        Gizmos.DrawLine(bottomLeft.position, bottomRight);
        Gizmos.DrawLine(bottomRight, topRight.position);
        Gizmos.DrawLine(topRight.position, topLeft);
        Gizmos.DrawLine(topLeft, bottomLeft.position);
    }
}
