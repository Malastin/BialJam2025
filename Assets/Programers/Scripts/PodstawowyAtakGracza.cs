using UnityEngine;

public class PodstawowyAtakGracza : MonoBehaviour
{
    public GameObject caster;
    public bool killOnTime;
    public int damage;

    private void Start()
    {
        if (killOnTime)
        {
            Destroy(gameObject, 0.1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerFighterStats>() && collision.gameObject != caster)
        {
            collision.GetComponent<IHealth>().Damage(damage);
            if (transform.position.x < collision.transform.position.x)
            {
                if (!collision.GetComponent<PlayerController>().grabedToWall)
                {
                    collision.GetComponent<Rigidbody2D>().linearVelocityX += 15;
                }
            }
            else
            {
                if (!collision.GetComponent<PlayerController>().grabedToWall)
                {
                    collision.GetComponent<Rigidbody2D>().linearVelocityX -= 15;
                }
            }
            Destroy(gameObject);
        }
    }
}
