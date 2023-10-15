using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropController : MonoBehaviour
{
    public GameObject hiddenStage;
    public GameObject seedStage;
    public GameObject sproutStage;
    public GameObject youngStage;
    public GameObject matureStage;
    public GameObject harvestStage;

    private float growthTimer = 0f;
    private bool isPlanted = false;
    private bool isHarvestable = false;

    public CoinManager coinManager;

    private float timeForSeedToSprout = 10f;
    private float timeForSproutToYoung = 20f;
    private float timeForYoungToMature = 30f;
    private float timeForMatureToHarvest = 40f;

    private int currentStage = -1; // -1 for unplanted, 0 for seed, 1 for sprout, 2 for young, 3 for mature, 4 for harvest

    void Start()
    {
        // Initialize the crop as unplanted with the hidden stage active
        SetStage(-1);
    }

    void Update()
    {
        if (isPlanted)
        {
            growthTimer += Time.deltaTime;

            if (growthTimer >= timeForSeedToSprout && currentStage < 1)
            {
                SetStage(1); // Change to sprout stage
            }
            else if (growthTimer >= timeForSproutToYoung && currentStage < 2)
            {
                SetStage(2); // Change to young stage
            }
            else if (growthTimer >= timeForYoungToMature && currentStage < 3)
            {
                SetStage(3); // Change to mature stage
            }
            else if (growthTimer >= timeForMatureToHarvest && currentStage < 4)
            {
                SetStage(4); // Change to harvest stage
            }
        }
    }

    public void Plant()
    {
        if (!isPlanted)
        {
            isPlanted = true;
            growthTimer = 0f;
            SetStage(0); // Change to seed stage
        }
    }

    public void Harvest()
    {
        if (isHarvestable)
        {
            coinManager.AddCoins(50);
            isPlanted = false;
            SetStage(-1); // Change to unplanted state with hidden stage
        }
    }

    public bool CanHarvest()
    {
        return isHarvestable && currentStage == 4;
    }

    private void SetStage(int stage)
    {
        // Deactivate all stages
        seedStage.SetActive(false);
        sproutStage.SetActive(false);
        youngStage.SetActive(false);
        matureStage.SetActive(false);
        harvestStage.SetActive(false);
        hiddenStage.SetActive(false);

        currentStage = stage;

        // Activate the appropriate stage
        switch (currentStage)
        {
            case -1:
                hiddenStage.SetActive(true);
                isHarvestable = false;
                isPlanted = false;
                break;
            case 0:
                seedStage.SetActive(true);
                break;
            case 1:
                sproutStage.SetActive(true);
                break;
            case 2:
                youngStage.SetActive(true);
                break;
            case 3:
                matureStage.SetActive(true);
                break;
            case 4:
                harvestStage.SetActive(true);
                isHarvestable = true;
                break;
        }
    }

    public bool IsCropNotPlanted()
    {
        return isPlanted;
    }
}
