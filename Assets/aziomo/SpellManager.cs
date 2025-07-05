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
    public GameObject spellAreaPrefab;
    private GameObject spellInstance;
    private SpellStage spellStage = SpellStage.Idle;
    private SpriteRenderer selectedSpellIcon;

    public Vector3 areaMarkerRestingPosition;
    public float castingSpeed = 0.001f;

    private GameObject selectedSpell
    {
        get { return spellItems[selectedSpellIndex]; }
    }

    private SpellBehavior selectedSpellBehavior
    {
        get { return selectedSpell.GetComponent<SpellBehavior>(); }
    }

    void Start()
    {
        areaMarkerRestingPosition = spellAreaMarker.transform.position;
        selectedSpellIcon = GetComponent<SpriteRenderer>();
        selectedSpellIcon.sprite = selectedSpellBehavior.icon;
    }

    void Update()
    {
        if (selectedSpellBehavior is AreaOfEffectSpell && spellStage == SpellStage.Targeting)
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
                if (selectedSpellBehavior is TargetedSpell)
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
                if (selectedSpellBehavior is TargetedSpell)
                {
                    spellTargetMarker.GetComponent<TargetMarkerController>().ChangeTarget();
                }
            }
        }
    }

    public void Aim(InputAction.CallbackContext input)
    {
        if (spellStage != SpellStage.Targeting || !(selectedSpellBehavior is AreaOfEffectSpell))
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
                if (selectedSpellBehavior is TargetedSpell)
                {
                    spellTargetMarker.SetActive(true);
                }
                if (selectedSpellBehavior is AreaOfEffectSpell)
                {
                    spellAreaMarker.SetActive(true);
                }
            }
            else if (spellStage == SpellStage.Targeting)
            {
                CastSpell(input);
                spellStage = SpellStage.Idle;
                if (selectedSpellBehavior is TargetedSpell)
                {
                    spellTargetMarker.SetActive(false);
                }
                if (selectedSpellBehavior is AreaOfEffectSpell)
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
        selectedSpellIcon.sprite = selectedSpellBehavior.icon;
    }

    public void SelectPreviousSpell()
    {
        selectedSpellIndex--;
        if (selectedSpellIndex < 0)
        {
            selectedSpellIndex = spellItems.Length - 1;
        }
        selectedSpellIcon.sprite = selectedSpellBehavior.icon;
    }

    public void CastSpell(InputAction.CallbackContext input)
    {
        if (selectedSpellBehavior is AreaOfEffectSpell)
        {
            spellInstance = Instantiate(selectedSpell, spellAreaMarker.transform.position, Quaternion.identity);
            spellInstance.transform.SetParent(transform);
            
            ResetSpellAreaOpacity();
            SetSpellAreaWidth();
            // FadeOutSpellArea();
            selectedSpellBehavior.CastSpell();
            Destroy(spellInstance, (selectedSpellBehavior as AreaOfEffectSpell).decayTime);
        }

        if (selectedSpellBehavior is TargetedSpell)
        {
            (selectedSpellBehavior as TargetedSpell).target = spellTargetMarker.GetComponent<TargetMarkerController>().target.gameObject;
            selectedSpellBehavior.CastSpell();
        }
    }

    void SetSpellAreaWidth()
    {
        var line = spellAreaMarker.GetComponent<LineRenderer>();
        Vector3[] points = new Vector3[2];
        line.GetPositions(points);
        spellInstance.transform.position = (points[0] + points[1]) / 2;
        spellInstance.transform.position = new Vector3(
            spellAreaMarker.transform.position.x,
            spellInstance.transform.position.y,
            spellInstance.transform.position.z
        );
        float worldScreenHeight = 2f * Camera.main.orthographicSize;
        spellInstance.transform.localScale = new Vector3(
            Vector3.Distance(points[0], points[1]),
            worldScreenHeight,
            spellInstance.transform.localScale.z
        );
        line.SetPositions(new Vector3[] { Vector3.zero, Vector3.zero });
    }


    void FadeOutSpellArea()
    {
        if (spellInstance != null)
        {
            var spriteRenderer = spellInstance.GetComponent<SpriteRenderer>();
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
        if (spellInstance != null)
        {
            var spriteRenderer = selectedSpell.GetComponent<SpriteRenderer>();
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
