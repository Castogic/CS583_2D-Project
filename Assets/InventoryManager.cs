using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using ClearSky;

public class InventoryManager : MonoBehaviour
{
    public GameObject[] slotHighlights = new GameObject[9]; // Reference to 9 slot highlight icons.
    public GameObject[] slotIcons = new GameObject[9]; // Reference to 9 slot icons.
    public TextMeshProUGUI[] itemCounterTexts = new TextMeshProUGUI[9]; // Reference to TextMeshProUGUI elements for item quantities.
    public SimplePlayerController playerController; // Reference to the SimplePlayerController
    public int selectedSlotIndex = 0;
    public List<Item> hotbar = new List<Item>(9);
    private List<GameObject> itemIcons = new List<GameObject>();

    [SerializeField]
    public GameObject selectedSeedBag;
    public ItemType selectedSeedType = ItemType.Unknown;

    public enum ItemType
    {
        Carrot,
        Yam,
        Bean,
        Tomato,
        Pumpkin,

        CarrotSeed,
        YamSeed,
        BeanSeed,
        TomatoSeed,
        PumpkinSeed,

        Hoe,
        WateringCan,
        SlimeGel,
        Scroll,
        Unknown
        // More item types here
    }

    public GameObject itemIconPrefab_Carrot;
    public GameObject itemIconPrefab_Yam;
    public GameObject itemIconPrefab_Bean;
    public GameObject itemIconPrefab_Tomato;
    public GameObject itemIconPrefab_Pumpkin;
    public GameObject itemIconPrefab_CarrotSeed;
    public GameObject itemIconPrefab_YamSeed;
    public GameObject itemIconPrefab_BeanSeed;
    public GameObject itemIconPrefab_TomatoSeed;
    public GameObject itemIconPrefab_PumpkinSeed;
    public GameObject itemIconPrefab_SlimeGel;
    public GameObject itemIconPrefab_WateringCan;
    public GameObject itemIconPrefab_Hoe;
    public GameObject itemIconPrefab_Scroll;
    // More fields for other item types
    public bool isFarmer = false;

    private void Start()
    {
        // Initialize the slot highlights.
        for (int i = 0; i < 9; i++)
        {
            slotHighlights[i].SetActive(false); // Hide all highlights initially.
        }

        // Show the highlight for the first slot to indicate it's selected.
        slotHighlights[selectedSlotIndex].SetActive(true);

        // Initialize the hotbar with empty slots.
        for (int i = 0; i < 9; i++)
        {
            hotbar.Add(new Item("", null));
        }

        UpdateInventoryUI();
    }

    private GameObject GetItemIconPrefabForItemType(ItemType itemType)
    {
        // Return the appropriate item icon prefab based on the itemType.
        switch (itemType)
        {
            case ItemType.Carrot:
                return itemIconPrefab_Carrot;
            case ItemType.Yam:
                return itemIconPrefab_Yam;
            case ItemType.Bean:
                return itemIconPrefab_Bean;
            case ItemType.Tomato:
                return itemIconPrefab_Tomato;
            case ItemType.Pumpkin:
                return itemIconPrefab_Pumpkin;

            case ItemType.CarrotSeed:
                return itemIconPrefab_CarrotSeed;
            case ItemType.YamSeed:
                return itemIconPrefab_YamSeed;
            case ItemType.BeanSeed:
                return itemIconPrefab_BeanSeed;
            case ItemType.TomatoSeed:
                return itemIconPrefab_TomatoSeed;
            case ItemType.PumpkinSeed:
                return itemIconPrefab_PumpkinSeed;

            case ItemType.WateringCan:
                return itemIconPrefab_WateringCan;
            case ItemType.Hoe:
                return itemIconPrefab_Hoe;
            case ItemType.Scroll:
                return itemIconPrefab_Scroll;
            // More cases for other item types here
            default:
                return null; // Return null for unknown item types
        }
    }

    private void Update()
    {
        // Scrolling through the inventory.
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            // Hide the highlight of the currently selected slot.
            slotHighlights[selectedSlotIndex].SetActive(false);

            // Change the selected slot index.
            selectedSlotIndex -= (int)Mathf.Sign(scroll);
            selectedSlotIndex = Mathf.Clamp(selectedSlotIndex, 0, 8);

            // Show the highlight for the new selected slot.
            slotHighlights[selectedSlotIndex].SetActive(true);

            // Update UI for the new selected slot.
            UpdateInventoryUI();
        }

        // Input for selecting slots using keypad 1-9.
        for (int i = 0; i < 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                // Hide the highlight of the currently selected slot.
                slotHighlights[selectedSlotIndex].SetActive(false);

                // Update the selected slot index.
                selectedSlotIndex = i;

                // Show the highlight for the new selected slot.
                slotHighlights[selectedSlotIndex].SetActive(true);

                // Update UI for the new selected slot.
                UpdateInventoryUI();
                break;
            }
        }

        // Free seeds at the start of game
        if (isFarmer)
        {
            // Determine the item type based on the item's name.
            ItemType itemType = GetItemTypeFromName("CarrotSeed"); // Replace "Carrot" with the desired item name.

            // Instantiate the item icon based on the item type.
            GameObject itemIconPrefab = GetItemIconPrefabForItemType(itemType);

            // Create an item with a name and icon.
            Item newItem = new Item("CarrotSeed", itemIconPrefab.GetComponent<SpriteRenderer>().sprite); // Assumes that the sprite is on the prefab.

            // Call the AddItemToHotbar method to add the item.
            for (int i = 0; i < 5; i++)
                AddItemToHotbar(newItem);
            isFarmer = false;
        }
    }

    public void SetItemPrefabToBuy(string productName)
    {
        // Determine the item type based on the item's name.
        ItemType itemType = GetItemTypeFromName(productName);
        // Instantiate the item icon based on the item type.
        GameObject itemIconPrefab = GetItemIconPrefabForItemType(itemType);
        // Create an item with a name and icon.
        Item newItem = new Item(productName, itemIconPrefab.GetComponent<SpriteRenderer>().sprite);
        AddItemToHotbar(newItem);
        UpdateInventoryUI();
    }

    private void UpdateInventoryUI()
    {
        // Hide all highlights.
        for (int i = 0; i < 9; i++)
        {
            slotHighlights[i].SetActive(false);
        }

        // Show the highlight for the selected slot.
        slotHighlights[selectedSlotIndex].SetActive(true);

        // Show all icons in the inventory slots.
        for (int i = 0; i < 9; i++)
        {
            slotIcons[i].SetActive(true); // Show all icons initially.
        }

        // Loop through all slots to update item counters.
        for (int i = 0; i < 9; i++)
        {
            Item currentItem = hotbar[i];
            if (!string.IsNullOrEmpty(currentItem.itemName)) // Check if the slot is not empty.
            {
                itemCounterTexts[i].text = currentItem.quantity.ToString();
            }
            else
            {
                itemCounterTexts[i].text = "";
            }
        }
    }

    public void AddItemToHotbar(Item newItem)
    {
        // Determine the item type based on the item's name.
        ItemType itemType = GetItemTypeFromName(newItem.itemName);

        // Check if the item already exists in the hotbar.
        for (int i = 0; i < hotbar.Count; i++)
        {
            if (hotbar[i].itemName == newItem.itemName && hotbar[i].quantity < Item.maxQuantity)
            {
                hotbar[i].AddItem(); // If found, increment the quantity.
                UpdateInventoryUI();
                return;
            }
        }

        // If not found, find the first available slot and add the item.
        for (int i = 0; i < hotbar.Count; i++)
        {
            if (hotbar[i].itemName == "")
            {
                hotbar[i] = newItem;

                // Instantiate the item icon based on the item type.
                GameObject icon = Instantiate(GetItemIconPrefabForItemType(itemType), slotIcons[i].transform);
                icon.transform.localPosition = Vector3.zero;
                icon.SetActive(true); // Initially hide the icons.
                itemIcons.Add(icon);

                UpdateInventoryUI();
                return;
            }
        }

        // If no empty slots are found, do nothing (inventory is full).
    }

    // Helper method to get the item type based on the item's name.
    private ItemType GetItemTypeFromName(string itemName)
    {
        switch (itemName)
        {
            case "Carrot":
                return ItemType.Carrot;
            case "Yam":
                return ItemType.Yam;
            case "Bean":
                return ItemType.Bean;
            case "Tomato":
                return ItemType.Tomato;
            case "Pumpkin":
                return ItemType.Pumpkin;
            case "CarrotSeed":
                return ItemType.CarrotSeed;
            case "YamSeed":
                return ItemType.YamSeed;
            case "BeanSeed":
                return ItemType.BeanSeed;
            case "TomatoSeed":
                return ItemType.TomatoSeed;
            case "PumpkinSeed":
                return ItemType.PumpkinSeed;
            case "SlimeGel":
                return ItemType.SlimeGel;
            case "WateringCan":
                return ItemType.WateringCan;
            case "Hoe":
                return ItemType.Hoe;
            case "Scroll":
                return ItemType.Scroll;
            default:
                return ItemType.Unknown;
        }
    }

    public bool HasCarrotSeed()
    {
        // Loop through the hotbar to check if there's a CarrotSeed
        for (int i = 0; i < hotbar.Count; i++)
        {
            Item item = hotbar[i];
            if (item.itemName == "CarrotSeed")
            {
                if (item.quantity > 1)
                {
                    item.RemoveItem(); // Decrement the quantity.
                }
                else
                {
                    // Remove the item from the hotbar.
                    hotbar[i] = new Item("", null);
                    // Remove the corresponding icon.
                    Destroy(itemIcons[i]);
                    itemIcons.RemoveAt(i);
                }

                UpdateInventoryUI(); // Update the UI.
                return true; // Found a CarrotSeed
            }
        }

        return false; // CarrotSeed not found
    }

    public bool HasScroll()
    {
        // Get the currently selected slot's index.
        int currentIndex = selectedSlotIndex;

        // Check if the current slot has a Scroll.
        Item currentItem = hotbar[currentIndex];
        if (currentItem.itemName == "Scroll")
        {
            if (currentItem.quantity > 1)
            {
                currentItem.RemoveItem(); // Decrement the quantity.
            }
            else
            {
                // Remove the item from the hotbar.
                hotbar[currentIndex] = new Item("", null);
                // Remove the corresponding icon.
                Destroy(itemIcons[currentIndex]);
                itemIcons.RemoveAt(currentIndex);
            }

            UpdateInventoryUI(); // Update the UI.
            return true; // Found a scroll
        }

        return false; // Scroll not found in the current slot
    }

}
