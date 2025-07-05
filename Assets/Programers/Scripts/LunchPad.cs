using UnityEngine;

public class LunchPad : MonoBehaviour
{
    [SerializeField] private float lunchPower;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
            collision.GetComponent<Rigidbody2D>().linearVelocityY += lunchPower;
            collision.GetComponent<Rigidbody2D>().gravityScale = 4f;
        }
    }

}
