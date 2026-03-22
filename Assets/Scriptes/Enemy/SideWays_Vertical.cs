using UnityEngine;

public class SideWays_Vertical : MonoBehaviour
{
    [SerializeField] private float movementDistance;
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    private bool movingUp;
    private float upEdge;
    private float downEdge;

    private void Awake()
    {
        upEdge = transform.position.y + movementDistance;
        downEdge = transform.position.y - movementDistance;
    }
    private void Update()
    {

        if (movingUp)
        {
            if (transform.position.y < upEdge)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + speed * Time.deltaTime,
                    transform.position.z);
            }
            else movingUp = false;
        }
        else
        {
            if (transform.position.y > downEdge)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - speed * Time.deltaTime,
                    transform.position.z);
            }
            else movingUp = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Health>().takeDamage(damage);
        }
    }
}
