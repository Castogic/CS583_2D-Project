using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ClearSky;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text dialogueText;
    public Image iconFrame;
    public Image dialogueBackground;
    public Image npcIcon;
    private string[] currentDialogue;
    public float typingSpeed = 0.01f;

    private int currentLine = 0;
    private bool isTyping = false;
    public bool dialogueActive = false; // Flag to track if dialogue is active
    public SimplePlayerController playerController;
    private NPCInteraction npcInteraction;
    public AudioSource dialogueCharacter;

    private void Start()
    {
        // Initialize the dialogue text to be empty
        dialogueText.text = "";
        // Deactivate the dialogue UI elements at the start
        DeactivateDialogueUI();
    }

    private void Update()
    {
        if (dialogueActive)
        {
            // Check for player input to skip or advance the dialogue
            if (!isTyping && Input.GetKeyDown(KeyCode.E))
            {
                // If not typing, display the next line or end the dialogue
                DisplayNextLine();
            }
        }
    }

    public void StartDialogue(string[] dialogue, Sprite npcSprite, NPCInteraction npc)
    {
        currentDialogue = dialogue;
        currentLine = 0;
        dialogueText.text = "";
        npcIcon.sprite = npcSprite; // Set the NPC's icon
        npcInteraction = npc; // Assign the appropriate NPCInteraction script
        ActivateDialogueUI(); // Activate dialogue UI elements
        dialogueActive = true; // Mark dialogue as active
        StartCoroutine(StartTyping());
    }

    private IEnumerator StartTyping()
    {
        isTyping = true;
        foreach (char letter in currentDialogue[currentLine].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
            dialogueCharacter.Play();
        }
        isTyping = false;
    }

    private void DisplayNextLine()
    {
        if (currentLine < currentDialogue.Length - 1)
        {
            currentLine++;
            dialogueText.text = "";
            StartCoroutine(StartTyping());
        }
        else
        {
            // End of dialogue
            EndDialogue();
        }
    }

    private void CompleteLine()
    {
        // Finish typing the current line
        dialogueText.text = currentDialogue[currentLine];
        isTyping = false;
    }

    private void EndDialogue()
    {
        // Clear the dialogue text
        dialogueText.text = "";

        // Perform any other actions or close the dialogue UI
        Debug.Log("End of dialogue");
        DeactivateDialogueUI(); // Deactivate dialogue UI elements
        // Enable NPC interaction after dialogue ends
        playerController.canInteractWithNPC = true;
        dialogueActive = false; // Mark dialogue as inactive
        npcInteraction.FindNPC();
        npcInteraction.Unlock();
    }

    // Helper function to deactivate dialogue UI elements
    private void DeactivateDialogueUI()
    {
        dialogueBackground.gameObject.SetActive(false);
        iconFrame.gameObject.SetActive(false);
        npcIcon.gameObject.SetActive(false);
    }

    // Helper function to activate dialogue UI elements
    private void ActivateDialogueUI()
    {
        dialogueBackground.gameObject.SetActive(true);
        iconFrame.gameObject.SetActive(true);
        npcIcon.gameObject.SetActive(true);
    }
}
