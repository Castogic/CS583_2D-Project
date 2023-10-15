using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmingController : MonoBehaviour
{
    public GameObject carrotPrefab;
    public GameObject tomatoPrefab;
    public GameObject pumpkinPrefab;
    public GameObject yamPrefab;

    private GameObject selectedCropPrefab;

    private void Start()
    {
        // Initially, no crop selected
        selectedCropPrefab = null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedCropPrefab = carrotPrefab;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedCropPrefab = tomatoPrefab;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedCropPrefab = pumpkinPrefab;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            selectedCropPrefab = yamPrefab;
        }

        if (selectedCropPrefab != null && Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Instantiate(selectedCropPrefab, mousePos, Quaternion.identity);
        }
    }
}
