using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerFighterStats playerFighterStats;
    [SerializeField] private Rigidbody2D rb2D;

    public float speed = 1.0f;
    public float jumpPower;
    private Vector2 inputMovement;
    private bool canJump;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb2D.linearVelocity += new Vector2(inputMovement.x, 0) * speed * playerFighterStats.movementSpeed * 0.02f;
    }

    public void Jump(InputAction.CallbackContext callback)
    {
        if (canJump)
        {
            rb2D.linearVelocityY += jumpPower;
            canJump = false;
        }
    }

    public void Movement(InputAction.CallbackContext input)
    {
        inputMovement = input.ReadValue<Vector2>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            canJump = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            canJump = false;
        }
    }

}
