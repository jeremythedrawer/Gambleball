using System.Collections;
using UnityEngine;

public class Basket : MonoBehaviour
{
    [Header("Parameters")]
    public bool isMoving;
    [Range(0f, 1f)]
    public float moveScale;
    public Color plusThreeColor;

    [Header("References")]
    public Collider2D[] colliders;
    public Rigidbody2D backboardRB;
    public PlusScoreMaterial plusScoreMaterial;
    public ScoreTrigger scoreTrigger;
    public BasketMaterial[] materialControllers;

    [Header("Bounds")]
    public Transform topRight;
    public Transform bottomLeft;

    public static Basket instance { get; private set; }
    public Vector2 backboardStartPos { get; private set; }

    private float elepsedTime;
    private float minMovingHeight;
    private float maxMovingHeight;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        if (backboardStartPos == Vector2.zero)
        {
            backboardStartPos = backboardRB.transform.localPosition;
        }
        BallSpawner.onOutOfBounds += BallOutOfBounds;
    }

    private void OnDisable()
    {
        BallSpawner.onOutOfBounds -= BallOutOfBounds;
    }

    private void Update()
    {
        UpdatePlusScoreUI();
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

    private void BallOutOfBounds()
    {
        ResetBasketPos();
    }
    private void ResetBasketPos()
    {
        transform.position = NewPos(bottomLeft.position, topRight.position);
    }

    private void UpdatePlusScoreUI()
    {
        plusScoreMaterial.usePlusTwo = BallSpawner.instance.type != BallType.Moneyball;
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
