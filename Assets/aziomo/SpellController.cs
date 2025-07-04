using UnityEngine;
using UnityEngine.InputSystem;

public class SpellController : MonoBehaviour
{
    public GameObject spellCrosshair;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void Aim(InputAction.CallbackContext input)
    {
        if (input.performed)
        {
            Vector2 aimDirection = input.ReadValue<Vector2>();
            // Implement aiming logic here, e.g., rotate the spell indicator
            if (spellCrosshair != null)
            {
                spellCrosshair.transform.position = (Vector2)transform.position + aimDirection.normalized * 2.0f; // Adjust the distance as needed
            }
            else
            {
                Debug.LogWarning("Spell crosshair is not assigned!");
            }
        }
    }
}
