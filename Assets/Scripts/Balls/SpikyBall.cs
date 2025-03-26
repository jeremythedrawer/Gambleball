using System.Collections;
using System.Linq;
using UnityEngine;

public class SpikyBall : Ball
{
    private Basket activeBasket => GameManager.Instance.activeBasket;

    public bool isStuck {  get; private set; }
    public override void Update()
    {
        base.Update();
        DetectBasketCollision();
    }

    private void DetectBasketCollision()
    {
        if (activeBasket.colliders.Any(col => circleColliderBall.IsTouching(col)) && 
            rigidBodyBall.constraints != RigidbodyConstraints2D.FreezeAll && !isStuck)
        {
            isStuck = true;
            rigidBodyBall.constraints = RigidbodyConstraints2D.FreezeAll;
            StartCoroutine(ResettingPos());
        }
    }
    private IEnumerator ResettingPos()
    {
        ArcMaterial.Instance.gameObject.SetActive(false);
        yield return new WaitForSeconds(1);
        GameManager.Instance.HandleOutOfBounds();
        ArcMaterial.Instance.gameObject.SetActive(true);
        isStuck = false;
    }
}
