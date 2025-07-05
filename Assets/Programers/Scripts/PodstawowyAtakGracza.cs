using UnityEngine;

public class PodstawowyAtakGracza : MonoBehaviour
{
    public GameObject caster;
    public bool killOnTime;
    public int damage;
    public bool doNotDealDmg;
    public IHealth lastTarget;
    public GameObject lastTargetObj;

    private void Start()
    {
        if (killOnTime)
        {
            Destroy(gameObject, 0.1f);
        }
    }

    private void OnDestroy()
    {
        if (lastTarget == null)
        {
            return;
        }
        if (lastTargetObj.GetComponent<PlayerController>().animationState == PlayerStates.death)
        {
            return;
        }
        if (lastTargetObj.GetComponent<PlayerController>().animationState != PlayerStates.normalAttack)
        {
            lastTarget.Damage(damage);
        }
        if (caster.transform.position.x < lastTargetObj.transform.position.x)
        {
            if (!lastTargetObj.GetComponent<PlayerController>().grabedToWall)
            {
                lastTargetObj.GetComponent<Rigidbody2D>().linearVelocityX += 15;
            }
        }
        else
        {
            if (!lastTargetObj.GetComponent<PlayerController>().grabedToWall)
            {
                lastTargetObj.GetComponent<Rigidbody2D>().linearVelocityX -= 15;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerFighterStats>() && collision.gameObject != caster)
        {
            lastTarget = collision.GetComponent<IHealth>();
            lastTargetObj = collision.gameObject;
        }
    }
}
