using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class SpellController : MonoBehaviour
{
    public SpellBehavior[] spellBehaviors;
    public GameObject spellCrosshair;
    public GameObject spellArea;

    public Vector3 crosshairRestingPosition;
    public float castingSpeed = 0.001f;

    private bool isCasting = false;

    void Start()
    {
        if (spellCrosshair == null)
        {
            Debug.LogError("Spell crosshair is not assigned in the SpellController!");
        }
        else
        {
            crosshairRestingPosition = spellCrosshair.transform.position;
        }
    }

    void Update()
    {
        if (isCasting)
        {
            if (spellCrosshair != null)
            {
                var line = spellCrosshair.GetComponent<LineRenderer>();
                Vector3[] points = new Vector3[2];
                line.GetPositions(points);
                points[0] = points[0] + Vector3.left * castingSpeed;
                points[1] = points[1] + Vector3.right * castingSpeed;
                line.SetPositions(points);
            }
        }

    }

    public void Aim(InputAction.CallbackContext input)
    {
        if (input.performed)
        {
            Vector2 aimDirection = input.ReadValue<Vector2>();
            aimDirection.y = 0;

            // discard input taking into account analog stick deadzone
            if (aimDirection.magnitude < 0.05f)
            {
                aimDirection = Vector2.zero;
            }

            if (spellCrosshair != null)
            {
                spellCrosshair.transform.position = (Vector2)crosshairRestingPosition + aimDirection * 5.0f;
            }
            else
            {
                Debug.LogWarning("Spell crosshair is not assigned!");
            }
        }
    }

    public void CastSpell(InputAction.CallbackContext input)
    {
        var line = spellCrosshair.GetComponent<LineRenderer>();
        spellArea.AddComponent<ExplosionField>();
        if (input.performed)
        {
            line.enabled = true;
            isCasting = true;
        }
        else if (input.canceled)
        {
            isCasting = false;
            ResetSpellAreaOpacity();
            if (true)
            {
                Vector3[] points = new Vector3[2];
                line.GetPositions(points);
                spellArea.transform.position = (points[0] + points[1]) / 2;
                spellArea.transform.position = new Vector3(
                    spellCrosshair.transform.position.x,
                    spellArea.transform.position.y,
                    spellArea.transform.position.z
                );
                spellArea.transform.localScale = new Vector3(
                    Vector3.Distance(points[0], points[1]),
                    spellArea.transform.localScale.y,
                    spellArea.transform.localScale.z
                );

            }
            FadeOutSpellArea();

            line.SetPositions(new Vector3[] { Vector3.zero, Vector3.zero });
            line.enabled = false;
        }
    }


    void FadeOutSpellArea()
    {
        if (spellArea != null)
        {
            var spriteRenderer = spellArea.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                StartCoroutine(FadeToZeroAlpha(spriteRenderer));
            }
            else
            {
                Debug.LogWarning("SpriteRenderer not found on SpellArea!");
            }
        }
    }

    void ResetSpellAreaOpacity()
    {
        if (spellArea != null)
        {
            var spriteRenderer = spellArea.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.5f);
            }
            else
            {
                Debug.LogWarning("SpriteRenderer not found on SpellArea!");
            }
        }
    }

    System.Collections.IEnumerator FadeToZeroAlpha(SpriteRenderer spriteRenderer, float duration = 1f)
    {
        Color startColor = spriteRenderer.color;
        float time = 0f;

        while (time < duration)
        {
            float alpha = Mathf.Lerp(startColor.a, 0f, time / duration);
            spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            time += Time.deltaTime;
            yield return null;
        }

        // Ensure it's fully transparent at the end
        spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
    }
}
