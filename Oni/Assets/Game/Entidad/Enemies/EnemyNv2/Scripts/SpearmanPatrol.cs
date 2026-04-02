using UnityEngine;

public class SpearmanPatrol : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 1.5f;

    [Header("Combat Settings")]
    public float detectionRange = 5f;
    public float attackRange = 1.2f;
    public float attackCooldown = 1.5f;

    private Transform pointA;
    private Transform pointB;
    private Transform player;
    private Vector3 currentTarget;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private float lastAttackTime;
    private bool isAttackPlaying = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        pointA = GameObject.Find("Point_A")?.transform;
        pointB = GameObject.Find("Point_B")?.transform;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;

        if (pointB != null) currentTarget = pointB.position;
    }

    void Update()
    {
        if (pointA == null || pointB == null) return;
        if (player == null) return;

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
        animator.SetBool("isWalking", true);
        transform.position = Vector3.MoveTowards(transform.position, currentTarget, moveSpeed * Time.deltaTime);
        CheckDestination();
    }

    void Chase()
    {
        animator.SetBool("isWalking", true);
        transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        FlipTowards(player.position);
    }

    void StopAndAttack()
    {
        animator.SetBool("isWalking", false);
        FlipTowards(player.position);

        // Solo atacar si el cooldown pasó y no hay ataque en curso
        if (Time.time > lastAttackTime + attackCooldown && !isAttackPlaying)
        {
            isAttackPlaying = true;
            lastAttackTime = Time.time;
            animator.ResetTrigger("Attack1");
            animator.SetTrigger("Attack1");

            // Liberar el flag cuando termine la animación
            Invoke(nameof(ResetAttackFlag), attackCooldown * 0.9f);
        }
    }

    void ResetAttackFlag()
    {
        isAttackPlaying = false;
    }

    void CheckDestination()
    {
        if (Vector3.Distance(transform.position, currentTarget) < 0.1f)
        {
            currentTarget = (currentTarget == pointA.position) ? pointB.position : pointA.position;
            FlipTowards(currentTarget);
        }
    }

    void FlipTowards(Vector3 target)
    {
        spriteRenderer.flipX = target.x < transform.position.x;
    }
}