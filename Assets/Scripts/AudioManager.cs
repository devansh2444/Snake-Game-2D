using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    

    private bool isMuted = false;
    public Button muteButton;
    public Button unMuteButton;
   
   
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
      
        audioSource = GetComponent<AudioSource>(); 
        LoadAudioState();
        UpdateAudioState();
    }

    public void ToggleMute()
    {
        isMuted = !isMuted;
        UpdateAudioState();
        SaveAudioState();
    }

    // public void Mute()
    // {
    //     isMuted = true;
    //     UpdateAudioState();
    // }
    // public void UnMute()
    // {
    //     isMuted = false;
    //     UpdateAudioState();
    // }
    // Function to set the audio state (muted or unmuted)
    private void UpdateAudioState()
    {
        AudioListener.volume = isMuted ? 0 : 1;

        if(muteButton != null && unMuteButton != null)
        {
            muteButton.interactable = !isMuted;
            unMuteButton.interactable = isMuted;
        }
    }

     private void LoadAudioState()
    {
        isMuted = PlayerPrefs.GetInt("IsMuted", 0) == 1;
    }

    private void SaveAudioState()
    {
        PlayerPrefs.SetInt("IsMuted", isMuted ? 1 : 0);
        PlayerPrefs.Save();
    }
    
}
    
