using UnityEngine;

public class ExplosionField : AreaOfEffectSpell
{
    public float explosionPower;
    public int explosionDamage;
    [SerializeField] private bool destroyMe;

    public override void CastSpell()
    {
        return;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out IHealth health))
        {
            health.Damage(explosionDamage);
        }
        // collision.GetComponent<IHealth>().Damage(explosionDamage);
        if (collision.GetComponent<Rigidbody2D>() != null)
        {
            float posX = collision.transform.position.x - transform.position.x;

            if (posX >= 0)
            {
                collision.GetComponent<Rigidbody2D>().linearVelocity += new Vector2(explosionPower, 0);
            }
            else
            {
                collision.GetComponent<Rigidbody2D>().linearVelocity += new Vector2(-explosionPower, 0);
            }

            if (destroyMe)
            {
                Destroy(gameObject);
            }
        }
    }
}
