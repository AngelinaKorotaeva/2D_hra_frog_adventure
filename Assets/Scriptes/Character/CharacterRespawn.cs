using UnityEditor;
using UnityEngine;
using System.Collections;
using UnityEditor.PackageManager;

public class CharacterRespawn: MonoBehaviour
{
    [SerializeField] private GameObject finishObject;
    [SerializeField] private int scoreSpot;
    private Health playerHealth;
    private Rigidbody2D rb;
    private Vector3 checkPointPosition;
    private UIManager uiManager;
    private Animator anim;
    private Score playerScore;

    [System.Obsolete]
    private void Awake()
    {
        playerHealth = GetComponent<Health>();
        rb = GetComponent<Rigidbody2D>();
        checkPointPosition = transform.position;
        uiManager = FindObjectOfType<UIManager>();
        anim = GetComponent<Animator>();
        playerScore = GetComponent<Score>();
        var finishName = finishObject.name;

        if (finishName.Contains("Portal"))
        {
            anim.Play("Anim");
        }
    }

    public void Respawn()
    {
        playerHealth.RespawnHealth();

        if (playerHealth.currentHealth == 0)
        {
            StartCoroutine(PlayDeathAndDisable());
            return;
        }
        transform.position = checkPointPosition;
        
        GetComponent<CharacterMovement>().enabled = true;
    }
    private IEnumerator PlayDeathAndDisable()
    {
        InfoCharacter.Instance.Coins = 0;
        anim.SetTrigger("die");                
                                                   
        rb.linearVelocity = Vector2.zero;
        this.enabled = false; 

        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);

        uiManager.GameOver();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "CheckPoint")
        {
            playerScore.AddScore(scoreSpot);
            if (collision.gameObject == finishObject)
            {
                InfoCharacter.Instance.CoinsCompleted += InfoCharacter.Instance.Coins;
                InfoCharacter.Instance.Coins = 0;
                finishObject.GetComponent<Collider2D>().enabled = false;

                if (collision.gameObject.name.Contains("Portal"))
                {
                    uiManager.FinichScreen();
                }
                else if (collision.gameObject.name.Contains("CheckPointFinish"))
                {
                    InfoCharacter.Instance.LevelCompleted = InfoCharacter.Instance.LevelCompleted + 1;
                    finishObject.GetComponent<Animator>().SetTrigger("appear");
                    Animator finishAnimator = collision.GetComponent<Animator>();
                    
                    StartCoroutine(FinishSequence(finishAnimator));
                    
                }
                return;
            }
            else
            {
                checkPointPosition = collision.transform.position;
                collision.GetComponent<Collider2D>().enabled = false;
                collision.GetComponent<Animator>().SetTrigger("appear");

                Animator checkpointAnimator = collision.GetComponent<Animator>();
                StartCoroutine(WaitForAnimationToEnd(checkpointAnimator));
            }
        }
        if (collision.CompareTag("FallDown"))
        {
            Respawn();
            
        }
    }
    private IEnumerator FinishSequence(Animator finishAnimator)
    {
        yield return null;
        yield return WaitForAnimationToEnd(finishAnimator);

        uiManager.FinichScreen(); 
    }

    private IEnumerator WaitForAnimationToEnd(Animator checkpointAnimator)
    {
        while (checkpointAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }

        checkpointAnimator.SetTrigger("isActive");
    }
}

