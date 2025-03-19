using System.Linq;
using UnityEngine;

public class Basket : MonoBehaviour
{
    [Header("Parameters")]
    public Sprite spriteBasket;

    public Collider2D[] colliders {  get; private set; }
    public Rigidbody2D backboardRB {  get; private set; }

    public Vector2 backboardStartPos { get; private set; }
    public ScoreTrigger scoreTrigger { get; private set; }

    public virtual void OnEnable()
    {
        colliders = GetComponentsInChildren<Collider2D>().Where(col => !col.isTrigger).ToArray();
        backboardRB = GetComponentInChildren<Rigidbody2D>();
        scoreTrigger = GetComponentInChildren<ScoreTrigger>();

        if (backboardStartPos == Vector2.zero)
        {
            backboardStartPos = backboardRB.transform.localPosition;
        }
    }
}
