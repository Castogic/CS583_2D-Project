using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Deprecated, refer to SimplePlayerController.cs
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Adjust this to control the player's speed.

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();

        // Set the Animator Controller to "Wizard Demo."
        animator.runtimeAnimatorController = Resources.Load("Elf Wizard/2D Character/Animations/Wizard Demo") as RuntimeAnimatorController;
    }

    private void Update()
    {
        // Determine if the player is moving horizontally (pressing "A" or "D" keys)
        bool isMovingHorizontally = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);

        // Set the "IsMoving" parameter in the Animator based on horizontal movement
        animator.SetBool("IsRun", isMovingHorizontally);

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Set animation parameters based on player input
        // animator.SetFloat("Speed", Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));

        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0f) * moveSpeed * Time.deltaTime;

        transform.Translate(movement);
    }
}
