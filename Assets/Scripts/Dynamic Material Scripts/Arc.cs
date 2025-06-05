using UnityEngine;

public class Arc : MonoBehaviour
{
    private Ball activeBall => BallSpawner.instance.activeBall;

    private float width;
    private float height;

    private Vector2 topRight;
    private void Update()
    {
        transform.position = activeBall.transform.position;

        if ((Input.GetMouseButtonDown(0) && activeBall.body.linearVelocity.magnitude == 0) || 
            activeBall.body.constraints == RigidbodyConstraints2D.FreezeAll)
        {
            topRight = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else if (activeBall.body.linearVelocity.y <= 0)
        {
            topRight = activeBall.transform.position;
        }
        width = topRight.x - transform.position.x;
        height = Mathf.Max(topRight.y - transform.position.y, 0);

        transform.localScale = new Vector2( width, height );
    }
}
