using UnityEngine;
using TMPro;
using System.Collections;

public class CoinManager : MonoBehaviour
{
    public TextMeshProUGUI coinCountText; // Reference to the TextMeshPro Text element.
    private int coinCount = 0; // Initial coin count.
    private int targetCoinCount = 0; // The target coin count for counting animation.
    private float countingSpeed = 3.0f; // Speed of the counting animation.

    private void Start()
    {
        // Ensure the Text element is assigned in the Inspector.
        if (coinCountText == null)
        {
            Debug.LogError("Text element not assigned in the Inspector.");
            return;
        }

        // Update the Text element with the initial coin count.
        UpdateCoinCountText();
    }

    private void Update()
    {
        // Check for key presses to test incrementing and decrementing the coin count.
        if (Input.GetKeyDown(KeyCode.Equals))
        {
            AddCoins(100);
        }
        else if (Input.GetKeyDown(KeyCode.Minus))
        {
            SubtractCoins(10);
        }
    }

    // Function to add coins to the player's count with counting animation.
    public void AddCoins(int amount)
    {
        targetCoinCount += amount;

        // Start a coroutine to perform the counting animation.
        StartCoroutine(CountCoinsAnimation());
    }

    // Function to subtract coins from the player's count with counting animation.
    public void SubtractCoins(int amount)
    {
        targetCoinCount -= amount;
        if (targetCoinCount < 0)
        {
            targetCoinCount = 0;
        }

        // Start a coroutine to perform the counting animation.
        StartCoroutine(CountCoinsAnimation());
    }

    // Coroutine for the counting animation.
    private IEnumerator CountCoinsAnimation()
    {
        int startCount = coinCount;
        float timer = 0.0f;

        while (timer < 1.0f)
        {
            timer += Time.deltaTime * countingSpeed;

            // Interpolate the coin count for the animation.
            coinCount = Mathf.RoundToInt(Mathf.Lerp(startCount, targetCoinCount, timer));
            UpdateCoinCountText();

            yield return null;
        }

        // Ensure the displayed count matches the final target count.
        coinCount = targetCoinCount;
        UpdateCoinCountText();
    }

    // Function to update the TextMeshPro text element with the current coin count.
    private void UpdateCoinCountText()
    {
        if (coinCountText != null)
        {
            coinCountText.text = coinCount.ToString(); // Update the text with the current coin count.
        }
    }

    // Function to check if the player has enough coins for a purchase.
    public bool HasEnoughCoins(int amount)
    {
        return coinCount >= amount;
    }
}
