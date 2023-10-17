using UnityEngine;
using System.Collections;
using TMPro;

namespace ClearSky
{
    public class SimplePlayerController : MonoBehaviour
    {
        public InventoryManager inventoryManager;
        public PlayerHealth playerHealth;

        public CropPlanter cropPlanterr;
        public float movePower = 10f;
        public float jumpPower = 15f;
        
        private Rigidbody2D rb;
        private Animator anim;
        private int direction = 1;
        public bool alive = true;
        public float interactionDistance = 1f;

        // Reference to the player's attack prefabs for each skill.
        public GameObject[] attackPrefabs = new GameObject[3]; // Assign these in the Inspector.
        // Array of UI icons for each skill.
        public GameObject[] skillUIIcons = new GameObject[3]; // Assign these in the Inspector.
        private int selectedSkill = 0;
        public float attackCooldown = 1.0f; // Cooldown time in seconds for the attack.
        private float lastAttackTime = 0.0f;
        private float movementLockDuration = 2.0f; // Duration to lock movement when using Skill 2.
        private bool isMovementLocked = false;
        // Declare an array to hold destruction times for each skill.
        public float[] skillDestructionTimes = new float[3] { 1.0f, 10.5f, 1.4f };

        public Transform respawnPoint; // Set this in the Inspector to specify the respawn point.
        public float fadeDuration = 1.0f; // Duration of the fade effect in seconds.
        public bool fadeOut = false; // True if the player dies to restart

        public float attackRange = 2.0f; // Set the default attack range here.
        public bool canHurt = false; // Initialize to false

        // Add references to the DialogueManager and NPCInteraction components
        public DialogueManager dialogueManager;
        private NPCInteraction currentNPC;
        public bool canInteractWithNPC = true;
        private bool[] spellLockedStates = new bool[3];

        // Reference to the player's GameObject
        private GameObject player;

        // Spell sounds
        public AudioSource[] skillAudioSources = new AudioSource[3];
        public AudioSource plantCrop;

        [SerializeField]
        private TextMeshProUGUI quoteText;

        // Start is called before the first frame update

        private void Awake()
        {
            // Initialize the locked spell states.
            for (int i = 1; i < spellLockedStates.Length; i++) // Start from 1 to skip the first spell.
            {
                spellLockedStates[i] = true;
            }

            // Ensure that the inventoryManager reference is properly assigned.
            if (inventoryManager == null)
            {
                Debug.LogError("InventoryManager reference not set in PlayerController.");
            }
            // Initialize the player reference.
            player = gameObject;
            // Hide all skill icons first (if any are visible).
            foreach (var icon in skillUIIcons)
            {
                icon.SetActive(false);
            }
        }

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
        }

        private void Update()
        {
            Restart();
            // Check for nearby NPCs
            CheckForNearbyNPC();
            if (alive)
            {
                Hurt();
                Die();
                SwitchSkill();

                // Check if enough time has passed to allow another attack.
                if (Time.time - lastAttackTime >= attackCooldown)
                {
                    Attack();
                }

                // Check for player interaction with an NPC
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (currentNPC != null)
                    {
                        if (canInteractWithNPC)
                        {
                            currentNPC.InteractWithNPC();
                            canInteractWithNPC = false;
                        }
                    }
                }
                if (Input.GetKeyDown(KeyCode.E))
                {
                    CheckForNearbyCrop();
                }
                // Godmode
                if (Input.GetKeyDown(KeyCode.G))
                {
                    PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
                    if (playerHealth  != null)
                    {
                        playerHealth.currentHealth += 9999;
                    }
                }

                Move();
            }
        }

        // Function to switch between skills when the player presses keys 1-9.
        void SwitchSkill()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                // Find the next unlocked spell.
                int nextSpell = (selectedSkill + 1) % spellLockedStates.Length;
                while (spellLockedStates[nextSpell])
                {
                    nextSpell = (nextSpell + 1) % spellLockedStates.Length;
                }

                selectedSkill = nextSpell;
                ShowSkillUI(selectedSkill);
            }
        }

        void ShowSkillUI(int skillIndex)
        {
            // Check if the skill index is within a valid range.
            if (skillIndex >= 0 && skillIndex < skillUIIcons.Length)
            {
                // Hide all skill icons first (if any are visible).
                foreach (var icon in skillUIIcons)
                {
                    icon.SetActive(false);
                }

                // Show the UI icon for the selected skill.
                skillUIIcons[skillIndex].SetActive(true);

                // Start a coroutine to hide the icon after 5 seconds.
                StartCoroutine(HideSkillUIAfterDelay(skillIndex, 5.0f));
            }
        }

        // Coroutine to hide the skill icon after a specified delay.
        IEnumerator HideSkillUIAfterDelay(int skillIndex, float delay)
        {
            yield return new WaitForSeconds(delay);

            // Check if the skill icon is still the selected skill (it hasn't changed meanwhile).
            if (selectedSkill == skillIndex)
            {
                skillUIIcons[skillIndex].SetActive(false);
            }
        }

        // Function to lock a spell.
        void LockSpell(int spellIndex)
        {
            if (spellIndex >= 1 && spellIndex < spellLockedStates.Length)
            {
                spellLockedStates[spellIndex] = true;
            }
        }

        // Function to unlock the next locked spell.
        public void UnlockNextLockedSpell()
        {
            for (int i = 1; i < spellLockedStates.Length; i++) // Start from 1 to skip the first spell.
            {
                if (spellLockedStates[i])
                {
                    spellLockedStates[i] = false;
                    return;
                }
            }
        }

        void Move()
        {
            // Check if movement is locked (e.g., during Skill 2's effect).
            if (isMovementLocked)
            {
                // Don't allow movement and stop the animation.
                anim.SetBool("isRun", false);
                return;
            }

            Vector3 moveVelocity = Vector3.zero;

            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");

            // Set animation parameter "isRun" based on any movement input
            bool isMoving = (Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput)) > 0;

            // Update the animator only if movement is allowed.
            if (!isMovementLocked)
            {
                anim.SetBool("isRun", isMoving);
            }

            // Move the character
            moveVelocity = new Vector3(horizontalInput, verticalInput, 0f).normalized;

            if (moveVelocity != Vector3.zero)
            {
                direction = (int)Mathf.Sign(horizontalInput);
                transform.localScale = new Vector3(direction, 1, 1);
            }

            transform.position += moveVelocity * movePower * Time.deltaTime;
        }

        void Attack()
        {
            if (Input.GetMouseButtonDown(0))
            {
                anim.SetTrigger("attack");

                // Check if the selectedSkill is within a valid range (0 to 3).
                if (selectedSkill >= 0 && selectedSkill < attackPrefabs.Length)
                {
                    // Play the sound for the selected skill.
                    if (selectedSkill >= 0 && selectedSkill < skillAudioSources.Length)
                    {
                        skillAudioSources[selectedSkill].Play();
                    }

                    if (selectedSkill == 1) // Skill 2 exception
                    {
                        // For Skill 2, move the attack effect a bit higher and to the left.
                        Vector3 skill2Offset = new Vector3(-1.0f, 2.0f, 0.0f);
                        Vector3 skill2Position = transform.position + skill2Offset;
                        GameObject skill2Attack = Instantiate(attackPrefabs[selectedSkill], skill2Position, Quaternion.identity);
                        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
                        if (playerHealth != null)
                        {
                            if (playerHealth.currentHealth < 4)
                            {
                                if (playerHealth.currentHealth == 3.5f)
                                {
                                    playerHealth.currentHealth += 0.5f;
                                }
                                else
                                {
                                    playerHealth.currentHealth += 1;
                                }
                            }
                            playerHealth.UpdateUI();
                        }
                        // Destroy the Skill 2 attack effect after a certain delay (e.g., 1.5 seconds).
                        Destroy(skill2Attack, skillDestructionTimes[selectedSkill]);

                        // Lock player movement for the same duration as Skill 2's effect.
                        StartCoroutine(LockMovement(movementLockDuration));
                    }
                    else if (selectedSkill == 2) // Skill 3
                    {
                        // For Skill 3, calculate the position to instantiate the attack.
                        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        Vector3 attackPosition = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);
                        GameObject skillAttack = Instantiate(attackPrefabs[selectedSkill], attackPosition, Quaternion.identity);
                        
                        // Find all SlimeEnemy objects in the scene.
                        SlimeEnemy[] slimeEnemies = FindObjectsOfType<SlimeEnemy>();

                        foreach (SlimeEnemy slimeEnemy in slimeEnemies)
                        {
                            // Calculate the distance between the slime and the attack position.
                            float distance = Vector2.Distance(slimeEnemy.transform.position, attackPosition);

                            // Check if the slime is within the attack range.
                            if (distance <= attackRange)
                            {
                                // Deduct 10 health from the slime for Skill 3.
                                slimeEnemy.TakeDamage(10);
                            }
                        }

                        // Destroy the attack effect after the determined destruction time.
                        Destroy(skillAttack, skillDestructionTimes[selectedSkill]);
                    }
                    else // For other skills (including Skill 1)
                    {
                        // Calculate the position to instantiate the attack.
                        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        Vector3 attackPosition = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);
                        GameObject skillAttack = Instantiate(attackPrefabs[selectedSkill], attackPosition, Quaternion.identity);
                        
                        // Find all SlimeEnemy objects in the scene.
                        SlimeEnemy[] slimeEnemies = FindObjectsOfType<SlimeEnemy>();

                        foreach (SlimeEnemy slimeEnemy in slimeEnemies)
                        {
                            // Calculate the distance between the slime and the attack position.
                            float distance = Vector2.Distance(slimeEnemy.transform.position, attackPosition);

                            // Check if the slime is within the attack range.
                            if (distance <= attackRange)
                            {
                                // Deduct 2 health from the slime for Skill 1 and others.
                                slimeEnemy.TakeDamage(2);
                            }
                        }

                        // Destroy the attack effect after the determined destruction time.
                        Destroy(skillAttack, skillDestructionTimes[selectedSkill]);
                    }
                    
                    // Update the last attack time to the current time.
                    lastAttackTime = Time.time;
                }
            }
        }

        // Coroutine to lock player movement for a specified duration.
        IEnumerator LockMovement(float duration)
        {
            isMovementLocked = true;
            yield return new WaitForSeconds(duration);
            isMovementLocked = false;
        }

        public void Hurt()
        {
            if (canHurt && alive) {
                anim.SetTrigger("hurt");
                /*
                if (direction == 1)
                    rb.AddForce(new Vector2(-5f, 1f), ForceMode2D.Impulse);
                else
                    rb.AddForce(new Vector2(5f, 1f), ForceMode2D.Impulse);
                */
                canHurt = false;
            }
        }

        public void Die()
        {
            if (!alive) {
                anim.SetTrigger("die");
                // Start the fade-out effect.
                //
                fadeOut = true;
                StartCoroutine(FadeOutAndRespawn());
            }
        }

        private IEnumerator FadeOutAndRespawn()
        {
            if (fadeOut) {
                // Access the ScreenFader script
                ScreenFader screenFader = FindObjectOfType<ScreenFader>();

                if (screenFader != null)
                {
                    yield return StartCoroutine(screenFader.FadeOut());

                    // Move the player to the respawn point.
                    transform.position = respawnPoint.position;

                    // Reset the player's health.
                    //PlayerHealth playerHealth = GetComponent<PlayerHealth>();
                    PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
                    if (playerHealth != null)
                    {
                        playerHealth.restoreHP = true;
                        playerHealth.RestoreHealth();
                    }
                    alive = true;
                    string[] deathQuotes = {
                        "In the end, even heroes must find rest in the shadows.",
                        "The battle may be lost, but the war for our world endures.",
                        "From dust we came, to dust we return. Yet our legend lives on.",
                        "A hero's tale is often etched in sacrifice and valor.",
                        "The darkness claims this battle, but our spirit remains unbroken.",
                        "In the tapestry of fate, this thread may be severed, but the story goes on.",
                        "Fear not the abyss, for from it, heroes are born anew.",
                        "This is not the end, but merely a chapter in our epic saga.",
                        "The stars weep for our fallen, but destiny has more in store.",
                        "To fall in battle is an honor; to rise again, a legend."
                    };
                    int quoteLength = deathQuotes.Length;
                    string randomQuote = deathQuotes[Random.Range(0, quoteLength)];
                    quoteText.text = randomQuote;
                    yield return new WaitForSeconds(8f);
                    // Hide the quote
                    quoteText.text = "";

                    // Trigger the fade-in effect.
                    yield return StartCoroutine(screenFader.FadeIn());

                    // Allow further actions.
                    alive = true;
                }
                fadeOut = false;
                playerHealth.isDead = false;
            }
        }

        void Restart()
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                anim.SetTrigger("idle");
                alive = true;
            }
        }

        private void CheckForNearbyNPC()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * direction, interactionDistance, LayerMask.GetMask("NPC"));

            if (hit.collider != null)
            {
                // Check if the raycast hit an NPCInteraction component
                NPCInteraction npcInteraction = hit.collider.gameObject.GetComponent<NPCInteraction>();

                if (npcInteraction != null)
                {
                    currentNPC = npcInteraction;
                }
            }
            else
            {
                currentNPC = null;
            }
        }

        private void CheckForNearbyCrop()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * direction, interactionDistance, LayerMask.GetMask("Crops"));

            if (hit.collider != null)
            {
                // Check if the raycast hit an NPCInteraction component
                CropController cropController = hit.collider.gameObject.GetComponent<CropController>();

                if (cropController != null)
                {
                    if (cropController.CanHarvest())
                    {
                        cropController.Harvest();
                    }
                    else if ( (cropController.IsCropNotPlanted() == false) && (inventoryManager.HasSeed() == true))
                    {
                        string nameOfPlant = inventoryManager.chosenCrop;
                        plantCrop.Play();
                        cropController.Plant(nameOfPlant);
                    }
                }
            }
        }


    }
}
