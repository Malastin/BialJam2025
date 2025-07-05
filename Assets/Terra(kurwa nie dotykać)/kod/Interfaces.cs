using UnityEngine;

public interface IHealth{
    public void Damage(int damage);
    public void Heal(int heal);
    public int GetHealth();
    public void Resurrect();
}
