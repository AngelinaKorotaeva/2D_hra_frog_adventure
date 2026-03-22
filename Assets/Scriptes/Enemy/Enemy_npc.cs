using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;
using UnityEditor;
using Unity.VisualScripting;
using System.Diagnostics;
using System;

public class Enemy_npc : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float patrolDistance;
    [SerializeField] private Transform player;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float attackRange;
    [SerializeField] private float detectionRange;
    [SerializeField] private int damage;
    [SerializeField] private float startingHealth;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private int scoreValue;
    public float currentHealth { get; private set; }
    private SpriteRenderer _renderer;
    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfflashes;

    private Health playerHealth;

    private Animator animator;

    private bool movingLeft = true;

    private float leftEdge;
    private float rightEdge;

    private bool isWaiting = false;
    private bool isRunning = false;
    private bool isStunned = false;
    private bool isAttacking = false;
    private bool dead = false;
    private Coroutine attackRoutine;
    private float originalSpeed;
    private bool lookingLeft = true;
    private bool isTurning = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        leftEdge = transform.position.x - patrolDistance;
        rightEdge = transform.position.x;
        _renderer = GetComponent<SpriteRenderer>();
        currentHealth = startingHealth;
        playerHealth = player.GetComponent<Health>();
        originalSpeed = speed;
    }
    private void Update()
    {
        if (isStunned) return;
        float distanceToPlayer = Vector2.Distance(transform.position, playerHealth.transform.position);

        if (seePlayer())
        {
            if (distanceToPlayer > attackRange && !isAttacking)
            {
                animator.SetTrigger("Run");
                if (!isRunning)
                {
                    animator.SetBool("IsMoving", false);
                    isRunning = true;
                }
                if (attackRoutine == null)
                {
                    attackRoutine = StartCoroutine(AttackPlayer());
                }
                return;
            }
            return;
        } else if (isRunning)
        {
            animator.Play("IdleAnimation");
            isRunning = false;

        }

        if (!isRunning)
        {
            speed = originalSpeed;
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
                    animator.SetBool("IsMoving", true);
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
                    animator.SetBool("IsMoving", true);
                }
                else
                {
                    isWaiting = true;
                    StartCoroutine(WaitAtEdge(true));
                }
            }
        }
    }
    private void FacePlayer()
    {
        //UnityEngine.Debug.Log("face player to " + lookingLeft);
        //UnityEngine.Debug.Log("player: " + player.position.x + ", enemy: " + transform.position.x);
        Vector3 localScale = transform.localScale;

        if (player.position.x < transform.position.x && !lookingLeft)
        {
            localScale.x = Mathf.Abs(localScale.x); // Поворачиваем влево
            transform.localScale = localScale;
            lookingLeft = true;
        }
        else if (player.position.x > transform.position.x && lookingLeft)
        {
            localScale.x = -Mathf.Abs(localScale.x); // Поворачиваем вправо 
            transform.localScale = localScale;
            lookingLeft = false;
        }
    }
    private IEnumerator WaitAtEdge(bool turnLeft)
    {
        isWaiting = true;
        animator.SetBool("IsMoving", false);

        yield return new WaitForSeconds(3f);
        movingLeft = turnLeft;

        Vector3 localScale = transform.localScale;
        if (turnLeft)
        {
            localScale.x = Mathf.Abs(localScale.x); // Лицом влево
            lookingLeft = true;
        }
        else
        {
            localScale.x = -Mathf.Abs(localScale.x); // Лицом вправо
            lookingLeft = false;
        }
        transform.localScale = localScale;

        isWaiting = false;
    }

    private IEnumerator AttackPlayer()
    {
        isAttacking = true;

        speed = speed * 3;

        while (seePlayer() && Vector2.Distance(transform.position, playerHealth.transform.position) > attackRange)
        {
            FacePlayer();
            Vector2 dir = (playerHealth.transform.position - transform.position).normalized;
            transform.position += (Vector3)(dir * speed * Time.deltaTime);
                yield return null;
        }
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
        attackRoutine = null;
    }
    private IEnumerator AttackCooldown()
    {
        isStunned = true;
        animator.Play("IdleAnimation");
        yield return new WaitForSeconds(attackCooldown + 1);
        isStunned = false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            damagePlayer();
            StartCoroutine(AttackCooldown());
        }
    }

    private bool seePlayer()
    {
        float distance = Vector2.Distance(transform.position, playerHealth.transform.position);
        if (distance > detectionRange)
        {
            return false;
        }

        Vector2 direction = (player.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionRange, obstacleMask);

        if (hit.collider != null && hit.collider.gameObject != player.gameObject)
        {
            return false;
        }

        RaycastHit2D enemyGround = Physics2D.Raycast(transform.position, Vector2.down, 2f, LayerMask.GetMask("Ground"));
        RaycastHit2D playerGround = Physics2D.Raycast(player.position, Vector2.down, 2f, LayerMask.GetMask("Ground"));

        if (enemyGround.collider == null)
        {
            return false;
        }
        if (playerGround.collider == null)
        {
            return false;
        }

        if (enemyGround.collider.gameObject != playerGround.collider.gameObject)
        {
            return false;
        }

        return true;
    }
    private void damagePlayer()
    {
        if (seePlayer())
        {
            playerHealth.takeDamage(damage);
        }
    }
    public void takeDamage(int _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
        if (currentHealth > 0)
        {
            //if (pocetDamageZaScore == 0)
            //{
            //    scoreValue = 2500;
            //}
            //else if (pocetDamageZaScore == 1)
            //{
            //    scoreValue = 2000;
            //}
            animator.SetTrigger("Hurt");
            StartCoroutine(Invulnerability());
        }
        else
        {
            if (!dead)
            {
                FindObjectOfType<Score>().AddScore(scoreValue);
                StartCoroutine(DieAnimation());
                dead = true;
            }
        }
    }
    //// неуязвимость / перезагрузка
    private IEnumerator DieAnimation()
    {
        isStunned = true;
        animator.SetTrigger("Die");
        yield return null;
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }
        isStunned = false;
        Destroy(gameObject);
    }
    private IEnumerator Invulnerability()
    {
        Physics2D.IgnoreLayerCollision(8, 9, true);
        for (int i = 0; i < numberOfflashes; i++)
        {
            _renderer.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfflashes * (float)2.5));
            _renderer.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfflashes * (float)2.5));
        }
        Physics2D.IgnoreLayerCollision(8, 9, false);
    }
}
