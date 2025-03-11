using System.Linq;
using UnityEngine;

public class Basket : MonoBehaviour
{
    [Header("Parameters")]
    public Sprite spriteBasket;

    public Collider2D[] colliders {  get; private set; }
    public SpriteRenderer spriteRendererBasket { get; private set; }

    public ScoreTrigger scoreTrigger { get; private set; }

    private void Awake()
    {
        colliders = GetComponentsInChildren<Collider2D>().Where(col => !col.isTrigger).ToArray();
        spriteRendererBasket = GetComponent<SpriteRenderer>();
        scoreTrigger = GetComponentInChildren<ScoreTrigger>();
    }
}
