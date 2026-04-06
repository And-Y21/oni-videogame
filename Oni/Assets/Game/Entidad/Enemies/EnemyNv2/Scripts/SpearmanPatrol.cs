using UnityEngine;

public class SpearmanPatrol : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 1.5f;

    [Header("Patrol Points")]
    public Transform pointA;
    public Transform pointB;

    [Header("Combat Settings")]
    public float detectionRange = 5f;
    public float attackRange = 1.2f;
    public float attackCooldown = 1.5f;

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
        transform.position = Vector3.MoveTowards(
            transform.position,
            new Vector3(currentTarget.x, transform.position.y, 0),
            moveSpeed * Time.deltaTime
        );
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
        if (player == null) { Debug.Log("Player null"); return; }

        float dist = Vector2.Distance(transform.position, player.position);
        Debug.Log("HacerDano - distancia: " + dist + " | rango: " + (attackRange + 0.5f));

        if (dist < attackRange + 0.5f)
        {
            Playerhealth health = player.GetComponentInChildren<Playerhealth>();
            if (health != null)
                health.TakeDamage(10);
            else
                Debug.Log("No encontró Playerhealth");
        }
    }

    void ResetAttackFlag()
    {
        isAttackPlaying = false;
    }

    void CheckDestination()
    {
        if (Mathf.Abs(transform.position.x - currentTarget.x) < 0.1f)
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