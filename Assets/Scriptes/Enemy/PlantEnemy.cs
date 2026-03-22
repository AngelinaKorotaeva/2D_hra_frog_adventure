using UnityEngine;
using System.Collections;
using NUnit.Framework.Constraints;
using UnityEngine.UIElements.Experimental;
using System;

public class PlantEnemy : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float startingHealth;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float detectionRange;
    [SerializeField] private float yTolerance;

    [SerializeField] private Transform point;
    [SerializeField] private GameObject[] enemyBalls;

    [SerializeField] private LayerMask playerMask;
    public float currentHealth { get; private set; }
    [SerializeField] private int scoreValue;
    [SerializeField] private Transform player;

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfflashes;

    private Animator animator;
    private bool isHit = false;
    private bool seePlayer = false;
    private Rigidbody2D rb;
    private Health playerHealth;
    private SpriteRenderer spriteRend;

    private float attackTimer;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = startingHealth;
        playerHealth = player.GetComponent<Health>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (isHit) return;
        if (SeePlayer())
        {
            attackTimer -= Time.deltaTime;
            LookAtPlayer();
            if (attackTimer <= 0f)
            {
                Attack();
                attackTimer = attackCooldown;
            }
        }
    }

    private void Attack()
    {

        animator.SetTrigger("SeePlayer");

        int index = FindEnemyBall();
        if (index == -1)
            return;
        GameObject ball = enemyBalls[index];
        ball.transform.position = point.position;
        float direction = Mathf.Sign(transform.localScale.x);
        ball.GetComponent<EnemyBall>().SetDirection(direction);
    }
    private int FindEnemyBall()
    {
        for (int i = 0; i < enemyBalls.Length; i++)
        {
            if (!enemyBalls[i].activeInHierarchy)
            {
                return i;
            }
        }
        return -1;
    }
    private bool SeePlayer()
    {
        if (player == null) return false;
        float xDistance = Mathf.Abs(player.position.x - transform.position.x);
        float yDistance = Mathf.Abs(player.position.y - transform.position.y);

        return xDistance <= detectionRange && yDistance <= yTolerance;
    }
    private void LookAtPlayer()
    {
        if (player == null) return;

        float direction = player.position.x - transform.position.x;

        if (direction != 0)
        {
            Vector3 scale = transform.localScale;

            scale.x = -Mathf.Sign(direction) * Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
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
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.simulated = false;
            }
            StartCoroutine(DieAnimation());

        }
        else
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

        StartCoroutine(Invulnerability());
        yield return new WaitForSeconds(iFramesDuration);
        animator.ResetTrigger("Hit");
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
