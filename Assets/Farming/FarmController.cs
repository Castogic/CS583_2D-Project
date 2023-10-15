using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FarmController : MonoBehaviour
{
    public Sprite[] cropStages; // Array of crop growth stage sprites
    public Text currencyText;
    public Text seedsText;
    public GameObject CropPrefab;

    private int currency = 0;
    private int seeds = 5; // Starting number of seeds

    private GameObject currentCrop;
    private int currentStage = 0;
    private float growthTimer = 5f; // Time in seconds for each growth stage

    private void Start()
    {
        UpdateUI();
    }

    private void Update()
    {
        // Check for player input (e.g., mouse click or tap)
        if (Input.GetMouseButtonDown(0) && seeds > 0)
        {
            PlantCrop();
        }
    }

    private void PlantCrop()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        currentCrop = Instantiate(CropPrefab, mousePos, Quaternion.identity);
        StartCoroutine(GrowCrop());

        seeds--; // Decrement seeds
        UpdateUI();
    }

    private IEnumerator GrowCrop()
    {
        while (currentStage < cropStages.Length - 1)
        {
            yield return new WaitForSeconds(growthTimer);
            currentStage++;
            currentCrop.GetComponent<SpriteRenderer>().sprite = cropStages[currentStage];
        }
    }

    public void HarvestCrop()
    {
        currency++; // Increase currency when harvesting
        Destroy(currentCrop);
        currentStage = 0; // Reset growth stage
        seeds++; // Gain a seed when harvesting
        UpdateUI();
    }

    private void UpdateUI()
    {
        currencyText.text = "Currency: " + currency.ToString();
        seedsText.text = "Seeds: " + seeds.ToString();
    }
}