using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteManager1 : MonoBehaviour
{private bool isMuted = false;
    public Sprite mutedIcon;
    public Sprite unmutedIcon;
    private Image buttonImage;

    
    // Start is called before the first frame update
    void Start()
    {
        buttonImage = GetComponent<Image>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleUiSprite()
    {
        isMuted =!isMuted;
        if(isMuted)
        {
            buttonImage.sprite = unmutedIcon;
            
        }

        else
        {
            buttonImage.sprite = mutedIcon;
        }

    }
}
