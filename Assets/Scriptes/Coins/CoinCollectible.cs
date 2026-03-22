using UnityEngine;
using System.Collections;

public class CoinCollectible: MonoBehaviour
{
    [SerializeField] private int coinValue;
    [SerializeField] private int scoreValue;
    [SerializeField] private int activeTime;
    private Animator animator;
    private bool collect;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(JumpAnimation());
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Coin>().AddMoney(coinValue);
            gameObject.SetActive(false);
            if (!collect)
            {
                collision.GetComponent<Score>().AddScore(scoreValue);
                collect = true;
                animator.SetTrigger("Collect");
            }
        }
    }

    public int getCoinValue()
    {
        return this.coinValue;  
    }

    private IEnumerator JumpAnimation()
    {
        while (!collect)
        {
            yield return new WaitForSeconds(activeTime);
            animator.SetBool("Jump", true);
            yield return new WaitForSeconds(1f);
            animator.SetBool("Jump", false);
        }
    }
}
