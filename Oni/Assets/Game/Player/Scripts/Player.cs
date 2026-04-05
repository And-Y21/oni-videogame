using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    [Header("Components")]
    public Rigidbody2D rigidbody;
    public PlayerInput playerInput;
    public Animator animator;
    public PlayerHitBox hitbox;

    [Header("Movement Variables")]
    public float speed;
    public float jumpForce;
    public float jumpCutMultiplier = .5f;
    public float normalGravity;
    public float fallGravity;
    public float jumpGravity;

    public int facingDirection = 1;

    [Header("Health Settings")]
    public int maxHealth = 3;
    public int currentHealth;
    private bool isDead = false;

    // Inputs
    private Vector2 moveInput;
    private bool jumpPressed;
    private bool jumpReleased;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    private bool isGrounded;

    [Header("Attack")]
    public Transform attackHitboxTransform;
    public float hitboxOffset = 1f;
    public float attackCooldown = 0.5f;

    private bool isAttacking = false;

    private void Start()
    {
        rigidbody.gravityScale = normalGravity;
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (isDead) return;

        Flip();
        HandleAnimations();
        UpdateHitboxPosition();
    }

    void FixedUpdate()
    {
        if (isDead) return;

        ApplyVariableGravity();
        CheckGrounded();
        HandleMovement();
        HandleJump();
    }

    private void HandleMovement()
    {
        if (isAttacking) return;

        float targetSpeed = moveInput.x * speed;
        rigidbody.linearVelocity = new Vector2(targetSpeed, rigidbody.linearVelocity.y);
    }

    private void HandleJump()
    {
        if(jumpPressed && isGrounded)
        {
            rigidbody.linearVelocity = new Vector2(rigidbody.linearVelocity.x, jumpForce);
            jumpPressed = false;
            jumpReleased = false;
        }
        if (jumpReleased)
        {
            if (rigidbody.linearVelocity.y > 0)
            {
                rigidbody.linearVelocity = new Vector2(rigidbody.linearVelocity.x, rigidbody.linearVelocity.y * jumpCutMultiplier);
            }
            jumpReleased = false;
        }
    }

    void ApplyVariableGravity()
    {
        if (rigidbody.linearVelocity.y < -0.1f)
        {
            rigidbody.gravityScale = fallGravity;
        }
        else if (rigidbody.linearVelocity.y > 0.1f)
        {
            rigidbody.gravityScale = jumpGravity;
        }
        else
        {
            rigidbody.gravityScale = normalGravity;
        }
    }

    void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    void HandleAnimations()
    {
        animator.SetBool("isJumping", rigidbody.linearVelocity.y > .1f);
        animator.SetBool("isIdle", Mathf.Abs(moveInput.x) < .1f && isGrounded);
        animator.SetBool("isWalking", Mathf.Abs(moveInput.x) > .1f && isGrounded);
    }

    void Flip()
    {
        if (moveInput.x > 0.1f)
        {
            facingDirection = 1;
        }
        else if (moveInput.x < -0.1f)
        {
            facingDirection = -1;
        }

        transform.localScale = new Vector3(facingDirection, 1, 1);
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            jumpPressed = true;
            jumpReleased = false;
        }
        else
        {
            jumpReleased = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log("�Golpe! Vida restante: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void OnAttack(InputValue value)
    {
        if (isDead) return;

        if (value.isPressed && !isAttacking)
        {
            Debug.Log("ATAQUE");

            isAttacking = true;
            animator.SetTrigger("Attack");

            Invoke(nameof(ResetAttack), attackCooldown);
        }
    }

    void ResetAttack()
    {
        isAttacking = false;
    }

    public void EnableDamage()
    {
        if (hitbox == null)
        {
            Debug.LogError("HITBOX NO ASIGNADA");
            return;
        }

        hitbox.EnableDamage();
    }

    public void DisableDamage()
    {
        if (hitbox == null) return;

        hitbox.DisableDamage();
    }

    void UpdateHitboxPosition()
    {
        if (attackHitboxTransform == null) return;

        attackHitboxTransform.localPosition = new Vector3(
            facingDirection * hitboxOffset,
            attackHitboxTransform.localPosition.y,
            0
        );
    }

    public void Die()
    {
        isDead = true;

        animator.SetTrigger("Death");

        Debug.Log("El Samurái ha muerto");

        rigidbody.linearVelocity = Vector2.zero;

        Invoke("RestartLevel", 2f);
    }

    void RestartLevel()
    {
        return;
    }
}
