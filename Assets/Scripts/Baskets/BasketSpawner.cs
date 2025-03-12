using System.Collections.Generic;
using System.Collections;
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

    }

    private void Start()
    {
        InstantiateLevelDataObjects<Basket>(allBaskets, this.transform);
        SetNewBasketPos();
        
    }
    private void SetNewBasketPos()
    {
        StartCoroutine(SettingBasketPos());
    }

    private IEnumerator SettingBasketPos()
    {
        yield return new WaitUntil(()=> activeBasket != null);
        yield return new WaitUntil(() => basketRange.minSpawnPos.x != 0);
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
