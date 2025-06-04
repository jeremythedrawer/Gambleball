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
    private void Start()
    {
        SetBirdPos();
    }

    private void Update()
    {
        currentAnimStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        UpdatePos();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Dead();
        }
    }

    private void UpdatePos()
    {
        if(levelsToSpawn.Any(level => level == LevelManager.instance.currentLevelIndex + 1))
        {
            transform.position += (Vector3)moveDirection * speed * Time.deltaTime;

            if  ((chosenTransform == bottomLeftTransform && transform.position.x > topRightTransform.position.x) || 
                (!chosenTransform == bottomLeftTransform && transform.position.x < bottomLeftTransform.position.x) || 
                transform.position.y < bottomLeftTransform.position.y)
            {
                ResetBird();
            }
        }
    }


    private void Dead()
    {
        animator.Play(deadAnimState, 0, 0);
        StartCoroutine(SettingBodyToDynamic());
    }

    private IEnumerator SettingBodyToDynamic()
    {
        yield return new WaitForSeconds(0.2f);
        body.bodyType = RigidbodyType2D.Dynamic;
    }
    private void ResetBird()
    {
        animator.Play(flyingAnimState, 0, 0);
        body.bodyType = RigidbodyType2D.Kinematic;
        transform.eulerAngles = Vector2.zero;
        SetBirdPos();
    }

    private void SetBirdPos()
    {
        float randomY = Random.Range(basket.transform.position.y, topRightTransform.position.y);
        chosenTransform = Random.value < 0.5f ? bottomLeftTransform : topRightTransform;

        transform.position = new Vector3(chosenTransform.position.x, randomY, 0);
        spriteRenderer.flipX = chosenTransform == bottomLeftTransform;
        moveDirection = chosenTransform == bottomLeftTransform ? Vector2.right : Vector2.left;
    }
}
