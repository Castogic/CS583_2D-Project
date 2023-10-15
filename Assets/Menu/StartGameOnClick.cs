using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartGameOnClick : MonoBehaviour
{
    public string HarmoniaWorld; // Name of your main game scene.

    private Button playButton;

    private void Start()
    {
        // Get a reference to the Button component attached to this GameObject.
        playButton = GetComponent<Button>();

        // Add a click event listener to the button.
        playButton.onClick.AddListener(StartGame);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(HarmoniaWorld);
    }
}
