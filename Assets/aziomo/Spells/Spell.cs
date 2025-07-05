using UnityEngine;

public enum SpellType
{
    Targeted,
    AreaOfEffect
}

public abstract class SpellBehavior : MonoBehaviour
{
    public string spellName;
    public int spellDamage;
    public float spellCooldown;
    public SpellType spellType;

    public abstract void CastSpell();

}