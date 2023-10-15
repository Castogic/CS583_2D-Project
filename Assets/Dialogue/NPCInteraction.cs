using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public NPCDialogueData npcDialogueData;
    public Sprite npcIcon; // Reference to the NPC's icon
    private DialogueManager dialogueManager;
    private InventoryManager inventoryManager;
    public ShopManager farmShopManager;
    public ShopManager merchantShopManager;
    public bool isSlimeBlock = false;
    public bool isSlimeFarmer = false;
    public bool isSlimeMerchant = false;
    public CoinManager coinManager; // Reference to the CoinManager script.
    private bool hasInteracted = false;

    private void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        inventoryManager = FindObjectOfType<InventoryManager>();
    }

    public void InteractWithNPC()
    {
        if (!hasInteracted)
        {
            dialogueManager.StartDialogue(npcDialogueData.firstTimeDialogue, npcIcon, this);
            hasInteracted = true;
            if (isSlimeFarmer)
            {
                // Add items to the player's inventory
                inventoryManager.isFarmer = true;
            }
        }
        else
        {
            dialogueManager.StartDialogue(npcDialogueData.regularDialogue, npcIcon, this);
        }

    }

    public void Unlock()
    {
        if (isSlimeBlock)
        {
            // Find a GameObject named "SlimeBlock" and remove it
            GameObject slimeBlock = GameObject.Find("SlimeBlock");
            if (slimeBlock != null && coinManager.HasEnoughCoins(3000))
            {
                // Remove the `SlimeBlock` from the scene
                Destroy(slimeBlock);
                
                // Subtract coins from the player's coin manager
                coinManager.SubtractCoins(3000);
            }
            else
            {
                Debug.LogWarning("SlimeBlock not found in the scene.");
            }
        }
    }

    public void FindNPC()
    {
        if (npcDialogueData.canOpenShop)
        {
            if (isSlimeFarmer)
            {
                // Open farmer shop
                farmShopManager.OpenShop();
            }
            else if (isSlimeMerchant)
            {
                // Open merchant shop
                merchantShopManager.OpenShop();
            }
        }
    }

}
