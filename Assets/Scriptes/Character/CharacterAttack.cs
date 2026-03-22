using System.Threading;
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private GameObject tongueHitbox;
    private int damageValue;
    [SerializeField] private int attackRangeX;
    [SerializeField] private int attackRangeY;
    private Animator animator;
    private Transform attackPoint;
    private int attackRange = 3;
    [SerializeField] private LayerMask enemyLayer;
    private float cooldownTimer = Mathf.Infinity;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        attackPoint = tongueHitbox.transform;
        damageValue = InfoCharacter.Instance.AttackValue;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && cooldownTimer > attackCooldown) {
            Attack();
        }
        cooldownTimer += Time.deltaTime;
    }

    private void Attack()
    {
        animator.SetTrigger("attack");
        cooldownTimer = 0;
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPoint.position, new Vector2(attackRangeX, attackRangeY), 
    0f, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            var neutral = enemy.GetComponent<Nps_neultral>();
            if (neutral != null) neutral.TakeDamage(damageValue);

            var aggressive = enemy.GetComponent<Enemy_npc>();
            if (aggressive != null) aggressive.takeDamage(damageValue);
        }
    }
    public void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
    public void EnableTongue()
    {
        tongueHitbox.SetActive(true);
    }

    public void DisableTongue()
    {
        tongueHitbox.SetActive(false);
    }
}
