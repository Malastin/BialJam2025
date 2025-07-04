using UnityEngine;

public class ExplosionField : MonoBehaviour
{
    public float explosionPower;
    public int explosionDamage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.GetComponent<PlayerFighterStats>().DealDamageToPlayer(explosionDamage);
        if (collision.GetComponent<Rigidbody2D>())
        {
            float posX = collision.transform.position.x - transform.position.x;
            float posY = collision.transform.position.y - transform.position.y;

            float power;

            if (Mathf.Abs(posX) > Mathf.Abs(posY))
            {
                power = 1f * (Mathf.Abs(posY) / Mathf.Abs(posX));
                if (posX > 0)
                {
                    if (posY > 0)
                    {
                        collision.GetComponent<Rigidbody2D>().linearVelocity += new Vector2(explosionPower, explosionPower * power);
                    }
                    else
                    {
                        collision.GetComponent<Rigidbody2D>().linearVelocity += new Vector2(explosionPower, -explosionPower * power);
                    }
                }
                else
                {
                    if (posY > 0)
                    {
                        collision.GetComponent<Rigidbody2D>().linearVelocity += new Vector2(-explosionPower, explosionPower * power);
                    }
                    else
                    {
                        collision.GetComponent<Rigidbody2D>().linearVelocity += new Vector2(-explosionPower, -explosionPower * power);
                    }
                }
            }
            else
            {
                power = 1f * (Mathf.Abs(posX) / Mathf.Abs(posY));
                if (posY > 0)
                {
                    if (posX > 0)
                    {
                        collision.GetComponent<Rigidbody2D>().linearVelocity += new Vector2(explosionPower * power, explosionPower);
                    }
                    else
                    {
                        collision.GetComponent<Rigidbody2D>().linearVelocity += new Vector2(-explosionPower * power, explosionPower);
                    }
                }
                else
                {
                    if (posY > 0)
                    {
                        collision.GetComponent<Rigidbody2D>().linearVelocity += new Vector2(explosionPower * power, -explosionPower);
                    }
                    else
                    {
                        collision.GetComponent<Rigidbody2D>().linearVelocity += new Vector2(-explosionPower * power, -explosionPower);
                    }
                }
            }

            //Destroy(gameObject);
        }
    }
}
