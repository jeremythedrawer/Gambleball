using System.Collections;
using System.Linq;
using UnityEngine;

public class Basket : MonoBehaviour
{
    [Header("Parameters")]
    public Sprite spriteBasket;
    public bool isMoving;
    [Range(0f, 1f)]
    public float moveScale;
    public Collider2D[] colliders {  get; private set; }
    public Rigidbody2D backboardRB {  get; private set; }

    public Vector2 backboardStartPos { get; private set; }
    public ScoreTrigger scoreTrigger { get; private set; }

    public PlusScoreMaterial plugScoreMaterial { get; private set; }

    private float elepsedTime;
    private float minMovingHeight;
    private float maxMovingHeight;
    public virtual void OnEnable()
    {
        colliders = GetComponentsInChildren<Collider2D>().Where(col => !col.isTrigger).ToArray();
        backboardRB = GetComponentInChildren<Rigidbody2D>();
        scoreTrigger = GetComponentInChildren<ScoreTrigger>();
        plugScoreMaterial = GetComponentInChildren<PlusScoreMaterial>();
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
}
