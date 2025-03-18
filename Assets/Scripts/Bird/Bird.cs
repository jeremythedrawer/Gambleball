using UnityEngine;

public class Bird : MonoBehaviour
{
    public float speed;

    private SpriteRenderer spriteRenderer;
    private bool flyingForwards;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        flyingForwards = transform.position.x < ScreenRangeData.bottomLeftWorldPos.x;
    }

    private void Update()
    {
        float moveDirection = flyingForwards ? 1 : -1;
        spriteRenderer.flipX = flyingForwards;
        transform.position += Vector3.right * moveDirection * speed * Time.deltaTime;

        if (flyingForwards)
        {
            if (transform.position.x > ScreenRangeData.topRightWoldPos.x)
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            if (transform.position.x < ScreenRangeData.bottomLeftWorldPos.x)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
