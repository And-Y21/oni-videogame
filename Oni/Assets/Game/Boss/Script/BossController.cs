using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Referencias")]
    public Transform player;
    public Transform model;
    public BossHitBox hitbox;

    private Animator animator;
    private Rigidbody2D rb;

    [Header("Movimiento")]
    public float speed = 2f;
    public float attackRange = 2f;
    public float detectionRange = 6f;

    [Header("Salto")]
    public float jumpForce = 8f;
    private bool isGrounded = true;

    [Header("Combate")]
    public float attackCooldown = 1.5f;
    private float lastAttackTime;

    [Header("Vida")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Arena")]
    public GameObject leftWall;
    public GameObject rightWall;

    private bool isDead = false;
    private bool isActivated = false;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;

        if (leftWall) leftWall.SetActive(false);
        if (rightWall) rightWall.SetActive(false);
    }

    void Update()
    {
        if (isDead || player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // Activación por rango
        if (!isActivated)
        {
            if (distance <= detectionRange)
            {
                ActivateBoss();
            }
            else
            {
                animator.SetFloat("speed", 0);
                return;
            }
        }

        LookAtPlayer();

        if (distance > attackRange)
            MoveToPlayer();
        else
            TryAttack();
    }

    void ActivateBoss()
    {
        isActivated = true;

        if (leftWall) leftWall.SetActive(true);
        if (rightWall) rightWall.SetActive(true);
    }

    void MoveToPlayer()
    {
        animator.SetFloat("speed", 1);

        Vector3 target = new Vector3(player.position.x, transform.position.y, transform.position.z);

        transform.position = Vector2.MoveTowards(
            transform.position,
            target,
            speed * Time.deltaTime
        );
    }

    void TryAttack()
    {
        animator.SetFloat("speed", 0);

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;

            int attack = Random.Range(1, 5);

            if (attack == 4 && isGrounded)
            {
                JumpAttack();
            }
            else
            {
                animator.SetInteger("attackType", attack);
                animator.SetTrigger("attack");
            }
        }
    }

    void JumpAttack()
    {
        isGrounded = false;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

        animator.SetTrigger("jumpAttack");
    }

    void LookAtPlayer()
    {
        if (player.position.x > transform.position.x)
            model.localScale = new Vector3(1, 1, 1);
        else
            model.localScale = new Vector3(-1, 1, 1);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }

    // 🔥 Animation Events llaman esto
    public void EnableDamage()
    {
        if (hitbox != null)
            hitbox.EnableDamage();
    }

    public void DisableDamage()
    {
        if (hitbox != null)
            hitbox.DisableDamage();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        Debug.Log("Boss vida: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;

        Debug.Log("BOSS MUERTO 💀");

        animator.SetBool("isDead", true);

        // 🔒 detener movimiento
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;

        // 🚫 desactivar daño
        if (hitbox != null)
            hitbox.DisableDamage();

        // 🚧 abrir arena
        if (leftWall) leftWall.SetActive(false);
        if (rightWall) rightWall.SetActive(false);

        // 🗑️ destruir después de animación
        Destroy(gameObject, 6f);
    }
}
