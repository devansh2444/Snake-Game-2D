using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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
   
    
    // Start is called before the first frame update
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
        
        SceneManager.LoadScene("MainScene");
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
        // Check if an AudioClip is assigned to the AudioSource
        if (audioSource.clip != null)
        {
        // Play the click sound
        audioSource.PlayOneShot(clickAudioClip);
        }
    }
}
