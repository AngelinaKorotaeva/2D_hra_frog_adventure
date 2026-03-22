using UnityEngine;

public class CameraMovement : MonoBehaviour 
{
    [SerializeField]private float speed;
    private float currentPositionX;
    private float currentPositionY;
    private Vector3 velocity = Vector3.zero;

    [SerializeField] private Transform player;
    [SerializeField] private Transform background;

    private void Update()
    {
        //transform.position = new Vector2(player.position.x, player.position.y);
        Vector2 newPosition = new Vector2(player.position.x, player.position.y);
        if (background != null)
        {
            background.position = new Vector3(newPosition.x, newPosition.y,1);
        }
        transform.position = newPosition;
    }
}
