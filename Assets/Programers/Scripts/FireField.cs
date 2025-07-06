using System.Collections;
using UnityEngine;

public class FireField : AreaOfEffectSpell
{
    [SerializeField] private BoxCollider2D boxCollider2D;
    [SerializeField] private int damage;

    private void Start()
    {
        StartCoroutine(FireTick());
        boxCollider2D.enabled = false;
    }

    private IEnumerator FireTick()
    {
        int tick = 10;
        while (true)
        {
            if (tick > 0)
            {
                tick--;
            }
            else
            {
                boxCollider2D.enabled = !boxCollider2D.enabled;
                if (boxCollider2D.enabled)
                {
                    tick = 2;
                }
                else
                {
                    tick = 22;
                }
            }
            
            yield return new WaitForFixedUpdate();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerFighterStats>())
        {
            collision.GetComponent<IHealth>().Damage(damage);
        }
    }

    public override void CastSpell()
    {
        return;
    }
}
