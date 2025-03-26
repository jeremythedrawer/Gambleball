using UnityEngine;

public class ScoreTrigger : MonoBehaviour
{
    public PolygonCollider2D triggerCollider {  get; private set; }
    public Vector2[] worldPoints {  get; private set; }

    private Basket basket;

    private void Start()
    {
        basket = GetComponentInParent<Basket>();
    }

    private void Update()
    {
        UpdateWorldPoints();
    }
    public void SetUp()
    {
        triggerCollider = GetComponent<PolygonCollider2D>();
        UpdateWorldPoints();
    }

    private void UpdateWorldPoints()
    {
        Vector2[] localPoints = triggerCollider.GetPath(0);
        worldPoints = new Vector2[localPoints.Length];

        for (int i = 0; i < localPoints.Length; i++)
        {
            Vector2 worldPoint = transform.TransformPoint(localPoints[i]);
            worldPoints[i] = worldPoint;
        }
    }
}
