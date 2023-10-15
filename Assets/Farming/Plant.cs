using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Note: This version is deprecated and no longer in use. Only use for reference.
public class Plant : MonoBehaviour
{
    public enum GrowthStage
    {
        Seed,
        Stage1,
        Stage2,
        Stage3,
        FullyGrown
    }

    public GrowthStage currentStage;
    public float timeUntilNextGrowth;

    public GameObject seedPrefab;
    public GameObject stage1Prefab;
    public GameObject stage2Prefab;
    public GameObject stage3Prefab;
    public GameObject fullyGrownPrefab;

    private void Start()
    {
        InitializeGrowth();
    }

    private void Update()
    {
        timeUntilNextGrowth -= Time.deltaTime;
        if (timeUntilNextGrowth <= 0f)
        {
            Grow();
            timeUntilNextGrowth = CalculateTimeForNextStage(currentStage);
        }
    }

    public void InitializeGrowth()
    {
        SetPlantStage(GrowthStage.Seed);
        timeUntilNextGrowth = CalculateTimeForNextStage(currentStage);
    }

    private float CalculateTimeForNextStage(GrowthStage nextStage)
    {
        switch (nextStage)
        {
            case GrowthStage.Seed:
                return 0f;
            case GrowthStage.Stage1:
                return 10f;
            case GrowthStage.Stage2:
                return 15f;
            case GrowthStage.Stage3:
                return 20f;
            case GrowthStage.FullyGrown:
                return 0f;
            default:
                return 0f;
        }
    }

    public void SetPlantStage(GrowthStage newStage)
    {
        currentStage = newStage;

        // Activate the appropriate prefab based on the new stage.
        seedPrefab.SetActive(currentStage == GrowthStage.Seed);
        stage1Prefab.SetActive(currentStage == GrowthStage.Stage1);
        stage2Prefab.SetActive(currentStage == GrowthStage.Stage2);
        stage3Prefab.SetActive(currentStage == GrowthStage.Stage3);
        fullyGrownPrefab.SetActive(currentStage == GrowthStage.FullyGrown);
    }

    public void Grow()
    {
        // TODO:
        // Implement logic to advance the growth stage.
        // Call SetPlantStage to change the stage when it's time to grow.
    }

    public void Harvest()
    {
        // TODO:
        // Implement logic here for harvesting.
        // Disable the crop object or remove it from the scene.
    }
}