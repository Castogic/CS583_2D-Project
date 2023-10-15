using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingOrderModifier : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // Get the Y position of the object in world space.
        float yPos = transform.position.y;

        // Assign the Sorting Order based on Y position.
        spriteRenderer.sortingOrder = Mathf.RoundToInt(-yPos * 100);
    }
}
