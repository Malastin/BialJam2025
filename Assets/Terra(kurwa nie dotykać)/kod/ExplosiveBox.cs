using UnityEngine;

public class ExplosiveBox : Box{
    protected override void Death(){
        GetComponent<DamageEffect>().TriggerDamageSplash();
        Debug.Log("BOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOM");
        Destroy(gameObject);
    }
}
