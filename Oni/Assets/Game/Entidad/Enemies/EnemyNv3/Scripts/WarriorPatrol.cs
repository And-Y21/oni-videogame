using UnityEngine;

public class WarriorPatrol : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public float detectionRange = 5f;
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
        {
            StopAndAttack();
        }
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
        MoveTowards(currentTarget.position);
        animator.SetBool("isWalking", true);
        CheckDistance();
    }

    void Chase()
    {
        MoveTowards(player.position);
        animator.SetBool("isWalking", true);
    }

    void StopAndAttack()
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        animator.SetBool("isWalking", false);
        FlipTowards(player.position);

        if (Time.time > lastAttackTime + attackCooldown && !isAttackPlaying)
        {
            isAttackPlaying = true;
            lastAttackTime = Time.time;
            animator.ResetTrigger("Attack1");
            animator.SetTrigger("Attack1");
            Invoke(nameof(ResetAttackFlag), attackCooldown * 0.9f);
        }
    }

    void MoveTowards(Vector3 target)
    {
        Vector2 direction = (target - transform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);
        FlipTowards(target);
    }

    void CheckDistance()
    {
        if (Vector2.Distance(transform.position, currentTarget.position) < 0.5f)
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