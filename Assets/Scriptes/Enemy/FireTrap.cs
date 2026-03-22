using System.Collections;
using UnityEngine;

public class FireTrap:MonoBehaviour
{
    [SerializeField] private float damage;

    [Header("FireTrap Timers")]
    [SerializeField] private float activationDelay;
    [SerializeField] private float activeTime;
    private Animator animator;
    private SpriteRenderer spriteRend;

    private bool triggered;
    private bool active;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!triggered)
            {
                StartCoroutine(ActivateFiretrap());
            }
            if (active)
            {
                collision.GetComponent<Health>().takeDamage(damage);
            }
        }
    }

    private IEnumerator ActivateFiretrap()
    {
        triggered = true;
        spriteRend.color = Color.red;

        yield return new WaitForSeconds(activationDelay);
        spriteRend.color = Color.white;
        active = true;
        animator.SetBool("activated",true); 

        yield return new WaitForSeconds(activeTime);
        active = false;
        triggered = false;
        animator.SetBool("activated", false);
    }
}
