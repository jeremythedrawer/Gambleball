using System.Collections;
using System.Linq;
using UnityEngine;

public class Bird : MonoBehaviour
{
    [Header("Parameters")]
    public float speed;
    public int[] levelsToSpawn;

    [Header("References")]
    public SpriteRenderer spriteRenderer;
    public Rigidbody2D body;
    public Collider2D col;
    public Animator animator;
    public Basket basket;

    [Header("Start and End Points")]
    public Transform bottomLeftTransform;
    public Transform topRightTransform;

    private Transform chosenTransform;

    private AnimatorStateInfo currentAnimStateInfo;

    private Vector2 moveDirection;

    private string deadAnimState = "dead";
    private string flyingAnimState = "flying";

    private bool isDead;
    private bool inView;

    private float prevRandomYPos;
    private bool hasResetFlag; 
    private void Start()
    {
        SetBirdPos();
    }

    private void Update()
    {
        currentAnimStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        UpdatePos();

        inView =    transform.position.x > bottomLeftTransform.position.x &&
                    transform.position.x < topRightTransform.position.x &&
                    transform.position.y > bottomLeftTransform.position.y;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Dead();
        }  
    }

    private void UpdatePos()
    {
        if ((levelsToSpawn.Any(level => level == LevelManager.instance.currentLevelIndex + 1) || inView) && !isDead)
        {
            transform.position += (Vector3)moveDirection * speed * Time.deltaTime;
            hasResetFlag = false;
        }
        else if (((Mathf.Abs(transform.position.y - prevRandomYPos) < 0.01f && !isDead) || (isDead && !inView)) && !hasResetFlag)
        {
            ResetBird();
        }
    }

    private void Dead()
    {
        animator.Play(deadAnimState, 0, 0);
        StartCoroutine(Dying());
        isDead = true;
    }

    private IEnumerator Dying()
    {
        yield return new WaitForSeconds(0.2f);
        body.constraints = RigidbodyConstraints2D.None;
        col.enabled = false;
    }
    private void ResetBird()
    {
        hasResetFlag = true;
        SetBirdPos();
        animator.Play(flyingAnimState, 0, 0);
        body.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.eulerAngles = Vector2.zero;
    }

    private void SetBirdPos()
    {
        float randomY = Random.Range(basket.transform.position.y, topRightTransform.position.y);
        prevRandomYPos = randomY;
        chosenTransform = Random.value < 0.5f ? bottomLeftTransform : topRightTransform;

        transform.position = new Vector3(chosenTransform.position.x, prevRandomYPos, 0);
        spriteRenderer.flipX = chosenTransform == bottomLeftTransform;
        moveDirection = chosenTransform == bottomLeftTransform ? Vector2.right : Vector2.left;
    }
}
