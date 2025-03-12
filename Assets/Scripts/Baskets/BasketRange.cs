using UnityEngine;

public class BasketRange : LevelObjectRange
{
    public static BasketRange Instance {  get; private set; }
    public Camera cam => GameManager.Instance.cam;

    public SpriteRenderer activeBasketSpriteRenderer => LevelManager.Instance.activeBasket.spriteRendererBasket;

    public Vector2 minSpawnPos {  get; private set; }
    public Vector2 maxSpawnPos { get; private set; }

    private void OnDrawGizmos()
    {
        DrawBasketResetArea(resetPosWidth, resetPosHeight, new Color(0.0f, 0.5f, 1.0f));
    }

    private void Start()
    {
        if (Instance == null) Instance = this;

        Vector2 topRight = cam.ViewportToWorldPoint(new Vector2(resetPosWidth, resetPosHeight));

        minSpawnPos = transform.position;
        maxSpawnPos = topRight;
    }



    private void DrawBasketResetArea(float resetPosWidth, float resetPosHeight, Color color)
    {
        Vector2 viewportbottomRight = cam.ViewportToWorldPoint(Vector2.right);
        Vector2 viewportTopRight = cam.ViewportToWorldPoint(Vector2.one);

        float distanceFromBottom = transform.position.y - viewportbottomRight.y;
        float distanceFromRight = viewportTopRight.x - transform.position.x; 

        Vector2 bottomLeft = transform.position;
        Vector2 topRight = cam.ViewportToWorldPoint(new Vector2(resetPosWidth, resetPosHeight));
        Vector2 topLeft = cam.ViewportToWorldPoint(new Vector2(1, resetPosHeight)) - new Vector3(distanceFromRight, 0);
        Vector2 bottomRight = cam.ViewportToWorldPoint(new Vector2(resetPosWidth, 0)) + new Vector3(0, distanceFromBottom);

        Gizmos.color = color;
        Gizmos.DrawLine(bottomLeft, topLeft);
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
    }
}
