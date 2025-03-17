using UnityEngine;

public class ScoreTrigger : MonoBehaviour
{
    public BoxCollider2D triggerCollider {  get; private set; }
    private void Start()
    {
        triggerCollider = GetComponent<BoxCollider2D>();
    }
}
