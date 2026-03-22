using UnityEngine;

public class VerticalPlatform : MonoBehaviour
{
    [SerializeField] private float movementDistance;
    [SerializeField] private float speed;

    private bool movingUp;
    private float upEdge;
    private float downEdge;

    private void Awake()
    {
        float currentY = transform.position.y;
        float targetY = currentY + movementDistance;

        upEdge = Mathf.Max(currentY, targetY);
        downEdge = Mathf.Min(currentY, targetY);

        movingUp = movementDistance > 0;
    }

    private void Update()
    {
        if (movingUp)
        {
            if (transform.position.y < upEdge)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + speed * Time.deltaTime, transform.position.z);
            }
            else
            {
                movingUp = false;
            }
        }
        else
        {
            if (transform.position.y > downEdge)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - speed * Time.deltaTime, transform.position.z);
            }
            else
            {
                movingUp = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.parent = transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.parent = null;
        }
    }
}
