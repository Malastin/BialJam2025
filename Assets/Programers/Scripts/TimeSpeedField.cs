using UnityEngine;

public class TimeSpeedField : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerFighterStats>())
        {
            collision.GetComponent<PlayerFighterStats>().movementSpeed *= 2f;
            collision.GetComponent<PlayerFighterStats>().attackSpeed *= 2f;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerFighterStats>())
        {
            collision.GetComponent<PlayerFighterStats>().SetPlayerMovespeedToBase();
            collision.GetComponent<PlayerFighterStats>().SetPlayerAttackSpeedToBase();
        }
    }
}
