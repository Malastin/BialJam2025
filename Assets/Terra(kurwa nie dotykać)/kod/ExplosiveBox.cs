using UnityEngine;

public class ExplosiveBox : Box{

    [SerializeField] private GameObject explosionObj;
    [SerializeField] private GameObject particles;
    [SerializeField] private GameObject particles2;

    protected override void Death()
    {
        GetComponent<DamageEffect>().TriggerDamageSplash();

        var spawnExplosion = Instantiate(explosionObj, transform.parent);
        spawnExplosion.transform.position = transform.position;
        spawnExplosion.transform.localScale = new Vector3(3, 3, 1);
        var spawnParticles = Instantiate(particles, transform.parent);
        spawnParticles.transform.position = transform.position;
        var spawnParticles2 = Instantiate(particles2, transform.parent);
        spawnParticles2.transform.position = transform.position;
        
        Destroy(gameObject);
    }
}
