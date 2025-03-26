using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class BasketSpawner : Spawner
{
    [Header("Parameters")]
    [Range(0, 1)]
    public float resetPosWidth = 0;
    [Range(0, 1)]
    public float resetPosHeight = 0;

    public static BasketSpawner Instance { get; private set; }
    private Basket activeBasket => GameManager.Instance.activeBasket;
    public List<Basket> allBaskets = new List<Basket>();

    private struct BasketRangePoints
    {
        public Vector2 topLeft, topRight, bottomLeft, bottomRight;
    }

    private BasketRangePoints[] basketRangePointsArray = new BasketRangePoints[1];

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        InstantiateLevelDataObjects<Basket>(allBaskets, this.transform);
        SetBasketRangePointsArray();
        SetNewBasketPos();
        
    }
    public async void SetNewBasketPos()
    {
        while (activeBasket == null) {  await Task.Yield(); }
        bool isWrappingBasket = activeBasket is WrappingBasket;

        BasketRangePoints points = isWrappingBasket ? basketRangePointsArray[1] : basketRangePointsArray[0];

        newPos = GetNewPos(points.topLeft.x, points.bottomRight.x, points.bottomRight.y, points.topLeft.y);

        activeBasket.transform.position = newPos;
        activeBasket.scoreTrigger.SetUp();
    }

    private void SetBasketRangePointsArray()
    {
        basketRangePointsArray = NewBasketRangePoints();
    }

    private BasketRangePoints[] NewBasketRangePoints()
    {
        Vector2 viewportTopLeft = Camera.main.ViewportToWorldPoint(new Vector2(0, 1));
        Vector2 viewportTopRight = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        Vector2 viewportBottomRight = Camera.main.ViewportToWorldPoint(new Vector2(1, 0));
        Vector2 viewportBottomLeft = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));

        float distanceFromBottom = transform.position.y - viewportBottomRight.y;
        float distanceFromRight = viewportTopRight.x - transform.position.x;
        float distanceFromLeft = transform.position.x - viewportTopLeft.x;

        BasketRangePoints points0 = new BasketRangePoints
        {
            bottomLeft = transform.position,
            topRight = Camera.main.ViewportToWorldPoint(new Vector2(resetPosWidth, resetPosHeight)),
            topLeft = Camera.main.ViewportToWorldPoint(new Vector2(0, resetPosHeight)) + new Vector3(distanceFromLeft, 0),
            bottomRight = Camera.main.ViewportToWorldPoint(new Vector2(resetPosWidth, 0)) + new Vector3(0, distanceFromBottom)
        };

        BasketRangePoints points1 = new BasketRangePoints
        {
            bottomLeft = Camera.main.ViewportToWorldPoint(new Vector2(1 - resetPosWidth, 0)) + new Vector3(0, distanceFromBottom),
            topRight = Camera.main.ViewportToWorldPoint(new Vector2(0, resetPosHeight)) + new Vector3(distanceFromRight, 0),
            topLeft = Camera.main.ViewportToWorldPoint(new Vector2(1 - resetPosWidth, resetPosHeight)),
            bottomRight = new Vector2(-transform.position.x,transform.position.y),
        };

        return new BasketRangePoints[] { points0, points1 };
    }
    #region Gizmos
    private void OnDrawGizmos()
    {
        DrawBasketResetArea(resetPosWidth, resetPosHeight, new Color(0.0f, 0.5f, 1.0f));
    }

    private void DrawBasketResetArea(float resetPosWidth, float resetPosHeight, Color color)
    {
        basketRangePointsArray = NewBasketRangePoints();

        Gizmos.color = color;

        for (int i = 0; i < basketRangePointsArray.Length; i++)
        {
            BasketRangePoints points = basketRangePointsArray[i];
            Gizmos.DrawLine(points.topLeft, points.topRight);
            Gizmos.DrawLine(points.topRight, points.bottomRight);
            Gizmos.DrawLine(points.bottomRight, points.bottomLeft);
            Gizmos.DrawLine(points.bottomLeft, points.topLeft);
        }
    }
    #endregion
}

