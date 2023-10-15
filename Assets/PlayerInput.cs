using UnityEngine;

// Deprecated, refer to SimplePlayerController.cs
public class PlayerInput : MonoBehaviour
{
    // Variable to track the selected skill (1 for Fireball, 2 for Heal, 3 for Disintegration).
    private int selectedSkill = 1;

    // Reference to the player's attack prefab (e.g., Fireball prefab).
    public GameObject attackPrefab; // Assign this in the Inspector.

    void Update()
    {
        HandleSkillSelection();
        HandleAttackInput();
    }

    // Implement skill selection logic here.
    private void HandleSkillSelection()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedSkill = 1;
            // TODO: Update UI to indicate the selected skill.
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedSkill = 2;
            // TODO: Update UI to indicate the selected skill.
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedSkill = 3;
            // TODO: Update UI to indicate the selected skill.
        }
    }

    // Implement attack casting logic here.
    private void HandleAttackInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Detect left mouse button click.
            
            // Use raycasting to check if the player clicked on a valid target.
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            
            if (hit.collider != null)
            {
                // Instantiate the attack based on the selected skill and mouse position.
                if (selectedSkill == 1) // Fireball
                {
                    // Instantiate the fireball attack prefab.
                    Instantiate(attackPrefab, transform.position, Quaternion.identity);
                }
                // TODO: Add similar code for other skills (e.g., Ice Shard, Lightning Bolt)
            }
        }
    }
}
