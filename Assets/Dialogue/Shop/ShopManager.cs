using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    // All UI assets for the shop, activate when shop is open
    public GameObject npcUi;

    public GameObject shopSlot1;
    public GameObject coinSlot1;
    public GameObject shopSlot2;
    public GameObject coinSlot2;
    public GameObject shopSlot3;
    public GameObject coinSlot3;
    public GameObject shopSlot4;
    public GameObject coinSlot4;

    public GameObject npcIcon; 
    public GameObject npcText;

    public GameObject itemForSale1;
    public GameObject itemForSale2;
    public GameObject itemForSale3;
    public GameObject itemForSale4;

    public GameObject itemCost1;
    public GameObject itemCost2;
    public GameObject itemCost3;
    public GameObject itemCost4;

    public GameObject npcName;
    public AudioSource buy;

    private int costItem1 = 25;
    private int costItem2 = 50;
    private int costItem3 = 100;
    private int costItem4 = 150;
    private string itemName1 = "CarrotSeed";
    private string itemName2 = "YamSeed";
    private string itemName3 = "TomatoSeed";
    private string itemName4 = "PumpkinSeed";

    public CoinManager coinManager; // Reference to the CoinManager script.
    
    public InventoryManager inventoryManager; // Reference to the InventoryManager script.

    private bool isShopOpen = false;

    private void Start()
    {
        // Initialize UI elements
        // Hide the shop UI initially.
        shopSlot1.SetActive(false);
        shopSlot2.SetActive(false);
        shopSlot3.SetActive(false);
        shopSlot4.SetActive(false);

        coinSlot1.SetActive(false);
        coinSlot2.SetActive(false);
        coinSlot3.SetActive(false);
        coinSlot4.SetActive(false);

        itemCost1.SetActive(false);
        itemCost2.SetActive(false);
        itemCost3.SetActive(false);
        itemCost4.SetActive(false);

        npcUi.SetActive(false);
        npcIcon.SetActive(false);
        npcText.SetActive(false);
        npcName.SetActive(false);

        itemForSale1.SetActive(false);
        itemForSale2.SetActive(false);
        itemForSale3.SetActive(false);
        itemForSale4.SetActive(false);
    }

    private void Update()
    {
        // Handle input to open/close the shop and navigate items.
        if (isShopOpen)
        {
            // Buy items
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                BuySelectedItem(costItem1, itemName1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                BuySelectedItem(costItem2, itemName2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                BuySelectedItem(costItem3, itemName3);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                BuySelectedItem(costItem4, itemName4);
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
        shopSlot2.SetActive(true);
        shopSlot3.SetActive(true);
        shopSlot4.SetActive(true);

        coinSlot1.SetActive(true);
        coinSlot2.SetActive(true);
        coinSlot3.SetActive(true);
        coinSlot4.SetActive(true);

        itemCost1.SetActive(true);
        itemCost2.SetActive(true);
        itemCost3.SetActive(true);
        itemCost4.SetActive(true);

        npcUi.SetActive(true);
        npcIcon.SetActive(true);
        npcText.SetActive(true);
        npcName.SetActive(true);

        itemForSale1.SetActive(true);
        itemForSale2.SetActive(true);
        itemForSale3.SetActive(true);
        itemForSale4.SetActive(true);

        // Set the shop as open.
        isShopOpen = true;
    }

    public void CloseShop()
    {
        // Close the shop UI.
        shopSlot1.SetActive(false);
        shopSlot2.SetActive(false);
        shopSlot3.SetActive(false);
        shopSlot4.SetActive(false);

        coinSlot1.SetActive(false);
        coinSlot2.SetActive(false);
        coinSlot3.SetActive(false);
        coinSlot4.SetActive(false);

        itemCost1.SetActive(false);
        itemCost2.SetActive(false);
        itemCost3.SetActive(false);
        itemCost4.SetActive(false);

        npcUi.SetActive(false);
        npcIcon.SetActive(false);
        npcText.SetActive(false);
        npcName.SetActive(false);

        itemForSale1.SetActive(false);
        itemForSale2.SetActive(false);
        itemForSale3.SetActive(false);
        itemForSale4.SetActive(false);

        // Set the shop as closed.
        isShopOpen = false;
    }

    // Use selectedItemIndex to determine which item the player wants to buy.
    // Deduct coins from the player's inventory and add the item to their inventory.
    public void BuySelectedItem(int cost, string name)
    {
        if (isShopOpen)
        {
            // Check if the player has enough coins to buy the item.
            if (coinManager != null && coinManager.HasEnoughCoins(cost))
            {
                // Deduct the item cost from the player's coins.
                coinManager.SubtractCoins(cost);

                // Initialize purchased utem and add to inventory
                inventoryManager.SetItemPrefabToBuy(name);

                buy.Play();
            }
            else
            {
                // Player doesn't have enough coins, do nothing
            }
        }
    }
}
