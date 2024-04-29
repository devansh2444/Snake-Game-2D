using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SnakeMovement : MonoBehaviour


{

//     public float moveSpeed;
//     private Rigidbody2D rb;

//     public JoyStickMovement joyStickMovement;
//     // Add a boolean to track whether the game is over.
//     private bool gameOver = false;
    

//     private Transform snakeHead;
//     public  bool powerup2XActive = false;

//     private bool PowerupLifeActive = false;

//     private float powerupDuration = 15f; // Duration of the power-up in seconds
//     private float currentPowerupTime; // Time remaining for the current power-up

//     public TextMeshProUGUI powerUpDurationText;
//     public Sprite gameOverSprite;
//     public Sprite normalHeadSprite;
//     private SpriteRenderer snakeHeadRenderer;
//     public GameObject gameOverCanvas;
//     private List<Transform> _snakeSpawn;
//     public Transform snakePrefab;
//     private Vector2 touchStartPos;

//     private AudioSource audioSource;

//     public AudioClip eatSound; // Assign this in the Unity Editor
//     public AudioClip gameOverSound;
//     public AudioClip powerUpSound;
    
//     private FoodSpawner foodSpawner;

//     private Vector3 moveDirection;
//     private AudioSource proximityAudioSource;
//     public AudioClip proximitySound;  // Assign this in the Unity Editor
//     public float proximityDistanceThreshold = 0.5f;  // Adjust the distance threshold as needed

//     // Add a variable to control the blinking effect
//     private bool isBlinking = false;

//     // Add a variable to control the blinking speed
//     public float blinkSpeed = 0.2f;
//     private int width;
//     private int height;

//     public SnakeMovement(int width, int height) {
//         this.width = width;
//         this.height = height;
//     }

     

    


//     // Start is called before the first frame update
//     void Start()
//     {
//         rb = GetComponent<Rigidbody2D>();
//         snakeHead = transform;
//         snakeHeadRenderer = GetComponent<SpriteRenderer>();
//         snakeHeadRenderer.sprite = normalHeadSprite;
//         //rb.velocity = new Vector2(moveSpeed,0);
//         audioSource = GetComponent<AudioSource>();

//         foodSpawner = FindObjectOfType<FoodSpawner>();
        
//         _snakeSpawn = new List<Transform>();
//         _snakeSpawn.Add(this.transform);


//         proximityAudioSource = gameObject.AddComponent<AudioSource>();
//         proximityAudioSource.clip = proximitySound;
//         proximityAudioSource.volume = 0.3f;  // Adjust the volume as needed
//         proximityAudioSource.loop = false;
//         // powerUpDurationText.text = "PowerUp: ";

        
        
        
//     }

//     // Update is called once per frame
//     void Update()
//     {

//         // Check if the game is over before processing input
//         if (!gameOver)
//         {
            
//             handleInput();
//             handlePowerups();
//             CheckProximityWithMonsters();

//         }
//     }

   

//     private void FixedUpdate() 
//     {
       
//         // Update the snake's position only if the game is not over
//         if (!gameOver)
//         {
//             MoveSnake();
            
//         } 


//     }

//     private void handleInput() 
//     {
//         //For Desktop Game
//             if(Input.GetKeyDown(KeyCode.DownArrow))
//             {
//                 moveDirection = Vector2.down;
//             }
//             if(Input.GetKeyDown(KeyCode.UpArrow))
//             {
//                 moveDirection = Vector2.up;
//             }
//             if(Input.GetKeyDown(KeyCode.LeftArrow))
//             {
//                 moveDirection = Vector2.left;
//             }
//             if(Input.GetKeyDown(KeyCode.RightArrow))
//             {
//                 moveDirection = Vector2.right;
                
//             }
//             HandleMobileInput();   
//     }
 

//     private void HandleMobileInput () 
//     {
//          // For Mobile Game

//             // Handle touch input (For moving snake using swiping the screen)
//             if (Input.touchCount > 0)
//             {
//                 Touch touch = Input.GetTouch(0); // Assuming only one touch for simplicity

//                 if (touch.phase == TouchPhase.Began)
//                 {
//                     touchStartPos = touch.position;
//                 }
//                 else if (touch.phase == TouchPhase.Moved)
//                 {
//                     Vector2 touchDelta = touch.position - touchStartPos;

//                     // Check if the touch movement is horizontal or vertical
//                     if (Mathf.Abs(touchDelta.x) > Mathf.Abs(touchDelta.y))
//                     {
//                         // Horizontal movement
//                         moveDirection = (touchDelta.x > 0) ? Vector2.right : Vector2.left;
//                     }
//                     else
//                     {
//                         // Vertical movement
//                         moveDirection = (touchDelta.y > 0) ? Vector2.up : Vector2.down;
//                     }
//                 }
//             }    
//     }



// void MoveSnake()
// {
    

//     Vector3 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

//     // Check if the snake's head goes beyond the screen bounds
//     if (snakeHead.position.x > screenBounds.x)
//     {
//         // Wrap around from right to left
//         WrapAround( Vector3.left * screenBounds.x * 2f);
//     }
//     else if (snakeHead.position.x < -screenBounds.x)
//     {
//         // Wrap around from left to right
//         WrapAround(Vector3.right * screenBounds.x * 2f);
//     }
//     else if (snakeHead.position.y > screenBounds.y)
//     {
//         // Wrap around from top to bottom
//         WrapAround(Vector3.down * screenBounds.y * 2f);
//     }
//     else if (snakeHead.position.y < -screenBounds.y)
//     {
//         // Wrap around from bottom to top
//         WrapAround(Vector3.up * screenBounds.y * 2f);
//     }

//     // Move the snake as before
//     rb.velocity = moveDirection * moveSpeed;

//     for (int i = _snakeSpawn.Count - 1; i > 0; i--)
//     {
//         Vector3 targetPosition = _snakeSpawn[i - 1].position - (moveDirection * 0.03f);
//         _snakeSpawn[i].position = Vector3.MoveTowards(_snakeSpawn[i].position, targetPosition, moveSpeed * Time.fixedDeltaTime);
//     }
// }


   
// void WrapAround(Vector3 offset)
// {
//     // Move the entire snake including head and body segments
//     snakeHead.position += offset;

//     // Loop through all snake body segments and move them accordingly
//     for (int i = 1; i < _snakeSpawn.Count; i++)
//     {
//         _snakeSpawn[i].position += offset;
//     }
// }
//     void handlePowerups()
//     {
//         if (powerup2XActive || PowerupLifeActive)
//             {
                
//                 currentPowerupTime -= Time.deltaTime;
                

//                 powerUpDurationText.text = "Power-up:" + Mathf.Ceil(currentPowerupTime) + "s";
//                 powerUpDurationText.enabled = true;
//                 //Debug.Log(powerUpDurationText.text);

//             if ( !isBlinking && currentPowerupTime > 0)
//             {
//                 //StartCoroutine(BlinkSnake());
//             }
//                 if (currentPowerupTime <= 0)
//                 {
//                     // Power-up duration has ended
//                     DeactivatePowerup2X();
//                     // PowerupLifeActive = false;
//                     DeactivatePowerupLife();
//                     //Debug.Log("PowerUpDeactivated");
//                     // Stop the blinking coroutine and reset the snake's sprites
//                     //StopCoroutine(BlinkSnake());
//                     //ResetSnakeSprites();
//                     powerUpDurationText.enabled = false;
//                 }
//             }
//                 else
//                 {
//                     powerUpDurationText.enabled = false; // Hide the text when no power-up is active
//                 }
//     }


// // Coroutine for blinking effect on the head and body
// IEnumerator BlinkSnake()
// {
//     isBlinking = true;

//     while (currentPowerupTime > 0)
//     {
//         // Toggle the visibility of the snake's head sprite
//         snakeHeadRenderer.enabled = !snakeHeadRenderer.enabled;

//         // Toggle the visibility of all body segments' sprites
//         foreach (Transform segment in _snakeSpawn)
//         {
//             segment.GetComponent<SpriteRenderer>().enabled = !segment.GetComponent<SpriteRenderer>().enabled;
//         }

//         // Wait for the specified blink speed
//         yield return new WaitForSeconds(blinkSpeed);
//     }

//     // Ensure the head and body sprites are visible when the blinking stops
//     snakeHeadRenderer.enabled = true;
//     ResetSnakeSprites();
//     isBlinking = false;
// }
// // Method to reset all snake sprites to normal
// void ResetSnakeSprites()
// {
//     snakeHeadRenderer.sprite = normalHeadSprite;

//     foreach (Transform segment in _snakeSpawn)
//     {
//         segment.GetComponent<SpriteRenderer>().enabled = true;
//     }
// }
// void CheckProximityWithMonsters()
// {
//     GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

//     foreach (GameObject monster in monsters)
//     {
//         if (monster != null)
//         {
//             float distance = Vector2.Distance(transform.position, monster.transform.position);

//             if (distance < proximityDistanceThreshold && !proximityAudioSource.isPlaying)
//             {
//                 proximityAudioSource.Play();
//             }
//         }
//     }
// }



//     private void grow()
// {
//     Transform snakeSpawn = Instantiate(this.snakePrefab);

//     // Calculate target position based on last segment and movement direction
//     Vector3 targetPosition = _snakeSpawn[_snakeSpawn.Count - 1].position;
//     if (_snakeSpawn.Count > 1)
//     {
//         Vector2 lastMoveDirection = (_snakeSpawn[_snakeSpawn.Count - 1].position - _snakeSpawn[_snakeSpawn.Count - 2].position).normalized;
//         targetPosition += (Vector3)lastMoveDirection * 0.1f;
//     }

//     snakeSpawn.position = targetPosition;
//     _snakeSpawn.Add(snakeSpawn);

//     if (isBlinking)
//     {
//         snakeSpawn.GetComponent<SpriteRenderer>().enabled = !snakeSpawn.GetComponent<SpriteRenderer>().enabled;
//     }
// }






//     // For Moving Snake Using Buttons 
//     public void MoveLeft()
//     {    
//         if(!gameOver)
//         {
//              rb.velocity = new Vector2(-moveSpeed, 0);
//         }
       
//     }

//     public void MoveRight()
//     {
//         if(!gameOver)
//         {
//         rb.velocity = new Vector2(moveSpeed, 0);
//         }
//     }

//     public void MoveUp()
//     {
//         if(!gameOver)
//         {
//         rb.velocity = new Vector2(0, moveSpeed);
//         }
//     }

//     public void MoveDown()
//     {
//         if(!gameOver)
//         {
//         rb.velocity = new Vector2(0, -moveSpeed);
//         }
//     }

//     public void StopMovement()
//     {
//         if(!gameOver)
//         {
//         rb.velocity = Vector2.zero;
//         }
//     }
    

    
//     private void OnTriggerEnter2D(Collider2D collision) 
//     {
//         if(collision.gameObject.tag == "Food")
//         {
//             audioSource.PlayOneShot(eatSound);
//             grow();
//         }
//          else if(collision.gameObject.tag == "PowerUp")
//         {
//             //Debug.Log("PowerUp2XActive");
//             audioSource.PlayOneShot(powerUpSound);
//             FindObjectOfType<PowerupManager>().DestroyPowerup();
            
//             ActivatePowerup2X();
//             //powerUpCollisionDetected = true;

//         }
//          else if(collision.gameObject.tag == "PowerUpLife")
//         {
//             // PowerupLifeActive = true;
//             //Debug.Log("PowerUpLifeActive");
//             // audioSource.PlayOneShot(powerUpSound);
//             FindObjectOfType<PowerupManager>().DestroyPowerup();
            
//             ActivatePowerUpLife();

//         }
//         // else if(collision.gameObject.tag == "SnakeBody")
//         // {
//         //     GameOver();
//         // }   
//     }
//     private void OnCollisionEnter2D(Collision2D collision) {
//         if(!gameOver)
//         {
//             //(collision.gameObject.tag == "Wall" && PowerupLifeActive == false) || 
            
//             if((collision.gameObject.tag == "Monster" && PowerupLifeActive == false))
//             {
//                 // snakeHeadRenderer.sprite = gameOverSprite;
//                 GameOver();
            
//             }
       
//         }
//     }

//     void GameOver()
//     {
//          // Set the game over flag.
//             gameOver = true;

//             // audioSource.PlayOneShot(gameOverSound);

//             // Display the game over UI (assuming you have a Canvas variable named gameOverCanvas).
//             gameOverCanvas.SetActive(true);

//             // Optionally, you may want to stop the snake movement or handle other game over logic here.
             
//             rb.velocity = Vector2.zero; // Stop the snake movement, if needed.

            
//             FindObjectOfType<MonsterMovement>().StopMonsterMovements();

//             FindObjectOfType<PowerupManager>().StopPowerupSpawning();
            
//     }

//     private void ActivatePowerup2X()
//     {
//         powerup2XActive = true;
//         currentPowerupTime = powerupDuration;
//     }

//     private void ActivatePowerUpLife()
//     {
//         PowerupLifeActive = true;
//         currentPowerupTime = powerupDuration;
//         GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
//          // Loop through each Monster and modify the Collider2D components
//         foreach (GameObject monster in monsters)
//         {
//             Collider2D monsterCollider = monster.GetComponent<Collider2D>();
//             if (monsterCollider != null)
//             {
//                 monsterCollider.isTrigger = true;
//             }
//         }

        
//     }

//     private void DeactivatePowerup2X()
//     {
//         powerup2XActive = false;
//     }

//     private void DeactivatePowerupLife () 
//     {
//         PowerupLifeActive = false;
//         GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster"); 
//         foreach (GameObject monster in monsters)
//         {
//             Collider2D monsterCollider = monster.GetComponent<Collider2D>();
//             if (monsterCollider != null)
//             {
//                 monsterCollider.isTrigger = false;
//             }
//         }
        
//     }
}
