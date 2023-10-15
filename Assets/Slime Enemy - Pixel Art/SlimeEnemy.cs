using System.Collections;
using UnityEngine;
using ClearSky;
public class SlimeEnemy : MonoBehaviour
{
    public float moveSpeed = 2.0f;            // Speed at which the slime moves.
    public float jumpForce = 5.0f;            // Force applied for jumping.
    public float detectionRange = 5.0f;       // Range at which the slime detects the player.
    public float attackRange = 1.0f;          // Range at which the slime attacks the player.
    public float attackCooldown = 2.0f;       // Cooldown between attacks.
    public float attackStopDistance = 0.5f;   // Distance at which the slime stops while attacking.

    private Transform player;                 // Reference to the player's transform.
    private Animator animator;               // Reference to the Animator component.
    private bool isJumping = false;           // Flag to track jumping state.
    private bool canAttack = true;            // Flag to track attack cooldown.
    public int maxHealth = 10;
    [SerializeField] SlimeHealthBar healthBar;

    public int currentHealth;
    private Vector3 originalSpawnPosition;

    private float lastRespawnTime = 0f;
    private bool isRespawning = false;

    public CoinManager coinManager; // Reference to CoinManager


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        healthBar = GetComponentInChildren<SlimeHealthBar>();
        healthBar.UpdateHPBar(currentHealth, maxHealth);

        // Store the original spawn position.
        originalSpawnPosition = transform.position;

        // Start with the idle animation.
        animator.SetTrigger("idle");
    }

    private void ResetToOriginalPosition()
    {
        // Reset the position of the Slime enemy to the original spawn position.
        transform.position = originalSpawnPosition;
    }

    private IEnumerator RespawnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Reset the Slime enemy to its original position.
        ResetToOriginalPosition();

        // Reset its health or any other attributes.
        currentHealth = maxHealth;
        healthBar.UpdateHPBar(currentHealth, maxHealth);

        // Set the enemy to idle state.
        animator.SetTrigger("idle");

        isRespawning = false; // Reset the respawning flag.
    }

    private void Update()
    {
        // Calculate the distance between the enemy and the player.
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // If the player is within detection range, start following.
        if (distanceToPlayer <= detectionRange)
        {
            // Face the player.
            FlipSprite(player.position.x > transform.position.x);

            // If the player is within attack range, attack.
            if (distanceToPlayer <= attackRange)
            {
                Attack();
            }
            else
            {
                // Calculate the move direction.
                Vector3 moveDirection = (player.position - transform.position).normalized;

                // Stop moving within the attack stop distance.
                if (distanceToPlayer > attackStopDistance)
                {
                    transform.position += moveDirection * moveSpeed * Time.deltaTime;
                }

                // Trigger jump animation.
                if (!isJumping)
                {
                    animator.SetTrigger("startJump");
                    StartCoroutine(Jump());
                }
            }
        }

        // Check if it's time to respawn the Slime enemy.
        if (Time.time - lastRespawnTime >= 30.0f && isRespawning == false)
        {
            // Start the respawn coroutine with a 30-second delay.
            StartCoroutine(RespawnAfterDelay(30.0f));
            lastRespawnTime = Time.time;
            isRespawning = true;
        }
    }

    private void FlipSprite(bool shouldFlip)
    {
        // Flip the sprite to face the player or the opposite direction.
        Vector3 newScale = transform.localScale;
        newScale.x = shouldFlip ? Mathf.Abs(newScale.x) : -Mathf.Abs(newScale.x);
        transform.localScale = newScale;
    }

    private void Attack()
    {
        // Check if the enemy can attack (cooldown).
        if (canAttack)
        {
            // Trigger the jump animation.
            animator.SetTrigger("startJump");

            // Find the PlayerHealth component in the scene.
            PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();

            // Check if PlayerHealth component was found.
            if (playerHealth != null)
            {
                // Set a flag in PlayerHealth to indicate that the player was attacked.
                playerHealth.isAttacked = true;
            }

            // Set the attack cooldown.
            canAttack = false;
            StartCoroutine(ResetAttackCooldown());
        }
    }

    private IEnumerator Jump()
    {
        // Simulate a jump by adjusting the y position.
        isJumping = true;
        float originalY = transform.position.y;
        float jumpHeight = 1.0f; // Adjust the jump height as needed.

        while (transform.position.y < originalY + jumpHeight)
        {
            Vector3 jumpStep = Vector3.up * jumpForce * Time.deltaTime;
            transform.position += jumpStep;
            yield return null;
        }

        // Wait for the jump animation to complete.
        yield return new WaitForSeconds(0.3f);

        // Set the jump animation to the falling state.
        animator.SetTrigger("jumpToFall");
        isJumping = false;
    }

    private IEnumerator ResetAttackCooldown()
    {
        // Reset the attack cooldown.
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.UpdateHPBar(currentHealth, maxHealth);

        // Play hurt animation.
        animator.SetTrigger("amHurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Play death animation.
        animator.SetTrigger("amDeath");

        // Drop money upon death
        coinManager.AddCoins(500);

        // Delay before destroying the slime object.
        float delayBeforeDestroy = 0.2f; // Adjust the delay time as needed.

        StartCoroutine(DelayedDestroy(delayBeforeDestroy));
    }

    private IEnumerator DelayedDestroy(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Destroy the slime object after the delay.
        Destroy(gameObject);
    }

}
