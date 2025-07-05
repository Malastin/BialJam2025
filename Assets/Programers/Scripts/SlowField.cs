using UnityEngine;

public class SlowField : AreaOfEffectSpell
{
    public override void CastSpell()
    {
        return;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerFighterStats>())
        {
            collision.GetComponent<PlayerFighterStats>().movementSpeed *= 0.5f;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerFighterStats>())
        {
            collision.GetComponent<PlayerFighterStats>().SetPlayerMovespeedToBase();
        }
    }
}
