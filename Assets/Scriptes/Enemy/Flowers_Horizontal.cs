using UnityEngine;
using System.Collections;
public class Flowers_Horizzzontal : MonoBehaviour
{
    [SerializeField] private float moveDistance;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float waitTime;
    [SerializeField] private int damage;

    private Vector3 startPos;
    private bool isMoving = false;
    private bool hasDamaged = false;

    private void Start()
    {
        startPos = transform.localPosition;
        StartCoroutine(MoveCycle());
    }

    private IEnumerator MoveCycle()
    {
        while (true)
        {
            if (!isMoving)
            {
                isMoving = true;
                yield return StartCoroutine(MoveTo(startPos + Vector3.left * moveDistance));
                yield return StartCoroutine(MoveTo(startPos));
                hasDamaged = false;
                yield return new WaitForSeconds(waitTime);
                isMoving = false;
            }
            yield return null;
        }
    }

    private IEnumerator MoveTo(Vector3 target)
    {
        while (Vector3.Distance(transform.localPosition, target) > 0.01f)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.localPosition = target;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasDamaged && collision.CompareTag("Player"))
        {
            collision.GetComponent<Health>().takeDamage(damage);
            hasDamaged = true;
        }
    }
}
