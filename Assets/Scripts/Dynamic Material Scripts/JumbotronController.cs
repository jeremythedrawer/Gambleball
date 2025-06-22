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
        BallSpawner.onInBasket += ChooseSign; 
    }
    private void OnDisable()
    {
        BallSpawner.onInBasket -= ChooseSign;
    }

    private void ChooseSign()
    {
        if (BallSpawner.instance.type == BallType.Moneyball)
        {
            moneyBall.onOff = true;
            AudioManager.instance.PlaySFX("moneyball");
        }
        else if (BallSpawner.instance.type == BallType.AttemptBoost)
        {
            attemptBoost.onOff = true;
            AudioManager.instance.PlaySFX("attemptBoost");
        }
        else if (StatsManager.instance.onFire)
        {
            onFire.onOff = true;
            AudioManager.instance.PlaySFX("onFire");
        }
        else if (StatsManager.instance.fromDowntown)
        {
            fromDowntown.onOff = true;
            AudioManager.instance.PlaySFX("fromDowntown");
        }
    }
}
