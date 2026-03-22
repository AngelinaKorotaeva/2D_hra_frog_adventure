using System.Collections;
using UnityEditor.ShaderGraph;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Health: MonoBehaviour
{
    private float startingHealth;
    public float currentHealth { get; private set; }
    private Animator animator;
    private bool dead;

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration; 
    [SerializeField] private int numberOfflashes;
    private int scoreValue;
    private SpriteRenderer spriteRend;
    private int pocetDamageZaScore = 0;
    private UIManager uiManager;
    private Rigidbody2D rb;
    private void Awake()
    {
        startingHealth = InfoCharacter.Instance.Lives;
        currentHealth = startingHealth;
        animator = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
        scoreValue = 3000;
        rb = GetComponent<Rigidbody2D>();
        uiManager = FindObjectOfType<UIManager>();
        Physics2D.IgnoreLayerCollision(8, 9, false);
    }
    public void takeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
        if (currentHealth > 0)
        {
            if (pocetDamageZaScore == 0)
            {
                scoreValue = 2500;
            } else if (pocetDamageZaScore == 1)
            {
                scoreValue = 2000;
            }
                animator.SetTrigger("hurt");
            StartCoroutine(Invulnerability());
        } else
        {
            if (!dead)
            {
                InfoCharacter.Instance.Coins = 0;
                StartCoroutine(PlayDeathAndDisable());
                return;
            }
        }
    }
    private IEnumerator PlayDeathAndDisable()
    {
        dead = true;
        animator.SetTrigger("die");

        rb.linearVelocity = Vector2.zero;
        this.enabled = false;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);
        gameObject.SetActive(false);
        uiManager.GameOver();
    }
    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }

    //мигает перс при получении урона
    private IEnumerator Invulnerability()
    {
        Physics2D.IgnoreLayerCollision(8,9,true);
        for (int i = 0; i < numberOfflashes; i++)
        {
            spriteRend.color = new Color(1,0,0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfflashes * (float)2.5));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfflashes * (float)2.5));
        }
        Physics2D.IgnoreLayerCollision(8, 9, false); 
    }

    public void RespawnHealth()
    {
        dead = false;
        takeDamage(1);
        animator.ResetTrigger("die");
        animator.Play("IdleAnimation");
        StartCoroutine(Invulnerability());
    }
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("FallDown"))
    //    {
    //        takeDamage(1);
    //        RespawnHealth();
    //    }
    //}
}