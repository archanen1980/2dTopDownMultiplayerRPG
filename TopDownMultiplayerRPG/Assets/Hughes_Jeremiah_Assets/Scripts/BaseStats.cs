using UnityEngine;

public class BaseStats : MonoBehaviour
{
    [Header("Health")]
    public float maxHp = 100f;
    public float currentHp;

    [Header("Mana")]
    public float maxMana = 50f;
    public float currentMana;

    [Header("Combat")]
    public float attackDamage = 10f;
    public float attackDelay = 1f;
    [HideInInspector] public float currentAttackDelay = 0f; // Hide this in the inspector
    public float attackRange = 1f;

    protected virtual void Awake()
    {
        currentHp = maxHp;
        currentMana = maxMana;
    }

    protected virtual void Update()
    {
           
    }

    public virtual void TakeDamage(float damage)
    {
        currentHp -= damage;
        if (currentHp <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        // Handle death (e.g., play death animation, destroy GameObject, etc.)
        Destroy(gameObject);
    }
}
