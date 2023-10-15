using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropPlanter : MonoBehaviour
{
    private GameObject seedPrefab; // Assign crop in Inspector
    public float harvestRange = 1f; // Adjust the range within which the player can harvest crops
    public float activationRange = 1f; // Adjust the range within which the player can activate crops
    private Transform cropsContainer; // Reference to the parent GameObject holding the crop prefabs

    private void Start()
    {
        // Find the "Crops" parent GameObject by name
        cropsContainer = GameObject.Find("Crops").transform;
    }


    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            // Harvest a crop when the player presses "E"
            HarvestCrop();
        }
    }

    

    private void HarvestCrop()
    {
        // Create a raycast to check for nearby crops within the harvest range
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, harvestRange, LayerMask.GetMask("Crops"));

        if (hit.collider != null)
        {
            // Check if the raycast hit a crop GameObject
            GameObject crop = hit.collider.gameObject;

            // Check if the crop has a CropController script
            CropController cropController = crop.GetComponent<CropController>();

            if (cropController != null && cropController.CanHarvest())
            {
                // Harvest the crop using the CropController's Harvest method
                cropController.Harvest();
            }
        }
    }

}
