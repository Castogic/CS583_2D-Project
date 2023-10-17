using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropController : MonoBehaviour
{
    public GameObject hiddenStage;
    
    public GameObject carrotSeedStage;
    public GameObject carrotSproutStage;
    public GameObject carrotYoungStage;
    public GameObject carrotMatureStage;
    public GameObject carrotHarvestStage;

    public GameObject yamSeedStage;
    public GameObject yamSproutStage;
    public GameObject yamYoungStage;
    public GameObject yamMatureStage;
    public GameObject yamHarvestStage;

    public GameObject tomatoSeedStage;
    public GameObject tomatoSproutStage;
    public GameObject tomatoYoungStage;
    public GameObject tomatoMatureStage;
    public GameObject tomatoHarvestStage;

    public GameObject pumpkinSeedStage;
    public GameObject pumpkinSproutStage;
    public GameObject pumpkinYoungStage;
    public GameObject pumpkinMatureStage;
    public GameObject pumpkinHarvestStage;

    private float growthTimer = 0f;
    private bool isPlanted = false;
    private bool isHarvestable = false;

    public CoinManager coinManager;

    private float timeForSeedToSprout = 10f;
    private float timeForSproutToYoung = 20f;
    private float timeForYoungToMature = 30f;
    private float timeForMatureToHarvest = 40f;

    private int currentStage = -1;
    private string nameOfCrop = "";

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

            if (nameOfCrop == "CarrotSeed")
            {
                if (growthTimer >= timeForSeedToSprout && currentStage == 0)
                {
                    SetStage(1); // Change to sprout stage
                }
                else if (growthTimer >= timeForSproutToYoung && currentStage == 1)
                {
                    SetStage(2); // Change to young stage
                }
                else if (growthTimer >= timeForYoungToMature && currentStage == 2)
                {
                    SetStage(3); // Change to mature stage
                }
                else if (growthTimer >= timeForMatureToHarvest && currentStage == 3)
                {
                    SetStage(4); // Change to harvest stage
                }
            }
            else if (nameOfCrop == "YamSeed")
            {
                if (growthTimer >= timeForSeedToSprout && currentStage == 5)
                {
                    SetStage(6); // Change to sprout stage
                }
                else if (growthTimer >= timeForSproutToYoung && currentStage == 6)
                {
                    SetStage(7); // Change to young stage
                }
                else if (growthTimer >= timeForYoungToMature && currentStage == 7)
                {
                    SetStage(8); // Change to mature stage
                }
                else if (growthTimer >= timeForMatureToHarvest && currentStage == 8)
                {
                    SetStage(9); // Change to harvest stage
                }
            }
            else if (nameOfCrop == "TomatoSeed")
            {
                if (growthTimer >= timeForSeedToSprout && currentStage == 10)
                {
                    SetStage(11); // Change to sprout stage
                }
                else if (growthTimer >= timeForSproutToYoung && currentStage == 11)
                {
                    SetStage(12); // Change to young stage
                }
                else if (growthTimer >= timeForYoungToMature && currentStage == 12)
                {
                    SetStage(13); // Change to mature stage
                }
                else if (growthTimer >= timeForMatureToHarvest && currentStage == 13)
                {
                    SetStage(14); // Change to harvest stage
                }
            }
            else if (nameOfCrop == "PumpkinSeed")
            {
                if (growthTimer >= timeForSeedToSprout && currentStage == 15)
                {
                    SetStage(16); // Change to sprout stage
                }
                else if (growthTimer >= timeForSproutToYoung && currentStage == 16)
                {
                    SetStage(17); // Change to young stage
                }
                else if (growthTimer >= timeForYoungToMature && currentStage == 17)
                {
                    SetStage(18); // Change to mature stage
                }
                else if (growthTimer >= timeForMatureToHarvest && currentStage == 18)
                {
                    SetStage(19); // Change to harvest stage
                }
            }
        }
    }

    public void Plant(string cropofChoice)
    {
        if (!isPlanted)
        {
            nameOfCrop = cropofChoice;
            isPlanted = true;
            growthTimer = 0f;
            if (nameOfCrop == "CarrotSeed")
            {
                SetStage(0); // Change to carrot seed stage
            }
            else if (nameOfCrop == "YamSeed")
            {
                SetStage(5); // Change to yam seed stage
            }
            else if (nameOfCrop == "TomatoSeed")
            {
                SetStage(10); // Change to tomato seed stage
            }
            else if (nameOfCrop == "PumpkinSeed")
            {
                SetStage(15); // Change to pumpkin seed stage
            }
        }
    }

    public void Harvest()
    {
        if (isHarvestable)
        {
            if (nameOfCrop == "CarrotSeed")
            {
                coinManager.AddCoins(50);
            }
            else if (nameOfCrop == "YamSeed")
            {
                coinManager.AddCoins(100);
            }
            else if (nameOfCrop == "TomatoSeed")
            {
                coinManager.AddCoins(200);
            }
            else if (nameOfCrop == "PumpkinSeed")
            {
                coinManager.AddCoins(300);
            }
            isPlanted = false;
            SetStage(-1); // Change to unplanted state with hidden stage
        }
        
    }

    public bool CanHarvest()
    {
        return isHarvestable && (currentStage == 4 || currentStage == 9 || currentStage == 14 || currentStage == 19);
    }

    private void SetStage(int stage)
    {
        // Deactivate all stages
        hiddenStage.SetActive(false);

        carrotSeedStage.SetActive(false);
        carrotSproutStage.SetActive(false);
        carrotYoungStage.SetActive(false);
        carrotMatureStage.SetActive(false);
        carrotHarvestStage.SetActive(false);

        yamSeedStage.SetActive(false);
        yamSproutStage.SetActive(false);
        yamYoungStage.SetActive(false);
        yamMatureStage.SetActive(false);
        yamHarvestStage.SetActive(false);

        tomatoSeedStage.SetActive(false);
        tomatoSproutStage.SetActive(false);
        tomatoYoungStage.SetActive(false);
        tomatoMatureStage.SetActive(false);
        tomatoHarvestStage.SetActive(false);

        pumpkinSeedStage.SetActive(false);
        pumpkinSproutStage.SetActive(false);
        pumpkinYoungStage.SetActive(false);
        pumpkinMatureStage.SetActive(false);
        pumpkinHarvestStage.SetActive(false);

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
                carrotSeedStage.SetActive(true);
                break;
            case 1:
                carrotSproutStage.SetActive(true);
                break;
            case 2:
                carrotYoungStage.SetActive(true);
                break;
            case 3:
                carrotMatureStage.SetActive(true);
                break;
            case 4:
                carrotHarvestStage.SetActive(true);
                isHarvestable = true;
                break;
            case 5:
                yamSeedStage.SetActive(true);
                break;
            case 6:
                yamSproutStage.SetActive(true);
                break;
            case 7:
                yamYoungStage.SetActive(true);
                break;
            case 8:
                yamMatureStage.SetActive(true);
                break;
            case 9:
                yamHarvestStage.SetActive(true);
                isHarvestable = true;
                break;
            case 10: // Tomato Seed
                tomatoSeedStage.SetActive(true);
                break;
            case 11: // Tomato Sprout
                tomatoSproutStage.SetActive(true);
                break;
            case 12: // Tomato Young
                tomatoYoungStage.SetActive(true);
                break;
            case 13: // Tomato Mature
                tomatoMatureStage.SetActive(true);
                break;
            case 14: // Tomato Harvest
                tomatoHarvestStage.SetActive(true);
                isHarvestable = true;
                break;
            case 15: // Pumpkin Seed
                pumpkinSeedStage.SetActive(true);
                break;
            case 16: // Pumpkin Sprout
                pumpkinSproutStage.SetActive(true);
                break;
            case 17: // Pumpkin Young
                pumpkinYoungStage.SetActive(true);
                break;
            case 18: // Pumpkin Mature
                pumpkinMatureStage.SetActive(true);
                break;
            case 19: // Pumpkin Harvest
                pumpkinHarvestStage.SetActive(true);
                isHarvestable = true;
                break;
        }
    }

    public bool IsCropNotPlanted()
    {
        return isPlanted;
    }
}
