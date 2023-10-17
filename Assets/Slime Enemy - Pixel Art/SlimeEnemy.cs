using System.Collections;
using UnityEngine;
using ClearSky;
public class SlimeEnemy : MonoBehaviour
{
    public float moveSpeed = 4.0f;            // Speed at which the slime moves.
    public float jumpForce = 5.0f;            // Force applied for jumping.
    public float detectionRange = 5.0f;       // Range at which the slime detects the player.
    public float attackRange = 1.0f;          // Range at which the slime attacks the player.
    public float attackCooldown = 2.0f;       // Cooldown between attacks.
    public float attackStopDistance = 0.5f;   // Distance at which the slime stops while attacking.
    public AudioSource slimeJump;
    public AudioSource slimeHurt;

    private Transform player;                 // Reference to the player's transform.
    private Animator animator;               // Reference to the Animator component.
    private bool isJumping = false;           // Flag to track jumping state.
    private bool canAttack = true;            // Flag to track attack cooldown.
    public int maxHealth = 20;
    [SerializeField] SlimeHealthBar healthBar;

    public int currentHealth;
    private Vector3 originalSpawnPosition;

    private float lastRespawnTime = 0f;
    private bool isRespawning = false;
    private bool isDead = false; // Flag to track if the slime is dead.

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

        // Reset its health or any other attributes.
        currentHealth = maxHealth;
        healthBar.UpdateHPBar(currentHealth, maxHealth);

        // Set the enemy to idle state.
        animator.SetTrigger("idle");

        // Reactivate the slime.
        isDead = false;

        // Activate the health bar when respawning.
        healthBar.gameObject.SetActive(true);

        Renderer slimeRenderer = GetComponent<Renderer>();
        Collider2D slimeCollider = GetComponent<Collider2D>();

        if (slimeRenderer != null)
        {
            slimeRenderer.enabled = true;
        }

        if (slimeCollider != null)
        {
            slimeCollider.enabled = true;
        }

        isRespawning = false;
    }

    private void Update()
    {
        // Check if the slime is dead and should respawn.
        if (isDead && isRespawning == false)
        {
            healthBar.gameObject.SetActive(false); // Hide HP bar
            // Start the respawn coroutine with a 30-second delay.
            StartCoroutine(RespawnAfterDelay(30.0f));
            lastRespawnTime = Time.time;
            isRespawning = true;
        }

        if (isDead)
        {
            return; // Don't move if the slime is dead.
        }

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
        if (isDead)
        {
            return; // Don't attack if the slime is dead.
        }
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
        if (isDead)
        {
            yield break; // Exit the coroutine if the slime is dead.
        }

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

        slimeJump.Play();

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
        if (isDead)
            return; // Don't take damage if already dead.

        currentHealth -= damage;
        healthBar.UpdateHPBar(currentHealth, maxHealth);

        // Play hurt animation.
        animator.SetTrigger("amHurt");
        slimeHurt.Play();

        // Move towards the player when taking damage.
        Vector3 moveDirection = (player.position - transform.position).normalized;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return; // Don't die again if already dead.

        isDead = true; // Mark the slime as dead.

        // Play death animation.
        animator.SetTrigger("amDeath");

        // Drop money upon death
        coinManager.AddCoins(500);

        // Disable the renderer and collider to make the slime "disappear."
        Renderer slimeRenderer = GetComponent<Renderer>();
        Collider2D slimeCollider = GetComponent<Collider2D>();

        if (slimeRenderer != null)
        {
            slimeRenderer.enabled = false;
        }

        if (slimeCollider != null)
        {
            slimeCollider.enabled = false;
        }

        // Deactivate the health bar.
        healthBar.gameObject.SetActive(false);
    }

}
