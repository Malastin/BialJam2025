using UnityEngine;

public class PodstawowyAtakGracza : MonoBehaviour
{
    public GameObject caster;

    private void Start()
    {
        Destroy(gameObject, 0.1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerFighterStats>() && collision.gameObject != caster)
        {
            collision.GetComponent<PlayerFighterStats>().DealDamageToPlayer(caster.GetComponent<PlayerFighterStats>().baseDamage);
            Destroy(gameObject);
        }
    }
}
