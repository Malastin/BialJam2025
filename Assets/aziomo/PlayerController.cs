using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerFighterStats playerFighterStats;
    [SerializeField] private GameObject basicAttac;
    private Rigidbody2D rb2D;
    private SpriteRenderer spriteRenderer;
    private CapsuleCollider2D capsuleCollider2D;
    public DamageEffect damageEffect;

    public float speed = 1.0f;
    public float jumpPower;
    private Vector2 inputMovement;
    private byte jumpsAmount;
    private bool canDash = true;
    private bool blockMovement = false;
    private GameObject tempObj;
    public PlayerStates animationState;
    [SerializeField] private Animator animator;
    private bool inOtherAnimation;
    private int blockTicks;
    private bool blockNextAttack;
    private bool fallAttack;
    private bool ground;
    private bool closeToWall;
    public bool grabedToWall;
    public bool wallXisBigger;
    private bool tryGrabing;
    private bool isDeath;
    public float dashCooldown;
    public float wallGrabCooldown = .8f;
    private float wallGrabTimer;
    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        damageEffect = GetComponent<DamageEffect>();
    }
    private void FixedUpdate()
    {
        if (isDeath)
        {
            rb2D.gravityScale = 4f;
            return;
        }
        if (inputMovement.x != 0 && !ground)
        {
            animationState = PlayerStates.fall;
            if (!inOtherAnimation && !grabedToWall)
            {
                UpdateAnimationOfPlayer();
            }
        }
        if (!ground && !grabedToWall)
        {
            rb2D.gravityScale += 0.3f;
        }
        if (!grabedToWall)
        {
            if (blockMovement)
            {
                rb2D.linearVelocity += new Vector2(0, 0) * speed * playerFighterStats.movementSpeed * 0.02f;
                rb2D.linearVelocityY -= 3f;
            }
            else
            {
                rb2D.linearVelocity += new Vector2(inputMovement.x, 0) * speed * playerFighterStats.movementSpeed * 0.02f;
                if (inputMovement.x > 0 && !inOtherAnimation)
                {
                    spriteRenderer.flipX = false;
                }
                if (inputMovement.x < 0 && !inOtherAnimation)
                {
                    spriteRenderer.flipX = true;
                }
            }
        }
        if (inputMovement.x > 0 && wallXisBigger)
        {
            tryGrabing = true;
        }
        else if (inputMovement.x < 0 && !wallXisBigger)
        {
            tryGrabing = true;
        }
        else
        {
            tryGrabing = false;
        }

        if (closeToWall && wallGrabTimer <= 0 && !ground && tryGrabing && !grabedToWall){
            grabedToWall = true;
            blockMovement = true;
            inOtherAnimation = true;
            spriteRenderer.flipX = wallXisBigger;
            rb2D.linearVelocity = Vector2.zero;
            animationState = PlayerStates.grabedToWall;
            rb2D.gravityScale = 0;
            UpdateAnimationOfPlayer();
        }
    }
    private void Update(){
        wallGrabTimer -= Time.deltaTime;   
    }
    public void Jump(InputAction.CallbackContext callback)
    {
        if (grabedToWall && callback.phase == InputActionPhase.Started && !isDeath)
        {
            rb2D.linearVelocityY += jumpPower;
            if (spriteRenderer.flipX)
            {
                rb2D.linearVelocityX += -10;
            }
            else
            {
                rb2D.linearVelocityX += 10;
            }
            grabedToWall = false;
            wallGrabTimer = wallGrabCooldown;
            inOtherAnimation = false;
            blockMovement = false;
            animationState = PlayerStates.idle;
            UpdateAnimationOfPlayer();
            return;

        }
        if (jumpsAmount > 0 && callback.phase == InputActionPhase.Started && !isDeath)
        {
            rb2D.linearVelocityY += jumpPower;
            jumpsAmount--;
            animationState = PlayerStates.idle;
            UpdateAnimationOfPlayer();
        }
    }

    public void Dash(InputAction.CallbackContext callback)
    {
        if (canDash && callback.phase == InputActionPhase.Started && !isDeath && !grabedToWall && inputMovement.x != 0 )
        {
            StartCoroutine(DashCorutine());
            canDash = false;
            inOtherAnimation = true;
            animationState = PlayerStates.dash;
            UpdateAnimationOfPlayer();
        }
    }

    private IEnumerator DashCorutine()
    {
        int time = 5;
        int stage = 0;
        dashCooldown = 2.1f;
        while (true)
        {
            switch (stage)
            {
                case 0:
                    rb2D.linearVelocity += new Vector2(inputMovement.x, 0) * speed * playerFighterStats.movementSpeed * 0.12f;
                    break;
                case 1:
                    time = 20;
                    stage++;
                    break;
                case 3:
                    time = 80;
                    inOtherAnimation = false;
                    if (!grabedToWall && !isDeath)
                    {
                        if (inputMovement.x != 0)
                        {
                            animationState = PlayerStates.run;
                        }
                        else
                        {
                            animationState = PlayerStates.idle;
                        }
                        UpdateAnimationOfPlayer();
                    }
                    stage++;
                    break;
                case 5:
                    canDash = true;
                    yield break;
            }

            if (time > 0)
            {
                time--;
                dashCooldown -= 0.02f;
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
        if (isDeath)
        {
            return;
        }
        inputMovement = input.ReadValue<Vector2>();
        
        if (inputMovement.x != 0 && !grabedToWall)
        {
            animationState = PlayerStates.run;
        }
        if (!inOtherAnimation && !grabedToWall)
        {
            UpdateAnimationOfPlayer();
        }
        if (input.phase == InputActionPhase.Canceled)
        {
            animationState = PlayerStates.idle;
            if (!inOtherAnimation && !grabedToWall)
            {
                UpdateAnimationOfPlayer();
            }
        }
    }

    public void CastBasicAttack(InputAction.CallbackContext callback)
    {
        if (callback.phase == InputActionPhase.Started && !blockNextAttack && !grabedToWall && !isDeath)
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
            obj.GetComponent<PodstawowyAtakGracza>().damage = 2;
            obj.GetComponent<PodstawowyAtakGracza>().killOnTime = true;
            blockNextAttack = true;
            inOtherAnimation = true;
            blockTicks = 25;
            animationState = PlayerStates.normalAttack;
            UpdateAnimationOfPlayer();
            StartCoroutine(BlockAttack());
        }
    }

    public void CastFallingAttack(InputAction.CallbackContext callback)
    {
        if (callback.phase == InputActionPhase.Started && tempObj == null && !blockNextAttack && !ground && !grabedToWall && !isDeath)
        {
            var obj = Instantiate(basicAttac, transform);
            obj.transform.position = transform.position;
            obj.transform.localPosition += new Vector3(0, -0.4f, 0);
            obj.transform.localEulerAngles += new Vector3(0, 0, 90);
            obj.GetComponent<PodstawowyAtakGracza>().caster = gameObject;
            obj.GetComponent<PodstawowyAtakGracza>().damage = 4;
            obj.GetComponent<PodstawowyAtakGracza>().killOnTime = false;
            tempObj = obj;
            blockMovement = true;
            fallAttack = true;
            blockNextAttack = true;
            inOtherAnimation = true;
            animationState = PlayerStates.skyAttack;
            capsuleCollider2D.offset = new Vector2(0, -0.23f);
            UpdateAnimationOfPlayer();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpsAmount = 1;
            blockMovement = false;
            ground = true;
            rb2D.gravityScale = 4f;
            if (tempObj != null)
            {
                Destroy(tempObj);
            }
            if (fallAttack)
            {
                fallAttack = false;
                blockNextAttack = false;
                inOtherAnimation = false;
                animationState = PlayerStates.endSkyAttack;
                UpdateAnimationOfPlayer();
            }
            if (!inOtherAnimation)
            {
                if (inputMovement.x != 0)
                {
                    animationState = PlayerStates.run;
                    UpdateAnimationOfPlayer();
                }
            }
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            closeToWall = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            ground = true;
        }
        if (fallAttack)
        {
            fallAttack = false;
            blockNextAttack = false;
            inOtherAnimation = false;
            UpdateAnimationOfPlayer();
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            closeToWall = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            ground = false;
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            closeToWall = false;
        }
    }

    private void UpdateAnimationOfPlayer()
    {
        switch (animationState)
        {
            case PlayerStates.idle:
                animator.speed = 1f;
                capsuleCollider2D.offset = new Vector2(0, -0.05f);
                animator.Play("Idle");
                break;
            case PlayerStates.run:
                animator.speed = playerFighterStats.movementSpeed;
                capsuleCollider2D.offset = new Vector2(0, -0.05f);
                animator.Play("Walk");
                break;
            case PlayerStates.normalAttack:
                animator.speed = playerFighterStats.attackSpeed;
                capsuleCollider2D.offset = new Vector2(0, -0.05f);
                animator.Play("Attack");
                break;
            case PlayerStates.skyAttack:
                animator.speed = 1f;
                animator.Play("AssSword");
                break;
            case PlayerStates.fall:
                animator.speed = 1f;
                capsuleCollider2D.offset = new Vector2(0, -0.05f);
                animator.Play("InAir");
                break;
            case PlayerStates.dash:
                animator.speed = 4f;
                capsuleCollider2D.offset = new Vector2(0, -0.05f);
                animator.Play("DashStart");
                break;
            case PlayerStates.grabedToWall:
                capsuleCollider2D.offset = new Vector2(0, -0.05f);
                animator.Play("GrabWall");
                break;
            case PlayerStates.endSkyAttack:
                capsuleCollider2D.offset = new Vector2(0, -0.23f);
                animator.Play("AssSwordLanding");
                break;
            case PlayerStates.death:
                animator.Play("DeathStart");
                break;
        }
    }

    private IEnumerator BlockAttack()
    {
        while (true)
        {
            blockTicks--;
            if (blockTicks <= 0)
            {
                blockNextAttack = false;
                inOtherAnimation = false;
                if (!grabedToWall && !isDeath)
                {
                    if (inputMovement.x != 0)
                    {
                        animationState = PlayerStates.run;
                    }
                    else
                    {
                        animationState = PlayerStates.idle;
                    }
                    UpdateAnimationOfPlayer();
                }
                yield break;
            }
            yield return new WaitForFixedUpdate();
        }
    }

    public void DeatchAnimationTrigger()
    {
        if (animationState != PlayerStates.death)
        {
            animationState = PlayerStates.death;
            inOtherAnimation = true;
            blockMovement = true;
            isDeath = true;
            grabedToWall = false;
            UpdateAnimationOfPlayer();
        }
    }
}
