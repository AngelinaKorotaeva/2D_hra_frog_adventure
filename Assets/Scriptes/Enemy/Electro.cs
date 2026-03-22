using UnityEngine;
using System.Collections;

public class Electro : MonoBehaviour
{
    [SerializeField] private float damage ;
    [SerializeField] private float activeDuration;
    [SerializeField] private float cooldownDuration;

    private bool isActive = false;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        StartCoroutine(TrapCycle());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isActive && collision.CompareTag("Player"))
        {
            collision.GetComponent<Health>()?.takeDamage(damage);
        }
    }

    private IEnumerator TrapCycle()
    {
        while (true)
        {
            isActive = true;
            animator.SetBool("activated", true);

            yield return new WaitForSeconds(activeDuration);

            isActive = false;
            animator.SetBool("activated", false);

            yield return new WaitForSeconds(cooldownDuration);
        }
    }
}
