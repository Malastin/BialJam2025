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

        float direction = Random.value < 0.5f ? -1f : 1f;
        target.GetComponent<Rigidbody2D>().linearVelocity += new Vector2(gustForce * direction, 0);
    }
}