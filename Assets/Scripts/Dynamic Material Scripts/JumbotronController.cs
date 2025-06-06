using System.Collections;
using UnityEngine;

public class JumbotronController : MonoBehaviour
{
    [Header("From Downtown")]
    public JumbotronSignsMaterial fromDowntown;

    [Header("On Fire")]
    public JumbotronSignsMaterial onFire;

    private void OnEnable()
    {
        BallSpawner.onPlayerScored += ChooseSign; 
    }

    private void ChooseSign()
    {
        if (StatsManager.instance.fromDowntown)
        {
            fromDowntown.onOff = true;
        }
        else if (StatsManager.instance.onFire)
        {
            onFire.onOff = true;
        }
    }
}
