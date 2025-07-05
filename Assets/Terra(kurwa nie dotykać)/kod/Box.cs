using UnityEngine;

public class Box : MonoBehaviour, IHealth{
    public int health = 4;
    public void Damage(int damage){
        health -= damage;
        if (health <= 0){
            Death();
        }
    }
    protected virtual void Death(){
        GetComponent<DamageEffect>().TriggerDamageSplash();
        Destroy(gameObject);
    }
    public void Heal(int heal){}
    public void Resurrect(){}
}
