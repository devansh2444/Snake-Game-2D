using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class StartGame : MonoBehaviour
{
    public GameObject logo;
    public GameObject play;
    public GameObject quit;
    public GameObject warningText;
    public GameObject yes;
    public GameObject no;
     public Animator animator;
    public string animationTriggerName;
    private AudioSource audioSource;
    public AudioClip clickAudioClip;
    public GameObject difficultyCanvas;
   
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
    
    }

    public void PlayButton () {
        PlayClickSound();
        difficultyCanvas.gameObject.SetActive(true);
        // SceneManager.LoadScene("MainScene");
    }

    public void CloseDifficultyPanel () {
        PlayClickSound();
        difficultyCanvas.gameObject.SetActive(false);
    }

    public void CheckForQuitButton()
    {
        PlayClickSound();
        warningText.gameObject.SetActive(true);
        yes.gameObject.SetActive(true);
        no.gameObject.SetActive(true);
        logo.gameObject.SetActive(false);
        play.gameObject.SetActive(false);
        quit.gameObject.SetActive(false);
        
    }

     public void YesToQuit()
    {
        PlayClickSound();
        Application.Quit();
    }

    public void NoToQuit()
    {
        PlayClickSound();
        warningText.gameObject.SetActive(false);
        yes.gameObject.SetActive(false);
        no.gameObject.SetActive(false);
        logo.gameObject.SetActive(true);
        play.gameObject.SetActive(true);
        quit.gameObject.SetActive(true);
    }

    public void TriggerAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger(animationTriggerName);
        }
    }
    
    private void PlayClickSound()
    {
        if (audioSource.clip != null)
        {
        audioSource.PlayOneShot(clickAudioClip);
        }
    }
}
