using UnityEngine;

public class ActiveRangeMaterial : MaterialManager
{
    private Vector3 color;

    public override void Start()
    {
        base.Start();
        color = new Vector3(0, 1, 0);
    }

    private void Update()
    {
        int attempts = GameManager.Instance.attempts;

        if (attempts == 3)
        {
            color = new Vector3(0,1,0);
        }
        else if (attempts == 2)
        {
            color = new Vector3(1,1,0);
        }
        else
        {
            color = new Vector3(1,0,0);
        }

        material.SetVector("_Color", color);
    }
}
