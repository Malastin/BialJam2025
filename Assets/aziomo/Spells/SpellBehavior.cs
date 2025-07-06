using UnityEngine;


public abstract class SpellBehavior : MonoBehaviour
{
    public string spellName;
    public Sprite icon;
    public int spellDamage;
    public float cooldown;
    public abstract void CastSpell();
}

public abstract class TargetedSpell : SpellBehavior
{
    public GameObject target;
    public Vector3 aimDirection;
}

public abstract class AreaOfEffectSpell : SpellBehavior
{
    public float growthRate;
    public float decayTime;
}

public abstract class GlobalSpell : SpellBehavior
{
}