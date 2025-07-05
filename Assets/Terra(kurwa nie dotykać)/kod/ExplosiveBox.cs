using UnityEngine;

public class ExplosiveBox : Box{
    [SerializeField] private GameObject explosion;

    protected override void Death(){
        GetComponent<DamageEffect>().TriggerDamageSplash();
        Debug.Log("BOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOM");

        var explosionObj = Instantiate(explosion, transform.parent);
        explosion.transform.position = transform.position;
        explosion.transform.localScale = new Vector3(1.5f, 1.5f, 1);
        Destroy(gameObject);
    }
}
