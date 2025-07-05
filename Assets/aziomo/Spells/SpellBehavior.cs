using UnityEngine;


public abstract class SpellBehavior : MonoBehaviour
{
    public string spellName;
    public int damage;
    public float cooldown;
    public abstract void CastSpell();
}

public abstract class TargetedSpell : SpellBehavior
{
    public GameObject target;
}

public abstract class AreaOfEffectSpell : SpellBehavior
{
    public float growthRate;
}

public abstract class GlobalSpell : SpellBehavior
{
}