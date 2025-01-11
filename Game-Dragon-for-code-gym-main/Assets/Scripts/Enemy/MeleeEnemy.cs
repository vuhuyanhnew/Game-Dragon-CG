using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;

    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [SerializeField] private LayerMask playerLayer;

    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float chaseDistance = 5f;

    private float cooldownTimer = Mathf.Infinity;
    private Vector3 initialPosition;
    private Transform player;
    private bool isChasing = false;

    //References
    private Animator anim;
    private Health playerHealth;
    private EnemyPatrol enemyPatrol;
    private Rigidbody2D rb;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
        rb = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (PlayerInSight())
        {
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                anim.SetTrigger("meleeAttack");
            }
            isChasing = true;
        }
        else
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            isChasing = distanceToPlayer <= chaseDistance;

            if (!isChasing)
            {
                anim.SetBool("Run", false);
                rb.linearVelocity = Vector2.zero;
            }
        }

        if (enemyPatrol != null)
            enemyPatrol.enabled = !isChasing;

        if (isChasing)
            ChasePlayer();
    }

    private void ChasePlayer()
    {
        if (player == null) return;

        // Calculate direction to player
        Vector2 direction = (player.position - transform.position).normalized;

        // Move towards player
        rb.linearVelocity = direction * moveSpeed;

        // Flip sprite based on movement direction
        if (direction.x > 0)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (direction.x < 0)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

        // Update run animation
        anim.SetBool("Run", rb.linearVelocity.magnitude > 0.1f);
    }
    private bool PlayerInSight()
    {
        RaycastHit2D hit =
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
            playerHealth = hit.transform.GetComponent<Health>();

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));

        // Draw chase range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }

    private void DamagePlayer()
    {
        if (PlayerInSight())
            playerHealth.TakeDamage(damage);
    }
}