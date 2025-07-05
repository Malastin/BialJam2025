using UnityEngine;
using UnityEngine.InputSystem;

public enum SpellStage
{
    Idle,
    Targeting,
    Casting
}

public class SpellManager : MonoBehaviour
{
    public GameObject[] spellItems;
    public int selectedSpellIndex = 0;

    public GameObject spellTargetMarker;
    public GameObject spellAreaMarker;
    public GameObject spellArea;
    private SpellStage spellStage = SpellStage.Idle;
    private SpriteRenderer selectedSpellSprite;

    public Vector3 areaMarkerRestingPosition;
    public float castingSpeed = 0.001f;

    private SpellBehavior selectedSpell
    {
        get { return spellItems[selectedSpellIndex].GetComponent<SpellBehavior>(); }
    }

    void Start()
    {
        areaMarkerRestingPosition = spellAreaMarker.transform.position;
        selectedSpellSprite = GetComponent<SpriteRenderer>();
        selectedSpellSprite.sprite = spellItems[selectedSpellIndex].GetComponent<SpriteRenderer>().sprite;
    }

    void Update()
    {
        if (selectedSpell is AreaOfEffectSpell && spellStage == SpellStage.Targeting)
        {
            if (spellAreaMarker != null)
            {
                var line = spellAreaMarker.GetComponent<LineRenderer>();
                Vector3[] points = new Vector3[2];
                line.GetPositions(points);
                points[0] = points[0] + Vector3.left * castingSpeed;
                points[1] = points[1] + Vector3.right * castingSpeed;
                line.SetPositions(points);
            }
        }
    }

    public void NavigateNext(InputAction.CallbackContext input)
    {
        if (input.performed)
        {
            if (spellStage == SpellStage.Idle)
            {
                SelectNextSpell();
            }
            if (spellStage == SpellStage.Targeting)
            {
                if (selectedSpell is TargetedSpell)
                {
                    spellTargetMarker.GetComponent<TargetMarkerController>().ChangeTarget();
                }
            }
        }
    }

    public void NavigatePrevious(InputAction.CallbackContext input)
    {
        if (input.performed)
        {
            if (spellStage == SpellStage.Idle)
            {
                SelectPreviousSpell();
            }
            if (spellStage == SpellStage.Targeting)
            {
                if (selectedSpell is TargetedSpell)
                {
                    spellTargetMarker.GetComponent<TargetMarkerController>().ChangeTarget();
                }
            }
        }
    }

    public void Aim(InputAction.CallbackContext input)
    {
        if (spellStage != SpellStage.Targeting || !(selectedSpell is AreaOfEffectSpell))
        {
            return;
        }
        if (input.performed)
        {
            Vector2 aimDirection = input.ReadValue<Vector2>();
            aimDirection.y = 0;

            // discard input taking into account analog stick deadzone
            if (aimDirection.magnitude < 0.05f)
            {
                aimDirection = Vector2.zero;
            }

            if (spellAreaMarker != null)
            {
                spellAreaMarker.transform.position = (Vector2)areaMarkerRestingPosition + aimDirection * 5.0f;
            }
            else
            {
                Debug.LogWarning("Spell crosshair is not assigned!");
            }
        }
    }

    public void Activate(InputAction.CallbackContext input)
    {
        if (input.performed)
        {
            if (spellStage == SpellStage.Idle)
            {
                spellStage = SpellStage.Targeting;
                if (selectedSpell is TargetedSpell)
                {
                    spellTargetMarker.SetActive(true);
                }
                if (selectedSpell is AreaOfEffectSpell)
                {
                    spellAreaMarker.SetActive(true);
                }
            }
            else if (spellStage == SpellStage.Targeting)
            {
                CastSpell(input);
                spellStage = SpellStage.Idle;
                if (selectedSpell is TargetedSpell)
                {
                    spellTargetMarker.SetActive(false);
                }
                if (selectedSpell is AreaOfEffectSpell)
                {
                    spellAreaMarker.SetActive(true);
                }
            }
        }
    }

    public void SelectNextSpell()
    {
        selectedSpellIndex++;
        if (selectedSpellIndex >= spellItems.Length)
        {
            selectedSpellIndex = 0;
        }
        selectedSpellSprite.sprite = spellItems[selectedSpellIndex].GetComponent<SpriteRenderer>().sprite;
    }

    public void SelectPreviousSpell()
    {
        selectedSpellIndex--;
        if (selectedSpellIndex < 0)
        {
            selectedSpellIndex = spellItems.Length - 1;
        }
        selectedSpellSprite.sprite = spellItems[selectedSpellIndex].GetComponent<SpriteRenderer>().sprite;
    }

    public void CastSpell(InputAction.CallbackContext input)
    {
        if (selectedSpell is AreaOfEffectSpell)
        {
            spellArea.AddComponent(selectedSpell.GetType());
            ResetSpellAreaOpacity();
            SetSpellAreaWidth();
            FadeOutSpellArea();
        }

        if (selectedSpell is TargetedSpell)
        {
            (selectedSpell as TargetedSpell).target = spellTargetMarker.GetComponent<TargetMarkerController>().target.gameObject;
            selectedSpell.CastSpell();
        }
    }

    void SetSpellAreaWidth()
    {
        var line = spellAreaMarker.GetComponent<LineRenderer>();
        Vector3[] points = new Vector3[2];
        line.GetPositions(points);
        spellArea.transform.position = (points[0] + points[1]) / 2;
        spellArea.transform.position = new Vector3(
            spellAreaMarker.transform.position.x,
            spellArea.transform.position.y,
            spellArea.transform.position.z
        );
        spellArea.transform.localScale = new Vector3(
            Vector3.Distance(points[0], points[1]),
            spellArea.transform.localScale.y,
            spellArea.transform.localScale.z
        );
        line.SetPositions(new Vector3[] { Vector3.zero, Vector3.zero });
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
