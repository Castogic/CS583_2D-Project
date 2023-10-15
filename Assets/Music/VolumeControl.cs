using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeControl : MonoBehaviour
{
    public GameObject mutedObject;
    public GameObject unmutedObject;

    private bool isMuted = false;

    // Start is called before the first frame update
    void Start()
    {
        // Initially, set the appropriate objects active and inactive.
        SetMuteUI();
    }

    // Update is called once per frame
    void Update()
    {
        // Check for the 'M' key press to toggle mute.
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMute();
        }
    }

    void ToggleMute()
    {
        isMuted = !isMuted;

        // Adjust the audio volume of all audio sources in the scene.
        AudioListener.volume = isMuted ? 0 : 1;

        SetMuteUI();
    }

    void SetMuteUI()
    {
        // Set the "Muted" object active if the game is muted, and "Unmuted" otherwise.
        if (isMuted)
        {
            if (mutedObject != null)
                mutedObject.SetActive(true);

            if (unmutedObject != null)
                unmutedObject.SetActive(false);
        }
        else
        {
            if (mutedObject != null)
                mutedObject.SetActive(false);

            if (unmutedObject != null)
                unmutedObject.SetActive(true);
        }
    }
}
