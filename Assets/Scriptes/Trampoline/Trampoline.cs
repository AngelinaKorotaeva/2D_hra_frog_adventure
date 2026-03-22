using System.Collections;
using UnityEngine;

public class Trampoline:MonoBehaviour
{
    [SerializeField] private float bounceForce;
    [SerializeField] private float activationDelay;
    private Animator animator;
    private SpriteRenderer spriteRend;

    private bool triggered;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>(); 
            if (!triggered)
            {
                StartCoroutine(ActivateTrm());
                if (rb != null) 
                {
                    Animator playerAnim = collision.GetComponent<Animator>();
                    if (playerAnim != null)
                    {
                        playerAnim.SetTrigger("Jump");
                    }
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); 
                    rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse); 
                }
            }
            
        }
    }

    private IEnumerator ActivateTrm()
    {
        triggered = true;
        yield return new WaitForSeconds(activationDelay);
        animator.SetBool("activated", true);

        triggered = false;
        animator.SetBool("activated", false);
    }
}
