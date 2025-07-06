using UnityEngine;

public class Box : MonoBehaviour, IHealth{
    public int health = 4;
    public void Damage(int damage){
        health -= damage;
        GetComponent<DamageEffect>().TriggerDamageSplash();
        if (health <= 0){
            Death();
        }
    }
    protected virtual void Death(){
        SoundManager.PlaySound(SoundType.BoxDestroy);
        Destroy(gameObject);
    }
    public void Heal(int heal){}
    public void Resurrect(){}

    public int GetHealth(){return health;}
}
