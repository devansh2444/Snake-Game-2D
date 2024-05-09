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


 public void SetMoveDirectionFromJoystick()
    {
        float angle = Vector2.SignedAngle(Vector2.right, joystickVec);

        // Determine the direction based on the angle of the joystick vector
        if (angle >= -45 && angle < 45)
        {
            // Right
            FindObjectOfType<Snake>().SetMoveDirection(Snake.Direction.Right);
        }
        else if (angle >= 45 && angle < 135)
        {
            // Up
            FindObjectOfType<Snake>().SetMoveDirection(Snake.Direction.Up);
        }
        else if (angle >= 135 || angle < -135)
        {
            // Left
            FindObjectOfType<Snake>().SetMoveDirection(Snake.Direction.Left);
        }
        else if (angle >= -135 && angle < -45)
        {
            // Down
            FindObjectOfType<Snake>().SetMoveDirection(Snake.Direction.Down);
        }
    }
    
}
