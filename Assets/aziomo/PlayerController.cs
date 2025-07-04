using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerFighterStats playerFighterStats;
    [SerializeField] private Rigidbody2D rb2D;

    public float speed = 1.0f;
    private Vector2 inputMovement;

    private void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().linearVelocity += new Vector2(inputMovement.x, 0) * speed * playerFighterStats.movementSpeed * 0.02f;
    }

    public void Movement(InputAction.CallbackContext input)
    {
        inputMovement = input.ReadValue<Vector2>();
    }
}
