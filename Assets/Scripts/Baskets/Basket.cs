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
}
