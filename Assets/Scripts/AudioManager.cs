using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    

    private bool isMuted = false;
   
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
      
        audioSource = GetComponent<AudioSource>(); 
        bool isMuted = PlayerPrefs.GetInt("IsMuted", 0) == 1;
        SetAudioState(isMuted);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // Function to set the audio state (muted or unmuted)
    private void SetAudioState(bool isMuted)
    {
        if (isMuted)
        {
            // Mute audio
            AudioListener.volume = 0;
        }
        else
        {
            // Unmute audio
            AudioListener.volume = 1;
        }
    }
    // Function to toggle the audio state (muted or unmuted)
    public void ToggleMute()
    {
        bool isMuted = AudioListener.volume == 0; // Check if audio is currently muted

        // Toggle the mute state
        isMuted = !isMuted;

        // Set the audio state and save it to PlayerPrefs
        SetAudioState(isMuted);
        PlayerPrefs.SetInt("IsMuted", isMuted ? 1 : 0);
    }
    //  public void ToggleMute()
    // {
    //     isMuted = !isMuted;

    //     if (isMuted)
    //     {
    //         // Mute audio
    //         //audioSource.mute = true;
    //         AudioListener.volume = 0;
           
    //     }
    //     else
    //     {
    //         // Unmute audio
    //         //audioSource.mute = false;
    //         AudioListener.volume = 1;
            
    //     }
    // }
}
