using UnityEngine;

public class ArcMaterial : MaterialManager
{
    private Ball activeBall;

    private float width;
    private float height;

    private Vector2 topRight;

    private void Update()
    {

        activeBall = GameManager.Instance.activeBall;
        transform.position = activeBall.transform.position;

        if (Input.GetMouseButtonDown(0) || activeBall.rigidBodyBall.constraints == RigidbodyConstraints2D.FreezeAll)
        {
            topRight = GameManager.Instance.cam.ScreenToWorldPoint(Input.mousePosition);
        }
        else if (activeBall.rigidBodyBall.linearVelocity.y < 0)
        {
            topRight = Vector2.zero;
        }
        width = Mathf.Max(topRight.x - transform.position.x, 0);
        height = Mathf.Max(topRight.y - transform.position.y, 0);

        transform.localScale = new Vector2( width, height );
    }
}
