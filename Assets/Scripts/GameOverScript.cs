using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour

{

    private AudioSource audioSource;
    public GameObject gameOverText;
    public GameObject warningText;
    public GameObject restart;
    public GameObject quit;
    public GameObject yes;
    public GameObject no;
    public GameObject warningPanel;
    // Start is called before the first frame update
    void Start()
    {
        //audioSource.Play();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Restart()
    {
        PlayClickSound();
        SceneManager.LoadScene("MainScene");
    }

    public void CheckForQuit()
    {
        PlayClickSound();
        // warningText.gameObject.SetActive(true);
        // yes.gameObject.SetActive(true);
        // no.gameObject.SetActive(true);
        warningPanel.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(false);
        restart.gameObject.SetActive(false);
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
        // warningText.gameObject.SetActive(false);
        // yes.gameObject.SetActive(false);
        // no.gameObject.SetActive(false);
        warningPanel.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(true);
        restart.gameObject.SetActive(true);
        quit.gameObject.SetActive(true);
    }

    private void PlayClickSound()
    {
        if(audioSource.clip != null)
        {
            audioSource.PlayOneShot(audioSource.clip);
        }
    }

}
