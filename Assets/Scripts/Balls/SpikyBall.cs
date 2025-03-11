using System.Collections;
using System.Linq;
using UnityEngine;

public class SpikyBall : Ball
{
    private Basket activeBasket => LevelManager.Instance.activeBasket;

    public bool isStuck {  get; private set; }
    private void Update()
    {
        if (activeBasket.colliders.Any(col => circleColliderBall.IsTouching(col)) && rigidBodyBall.constraints != RigidbodyConstraints2D.FreezeAll)
        {
            isStuck = true;
            rigidBodyBall.constraints = RigidbodyConstraints2D.FreezeAll;
            StartCoroutine(ResettingPos());
        }
    }

    private IEnumerator ResettingPos()
    {
        yield return new WaitForSeconds(1);
        GameManager.Instance.OutOfBounds();
        isStuck = false;
    }
}
