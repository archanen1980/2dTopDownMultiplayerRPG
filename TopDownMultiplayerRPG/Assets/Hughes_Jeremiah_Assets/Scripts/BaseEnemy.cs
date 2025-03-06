using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2f; // Default enemy movement speed
    public float rotationSpeed = 360f; // Rotation speed

    [Header("Combat")]
    public float attackDamage = 10f;
    public float attackDelay = 1f;
    public float attackRange = 1f;
    private float currentAttackDelay = 0f;

    [Header("Health")]
    public float maxHealth = 10f;
    private float currentHealth;

    [Header("Aggro")]
    public float aggroRange = 5f; // The range at which the enemy will start to chase the player
    public float aggroBreakRange = 10f; // The range at which the enemy will stop chasing the player

    protected Transform target; // The player or what the enemy is targeting
    private bool canAttack = false; //If we are in range, we can attack.
    private bool isAggroed = false; //If we are currently targeting the player.

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
    }

    protected virtual void Start()
    {
        // Find the player (or set target some other way)
        //This is a very basic implementation, but works for now
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
        else
        {
            Debug.LogError("No object tagged Player found.");
        }
    }

    protected virtual void Update()
    {
        if (currentAttackDelay > 0)
        {
            currentAttackDelay -= Time.deltaTime;
        }
        CheckAggro();
    }

    protected virtual void FixedUpdate()
    {
        if (target != null && isAggroed)
        {
            HandleMovement();
        }
    }

    protected virtual void HandleMovement()
    {
        // Basic movement towards the target
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        if (Vector3.Distance(target.position, transform.position) > attackRange)
        {
            // Rotate towards the target
            float targetAngle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, targetAngle), rotationSpeed * Time.deltaTime);

            // Move towards the target
            transform.position += directionToTarget * moveSpeed * Time.deltaTime;
            canAttack = false; //We are out of range, so we can not attack.
        }
        else
        {
            canAttack = true; //We are in range, we can attack.
            HandleAttack();
        }
    }

    protected virtual void HandleAttack()
    {
        if (currentAttackDelay <= 0 && canAttack)
        {
            //Get the player
            TopDownCharacterController player = target.GetComponent<TopDownCharacterController>();
            if (player != null)
            {
                Debug.Log("Attacking Player");  
                player.TakeDamage(attackDamage);
            }
            else
            {
                Debug.LogError("No Player found.");
            }
            currentAttackDelay = attackDelay; //Only count down attack delay if we attack.
        }
    }

    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void CheckAggro()
    {
        if (target != null)
        {
            float distanceToTarget = Vector3.Distance(target.position, transform.position);

            // If already aggroed, check if out of aggro break range
            if (isAggroed)
            {
                if (distanceToTarget > aggroBreakRange)
                {
                    isAggroed = false;
                }
            }
            // If not aggroed, check if in aggro range
            else
            {
                if (distanceToTarget <= aggroRange)
                {
                    isAggroed = true;
                }
            }
        }
    }

    protected virtual void Die()
    {
        // Handle enemy death (e.g., play death animation, drop loot, etc.)
        Destroy(gameObject);
    }
}
