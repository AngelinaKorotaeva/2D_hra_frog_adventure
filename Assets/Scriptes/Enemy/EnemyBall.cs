using UnityEngine;

public class EnemyBall : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private int damage;
    private float direction;
    private float lifetime;
    private bool hit;

    private BoxCollider2D boxCollider;
    private Animator animator;
    private Health playerHealth;
    [SerializeField] private Transform player;
    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        playerHealth = player.GetComponent<Health>();
    }
    private void Update()
    {
        if (hit) return;
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed,0,0);

        lifetime += Time.deltaTime;
        if (lifetime > 5)
        {
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "Ground")
        {
            hit = true;
            boxCollider.enabled = false;
            animator.SetTrigger("Explode");
            if (collision.tag == "Player")
            {
                playerHealth.takeDamage(damage);
            }
        }
    }
    public void SetDirection(float _direction)
    {
        lifetime = 0;
        direction = -_direction;
        gameObject.SetActive(true);
        hit = false;
        boxCollider.enabled = true;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * Mathf.Sign(_direction);
        transform.localScale = scale;
    }

    private void Deactive()
    {
        gameObject.SetActive(false);
    }
}
