using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using UnityEditor;

public class Nps_neultral : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float patrolDistance;
    [SerializeField] private int damage;
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    [SerializeField] private int scoreValue;
    [SerializeField] private Transform player;

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfflashes;

    private Animator animator;
    private bool movingLeft = true;

    private float leftEdge;
    private float rightEdge;
    private bool isTurning = false;
    private bool isWaiting = false;
    private bool isHit = false;
    private Rigidbody2D rb;
    private Health playerHealth;
    private SpriteRenderer spriteRend;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        leftEdge = transform.position.x - patrolDistance;
        rightEdge = transform.position.x;
        currentHealth = startingHealth;
        playerHealth = player.GetComponent<Health>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (isHit || isWaiting) return;
        if (movingLeft)
        {
            if (transform.position.x > leftEdge)
            {
                if (isTurning)
                {
                    Vector3 localScale = transform.localScale;
                    localScale.x = -Mathf.Abs(localScale.x);
                    transform.localScale = localScale;
                    isTurning = false;
                }
                transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y,
                    transform.position.z);
                animator.SetBool("Go", true);
            }
            else
            {
                isWaiting = true;
                StartCoroutine(WaitAtEdge(false));
            }
        }
        else
        {
            if (transform.position.x < rightEdge)
            {
                if (isTurning)
                {
                    Vector3 localScale = transform.localScale;
                    localScale.x = Mathf.Abs(localScale.x);

                    isTurning = false;
                }
                transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y,
                transform.position.z);
                animator.SetBool("Go", true);
            }
            else
            {
                isWaiting = true;
                StartCoroutine(WaitAtEdge(true));
            }
        }

    }

    private IEnumerator WaitAtEdge(bool turnLeft)
    {
        isWaiting = true;
        animator.SetBool("Go", false);

        yield return new WaitForSeconds(3f);
        movingLeft = turnLeft;

        Vector3 localScale = transform.localScale;
        if (turnLeft)
        {
            localScale.x = Mathf.Abs(localScale.x);
        }
        else
        {
            localScale.x = -Mathf.Abs(localScale.x);
        }
        transform.localScale = localScale;

        isWaiting = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerHealth.takeDamage(damage);
        }
    }
    public void TakeDamage(int amount)
    {
        if (currentHealth <= 0 || isHit) return;

        currentHealth -= amount;
        
        if (currentHealth <= 0)
        {
            FindObjectOfType<Score>().AddScore(scoreValue);
            speed = 0f;
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.simulated = false;
            }
            StartCoroutine(DieAnimation());

        } else
        {
            StartCoroutine(HandleHit());
        }
    }
    private IEnumerator DieAnimation()
    {
        animator.SetTrigger("Die");
        yield return null;
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }
        Destroy(gameObject);
    }
    public void OnDeathAnimationEnd()
    {
        Destroy(gameObject);
    }
    private IEnumerator HandleHit()
    {
        isHit = true;
        animator.SetTrigger("Hit");
        animator.SetBool("Go", false);

        float originalSpeed = speed;
        speed = 0f;
        StartCoroutine(Invulnerability());
        yield return new WaitForSeconds(iFramesDuration);
        animator.ResetTrigger("Hit");
        speed = originalSpeed;
        animator.SetBool("Go", true);
        isHit = false;
    }
    private IEnumerator Invulnerability()
    {
        Physics2D.IgnoreLayerCollision(8, 9, true); 

        for (int i = 0; i < numberOfflashes; i++)
        {
            spriteRend.color = new Color(1, 1, 1, 0.5f); 
            yield return new WaitForSeconds(iFramesDuration / (numberOfflashes * 2f));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfflashes * 2f));
        }

        Physics2D.IgnoreLayerCollision(8, 9, false);
    }
}
