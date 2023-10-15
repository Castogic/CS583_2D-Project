using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    // All UI assets for the shop, activate when shop is open
    public GameObject shopFrame;
    public GameObject NPCui;
    public GameObject HighlightBar;
    public GameObject shopSlot1;
    public GameObject coinSlot;
    public GameObject NPCicon; 
    public GameObject NPCtext;
    public GameObject itemForSale;
    public TextMeshProUGUI itemCost;
    public GameObject NPCname;
    public GameObject costUI;
    public int costItem;
    public string itemName;

    public CoinManager coinManager; // Reference to the CoinManager script.
    
    public InventoryManager inventoryManager; // Reference to the InventoryManager script.

    private bool isShopOpen = false;

    private void Start()
    {
        // Initialize UI elements
        // Hide the shop UI initially.
        shopSlot1.SetActive(false);
        NPCui.SetActive(false);
        shopFrame.SetActive(false);
        coinSlot.SetActive(false);
        NPCicon.SetActive(false);
        NPCtext.SetActive(false);
        itemForSale.SetActive(false);
        NPCname.SetActive(false);
        HighlightBar.SetActive(false);
        costUI.SetActive(false);
    }

    private void Update()
    {
        // Handle input to open/close the shop and navigate items.
        if (isShopOpen)
        {
            // Buy items
            if (Input.GetKeyDown(KeyCode.Return))
            {
                BuySelectedItem();
            }

            // Close shop 
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseShop();
            }
        }
    }

    // Set the shop item's icon, name, and cost in the shop slots.
    public void OpenShop()
    {
        // Show the UI
        shopSlot1.SetActive(true);
        NPCui.SetActive(true);
        shopFrame.SetActive(true);
        coinSlot.SetActive(true);
        NPCicon.SetActive(true);
        NPCtext.SetActive(true);
        itemForSale.SetActive(true);
        NPCname.SetActive(true);
        HighlightBar.SetActive(true);
        costUI.SetActive(true);

        // Set the shop as open.
        isShopOpen = true;
    }

    public void CloseShop()
    {
        // Close the shop UI.
        shopSlot1.SetActive(false);
        NPCui.SetActive(false);
        shopFrame.SetActive(false);
        coinSlot.SetActive(false);
        NPCicon.SetActive(false);
        NPCtext.SetActive(false);
        itemForSale.SetActive(false);
        NPCname.SetActive(false);
        HighlightBar.SetActive(false);
        costUI.SetActive(false);

        // Set the shop as closed.
        isShopOpen = false;
    }

    // Use selectedItemIndex to determine which item the player wants to buy.
    // Deduct coins from the player's inventory and add the item to their inventory.
    public void BuySelectedItem()
    {
        if (isShopOpen)
        {
            // Check if the player has enough coins to buy the item.
            if (coinManager != null && coinManager.HasEnoughCoins(costItem))
            {
                // Deduct the item cost from the player's coins.
                coinManager.SubtractCoins(costItem);

                // Initialize purchased utem and add to inventory
                inventoryManager.SetItemPrefabToBuy(itemName);
            }
            else
            {
                // Player doesn't have enough coins, do nothing
            }
        }
    }
}
