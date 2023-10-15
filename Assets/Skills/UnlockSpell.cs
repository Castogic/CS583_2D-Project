using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClearSky;

public class UnlockSpell : MonoBehaviour
{
    public InventoryManager inventoryManager; // Reference to the InventoryManager script.
    public SimplePlayerController playerController;  // Reference to the SimplePlayerController script.

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(1)) // Check for right-click
        {
            if (inventoryManager.HasScroll())
            {
                playerController.UnlockNextLockedSpell();
            }
        }
    }
}
