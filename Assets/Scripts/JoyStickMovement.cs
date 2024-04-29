using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStickMovement : MonoBehaviour
{

    public GameObject joystick;
    public GameObject joystickBG;
    public Vector2 joystickVec;
    public Vector2 joystickTouchPos;
    public Vector2 joystickOriginalPos;
    private float joystickRadius;
   
    // Start is called before the first frame update
    void Start()
    {
        joystickOriginalPos = joystickBG.transform.position;
        joystickRadius = joystickBG.GetComponent<RectTransform>().sizeDelta.y / 4;

    }

    public void PointerDown ()
    { 
        
        // joystick.transform.position = Input.mousePosition;
        // joystickBG.transform.position = Input.mousePosition;
        joystickTouchPos = Input.mousePosition;  
    }

    public void Drag(BaseEventData baseEventData)
{
    PointerEventData pointerEventData = baseEventData as PointerEventData;
    Vector2 dragPos = pointerEventData.position;
    joystickVec = (dragPos - joystickTouchPos).normalized;

    float joystickDistance = Vector2.Distance(dragPos, joystickTouchPos);

    if (joystickDistance < joystickRadius)
    {
        joystick.transform.position = joystickOriginalPos + joystickVec * joystickDistance;
    }
    else
    {
        Vector2 clampedPosition = joystickOriginalPos + joystickVec * joystickRadius;
        joystick.transform.position = clampedPosition;
    }
}



    public void PointerUp ()
    {
        joystickVec  = Vector2.zero;
        joystick.transform.position = joystickOriginalPos;
        joystickBG.transform.position = joystickOriginalPos;    
    }

    
}
