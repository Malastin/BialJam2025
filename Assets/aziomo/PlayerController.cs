using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    private Vector2 inputMovement;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().linearVelocity = inputMovement * speed;
    }

    public void Movement(InputAction.CallbackContext input)
    {
        inputMovement = input.ReadValue<Vector2>();
    }
}
