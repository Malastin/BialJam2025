using UnityEngine;
using UnityEngine.InputSystem;

public class SpellController : MonoBehaviour
{
    public GameObject spellCrosshair;
    public Vector3 crosshairRestingPosition;

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
                points[0] = points[0] + Vector3.left * 0.005f;
                points[1] = points[1] + Vector3.right * 0.005f;
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

        if (input.performed)
        {
            line.enabled = true;
            isCasting = true;
        }
        else if (input.canceled)
        {
            isCasting = false;
            line.SetPositions(new Vector3[] { Vector3.zero, Vector3.zero });
            line.enabled = false;
        }
        
    }
}
