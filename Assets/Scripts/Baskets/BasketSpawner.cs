using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BasketSpawner : Spawner
{
    public static BasketSpawner Instance { get; private set; }
    private Basket activeBasket => LevelManager.Instance.activeBasket;
    private BasketRange basketRange => BasketRange.Instance;
    public List<Basket> allBaskets = new List<Basket>();

    private void Awake()
    {
        if (Instance == null) Instance = this;

        InstantiateLevelDataObjects<Basket>(allBaskets, this.transform);
        SetNewBasketPos();
    }

    private async void SetNewBasketPos()
    {
        while (activeBasket == null) { await Task.Yield(); }
        newPos = GetNewPos
        (
            basketRange.minSpawnPos.x, 
            basketRange.maxSpawnPos.x, 
            basketRange.minSpawnPos.y, 
            basketRange.maxSpawnPos.y
        );

        activeBasket.transform.position = newPos;
    }
}
