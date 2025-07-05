using UnityEngine;

public class PodstawowyAtakGracza : MonoBehaviour
{
    public GameObject caster;
    public bool killOnTime;
    public int damage;
    public GameObject lastTargetObj;

    private void Start()
    {
        if (killOnTime)
        {
            Destroy(gameObject, 0.1f);
        }
    }

    private void OnDestroy(){
        if (lastTargetObj == null) return;
        CameraShaker.StartCameraShake(2, 0.1f);
        
        if (lastTargetObj.TryGetComponent(out Box box)) {
            lastTargetObj.GetComponent<IHealth>().Damage(damage);
        }
        
        if (lastTargetObj.TryGetComponent(out PlayerController player)){
            if (lastTargetObj.GetComponent<PlayerController>().animationState == PlayerStates.death){
                return;
            }
            if (lastTargetObj.GetComponent<PlayerController>().animationState != PlayerStates.normalAttack)
            {
                lastTargetObj.GetComponent<IHealth>().Damage(damage);
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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        if (collision.TryGetComponent(out IHealth health) && collision.gameObject != caster)
        {
            lastTargetObj = collision.gameObject;
        }
    }
}
