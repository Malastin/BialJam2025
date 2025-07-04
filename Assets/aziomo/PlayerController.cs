using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerFighterStats playerFighterStats;
    [SerializeField] private GameObject basicAttac;
    private Rigidbody2D rb2D;
    private SpriteRenderer spriteRenderer;

    public float speed = 1.0f;
    public float jumpPower;
    private Vector2 inputMovement;
    private byte jumpsAmount;
    private bool canDash = true;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        rb2D.linearVelocity += new Vector2(inputMovement.x, 0) * speed * playerFighterStats.movementSpeed * 0.02f;
        
    }

    public void Jump(InputAction.CallbackContext callback)
    {
        if (jumpsAmount > 0 && callback.phase == InputActionPhase.Started)
        {
            rb2D.linearVelocityY += jumpPower;
            jumpsAmount--;
        }
    }

    public void Dash(InputAction.CallbackContext callback)
    {
        if (canDash && callback.phase == InputActionPhase.Started)
        {
            StartCoroutine(DashCorutine());
            canDash = false;
        }
    }

    private IEnumerator DashCorutine()
    {
        int time = 5;
        int stage = 0;
        while (true)
        {
            Debug.Log(stage);
            switch (stage)
            {
                case 0:
                    rb2D.linearVelocity += new Vector2(inputMovement.x, 0) * speed * playerFighterStats.movementSpeed * 0.12f;
                    break;
                case 1:
                    time = 100;
                    stage++;
                    break;
                case 3:
                    canDash = true;
                    yield break;
            }

            if (time > 0)
            {
                time--;
            }
            else
            {
                stage++;
            }
            yield return new WaitForFixedUpdate();
        }
    }

    public void Movement(InputAction.CallbackContext input)
    {
        inputMovement = input.ReadValue<Vector2>();
        if (inputMovement.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        if (inputMovement.x < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    public void CastBasicAttack(InputAction.CallbackContext callback)
    {
        if (callback.phase == InputActionPhase.Started)
        {
            var obj = Instantiate(basicAttac, transform.parent);
            obj.transform.position = transform.position;
            if (spriteRenderer.flipX)
            {
                obj.transform.localPosition += new Vector3(-0.5f, 0, 0);
            }
            else
            {
                obj.transform.localPosition += new Vector3(0.5f, 0, 0);
            }
            obj.GetComponent<PodstawowyAtakGracza>().caster = gameObject;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpsAmount = 2;
        }
    }
}
