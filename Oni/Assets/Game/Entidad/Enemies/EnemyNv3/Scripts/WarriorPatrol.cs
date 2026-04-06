using UnityEngine;

public class WarriorPatrol : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public float detectionRange = 15f;
    public float attackRange = 1.2f;
    public float attackCooldown = 1.5f;

    public Transform Point_AW;
    public Transform Point_BW;

    private Transform currentTarget;
    private Transform player;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private WarriorHealth health;
    private float lastAttackTime;
    private bool isAttackPlaying = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = GetComponent<WarriorHealth>();
        currentTarget = Point_AW;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
    }

    void Update()
    {
        if (Point_AW == null || Point_BW == null) return;
        if (player == null) return;
        if (health != null && health.isDead) return;

        float dist = Vector2.Distance(transform.position, player.position);

        if (dist < attackRange)
            StopAndAttack();
        else if (dist < detectionRange)
        {
            isAttackPlaying = false;
            Chase();
        }
        else
        {
            isAttackPlaying = false;
            Patrol();
        }
    }

    void Patrol()
    {
        animator.SetBool("isWalking", true);
        MoveTowards(currentTarget.position);
        CheckDistance();
    }

    void Chase()
    {
        animator.SetBool("isWalking", true);

        // Solo persigue si el jugador está entre sus puntos de patrulla
        float minX = Mathf.Min(Point_AW.position.x, Point_BW.position.x);
        float maxX = Mathf.Max(Point_AW.position.x, Point_BW.position.x);

        if (player.position.x >= minX && player.position.x <= maxX)
            MoveTowards(player.position);
        else
            Patrol(); // si el jugador está fuera, sigue patrullando
    }

    void StopAndAttack()
    {
        rb.linearVelocity = Vector2.zero;
        animator.SetBool("isWalking", false);
        FlipTowards(player.position);

        if (Time.time > lastAttackTime + attackCooldown && !isAttackPlaying)
        {
            isAttackPlaying = true;
            lastAttackTime = Time.time;
            animator.ResetTrigger("Attack1");
            animator.SetTrigger("Attack1");
            Invoke(nameof(HacerDano), 0.4f);
            Invoke(nameof(ResetAttackFlag), attackCooldown * 0.9f);
        }
    }

    void HacerDano()
    {
        if (player == null) return;
        float dist = Vector2.Distance(transform.position, player.position);
        if (dist < attackRange + 0.5f)
        {
            Playerhealth playerHealth = player.GetComponent<Playerhealth>();
            if (playerHealth == null) playerHealth = player.GetComponentInParent<Playerhealth>();
            if (playerHealth == null) playerHealth = player.GetComponentInChildren<Playerhealth>();
            if (playerHealth != null)
                playerHealth.TakeDamage(10);
        }
    }

    void MoveTowards(Vector3 target)
    {
        Vector2 direction = (target - transform.position).normalized;
        Vector3 newPos = new Vector3(
            transform.position.x + direction.x * moveSpeed * Time.deltaTime,
            transform.position.y,
            0
        );
        rb.MovePosition(newPos);
        FlipTowards(target);
    }

    void CheckDistance()
    {
        if (Mathf.Abs(transform.position.x - currentTarget.position.x) < 0.5f)
            currentTarget = (currentTarget == Point_AW) ? Point_BW : Point_AW;
    }

    void FlipTowards(Vector3 target)
    {
        spriteRenderer.flipX = target.x < transform.position.x;
    }

    void ResetAttackFlag()
    {
        isAttackPlaying = false;
    }
}