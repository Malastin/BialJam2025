using UnityEngine;

public class WindGust : TargetedSpell
{
    public float gustForce = 10f;

    public override void CastSpell()
    {
        if (target == null)
        {
            Debug.LogWarning("No target specified for WindGust spell.");
            return;
        }

        Debug.Log($"Casting WindGust on {target.name} with force {gustForce} and direction {aimDirection}");
        Vector2 gustDirection = aimDirection.normalized;
        target.GetComponent<Rigidbody2D>().linearVelocity += gustDirection * gustForce;
    }
}