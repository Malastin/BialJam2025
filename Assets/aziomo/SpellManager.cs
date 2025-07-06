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
    public float aimSpeed = 0.05f;
    private GameObject spellInstance;
    private SpellStage spellStage = SpellStage.Idle;
    private SpriteRenderer selectedSpellIcon;

    public Vector3 areaMarkerRestingPosition;
    public float castingSpeed = 0.001f;
    public static float platformaX;

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
        platformaX = spellAreaMarker.transform.position.x;
        if (selectedSpellBehavior is AreaOfEffectSpell && spellStage == SpellStage.Targeting)
        {
            if (spellAreaMarker != null)
            {
                // marker width
                var line = spellAreaMarker.GetComponent<LineRenderer>();
                Vector3[] points = new Vector3[2];
                line.GetPositions(points);
                Camera camForLine = Camera.main;
                float halfWidthForLine = camForLine.orthographicSize * camForLine.aspect;
                float maxLineWidth = (halfWidthForLine * 2f) / 3f;
                Vector3 center = (points[0] + points[1]) / 2f;
                float currentHalfLength = Vector3.Distance(points[0], points[1]) / 2f;
                float newHalfLength = currentHalfLength + castingSpeed;
                float clampedHalfLength = Mathf.Min(newHalfLength, maxLineWidth / 2f);
                Vector3 dir = (points[1] - points[0]).normalized;
                if (dir == Vector3.zero) dir = Vector3.right;
                points[0] = center - dir * clampedHalfLength;
                points[1] = center + dir * clampedHalfLength;
                line.SetPositions(points);

                // marker position
                Vector3 newPos = spellAreaMarker.transform.position + new Vector3(aimDirection.x * aimSpeed, 0f, 0f);
                Camera cam = Camera.main;
                float halfWidth = cam.orthographicSize * cam.aspect;
                float minX = cam.transform.position.x - halfWidth;
                float maxX = cam.transform.position.x + halfWidth;
                newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
                spellAreaMarker.transform.position = newPos;
            }
        }
    }

    public void NavigateNext(InputAction.CallbackContext input)
    {
        if (input.performed)
        {
            if (spellStage == SpellStage.Idle)
            {
                Swietego(input);
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
                Iducha(input);
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

    private Vector2 aimDirection;

    public void Aim(InputAction.CallbackContext input)
    {
        if (spellStage != SpellStage.Targeting)
        {
            return;
        }
        if (input.performed)
        {
            aimDirection = input.ReadValue<Vector2>();

            // discard deadzone noise
            if (aimDirection.magnitude < 0.75f)
            {
                aimDirection = Vector2.zero;
            }
            if (selectedSpellBehavior is AreaOfEffectSpell)
            {
                aimDirection.y = 0;
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
                SoundManager.PlaySound(SoundType.Spell);
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
            Debug.Log("spellareamarker position " + spellAreaMarker.transform.position);
            Debug.Log("spellinstance position " + spellInstance.transform.position);
            spellInstance.transform.SetParent(transform);
            Debug.Log("spellinstance position " + spellInstance.transform.position);

            ResetSpellAreaOpacity();
            SetSpellAreaWidth();
            // FadeOutSpellArea();
            selectedSpellBehavior.CastSpell();
            Destroy(spellInstance, (selectedSpellBehavior as AreaOfEffectSpell).decayTime);
        }

        if (selectedSpellBehavior is TargetedSpell)
        {
            (selectedSpellBehavior as TargetedSpell).target = spellTargetMarker.GetComponent<TargetMarkerController>().target.gameObject;
            (selectedSpellBehavior as TargetedSpell).aimDirection = aimDirection;
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
            /*if (spriteRenderer != null)
            {
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.5f);
            }
            else
            {
                Debug.LogWarning("SpriteRenderer not found on SpellArea!");
            }*/
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


    // wpierdalam to tu i chuj
    private int inputsEntered = 0;
    public void WImieOjca(InputAction.CallbackContext input)
    {
        if (input.performed)
        {
            if (inputsEntered == 0)
            {
                inputsEntered++;
                Debug.Log("W Imie Ojca");
                Debug.Log(inputsEntered);

            }
            else
            {
                inputsEntered = 1;
            }
        }
    }
    public void ISyna(InputAction.CallbackContext input)
    {
        if (input.performed)
        {
            if (inputsEntered == 1)
            {
                inputsEntered++;
                Debug.Log("I Syna");
                Debug.Log(inputsEntered);
            }
            else
            {
                inputsEntered = 0;
            }
        }
            
    }
    public void Iducha(InputAction.CallbackContext input)
    {
        if (input.performed)
        {
            if (inputsEntered == 2)
            {
                inputsEntered++;
                Debug.Log("I Ducha");
                Debug.Log(inputsEntered);

            }
            else
            {
                inputsEntered = 0;
            }
        }
        
    }
    public void Swietego(InputAction.CallbackContext input)
    {
        if (input.performed)
        {
            if (inputsEntered == 3)
            {
                Debug.Log("Swietego");
                Debug.Log(inputsEntered);

                inputsEntered++;
                GameManager.GetPlayerReference(0).GetComponent<PlayerController>().Crucify();
                GameManager.GetPlayerReference(1).GetComponent<PlayerController>().Crucify();
            }
            else
            {
                inputsEntered = 0;
            }
        }
            
    }
}
