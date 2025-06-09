using System.Collections;
using UnityEngine;

public class JumbotronController : MonoBehaviour
{
    public JumbotronSignsMaterial fromDowntown;
    public JumbotronSignsMaterial onFire;
    public JumbotronSignsMaterial moneyBall;
    public JumbotronSignsMaterial attemptBoost;

    private void OnEnable()
    {
        BallSpawner.onPlayerScored += ChooseSign; 
    }
    private void OnDisable()
    {
        BallSpawner.onPlayerNotScored -= ChooseSign;
    }

    private void ChooseSign()
    {
        if (StatsManager.instance.onFire)
        {
            onFire.onOff = true;
        }
        else if (StatsManager.instance.fromDowntown)
        {
            fromDowntown.onOff = true;
        }
    }
}
