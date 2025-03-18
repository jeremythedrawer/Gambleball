using System.Collections;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public float speed;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidbodyBird;
    private Animator animator;
    private AnimatorStateInfo currentAnimStateInfo;
    private bool flyingForwards;

    private string deadAnimState = "dead";
    private string flyingAnimState = "flying";
    private Ball activeBall => LevelManager.Instance.activeBall;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rigidbodyBird = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        flyingForwards = transform.position.x < ScreenRangeData.bottomLeftWorldPos.x;
    }

    private void Update()
    {
        currentAnimStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        UpdatePos();
        BirdDead();
    }

    private void UpdatePos()
    {
        float moveDirection = flyingForwards ? 1 : -1;
        spriteRenderer.flipX = flyingForwards;
        transform.position += Vector3.right * moveDirection * speed * Time.deltaTime;

        if (flyingForwards)
        {
            if (transform.position.x > ScreenRangeData.topRightWoldPos.x ||
            transform.position.y < ScreenRangeData.bottomLeftWorldPos.y)
            {
                ResetBird();
                gameObject.SetActive(false);
            }
        }
        else
        {
            if (transform.position.x < ScreenRangeData.bottomLeftWorldPos.x || 
            transform.position.y < ScreenRangeData.bottomLeftWorldPos.y)
            {
                ResetBird();
                gameObject.SetActive(false);
            }
        }
    }

    private void BirdDead()
    {
        if (activeBall.playerHitBird && !currentAnimStateInfo.IsName(deadAnimState))
        {
            animator.Play(deadAnimState, 0, 0);
            StartCoroutine(RemovingConstraints());
        }
    }

    private IEnumerator RemovingConstraints()
    {
        yield return new WaitForSeconds(0.2f);
        rigidbodyBird.constraints = RigidbodyConstraints2D.None;
    }
    public void ResetBird()
    {
        animator.Play(flyingAnimState, 0, 0);
        rigidbodyBird.constraints = RigidbodyConstraints2D.FreezeAll;
    }
}
