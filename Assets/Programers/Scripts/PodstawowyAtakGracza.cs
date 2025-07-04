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
            collision.GetComponent<PlayerFighterStats>().DealDamageToPlayer(damage);
            Destroy(gameObject);
        }
    }
}
