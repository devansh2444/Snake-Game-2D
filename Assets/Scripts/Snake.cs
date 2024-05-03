/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using TMPro;
using CodeMonkey.Utils;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Snake : MonoBehaviour {

    public enum Direction {
        Left,
        Right,
        Up,
        Down
    }

    private enum State { 
        Alive,
        Dead
    }
        public  bool powerup2XActive = false;

    private bool PowerupLifeActive = false;
    private Animator animator;
    public float powerupDuration = 15f; // Duration of the power-up in seconds
    public float currentPowerupTime; // Time remaining for the current power-up
  public TextMeshProUGUI powerUpDurationText;
    public GameObject gameOverCanvas;
    public GameObject gmaeAssets;
    public GameObject monsters;
    public GameObject particleEttcts;
    public AudioClip gameOverSound;
    public AudioClip powerUpSound;
    public AudioClip eatSound;
    private AudioSource audioSource;
    private State state;
    public Direction gridMoveDirection;
    public  Vector3 gridPosition;
    private float gridMoveTimer;
    private float gridMoveTimerMax;
    private LevelGrid levelGrid;
    private float speed;
    private int snakeBodySize;
    private List<SnakeMovePosition> snakeMovePositionList;
    private List<SnakeBodyPart> snakeBodyPartList;
    //public BoxCollider2D foodSpawn;
    public float score;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    private Vector2 touchStartPos;
    public Button pauseButton;
    private bool foodBeingConsumed = false;

    public GameObject leftButton;
    public GameObject rightButton;
    public GameObject upButton;
    public GameObject downButton;

    public GameObject joyStick;
   
    
    public GameObject Walls;
    private bool isMode;
    private bool isModeJoyStick;
    private bool isSwipeMode;
    public static Snake Instance;
    
    
    public void Setup(LevelGrid levelGrid) {
        this.levelGrid = levelGrid;
        Debug.Log(levelGrid);
        Debug.Log(levelGrid);
        
    }
     private bool intToBool(int intValue)
     {
        return intValue == 1 ? true : false;
     }
    private void Start() {
         // Subscribe to the event for control settings changes
        SettingManager.OnControlSettingsChanged += UpdateControlSettings;
         LoadControlSettings();
        speed = 0.9f;
        audioSource = GameObject.Find("Snake").GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        bool isMode = intToBool(PlayerPrefs.GetInt("isMode"));
        bool isModeJoyStick = intToBool(PlayerPrefs.GetInt("isModeJoyStick"));
        bool isLevel = intToBool(PlayerPrefs.GetInt("isLevel"));

        // if(isLevel == true)
        // {
        //      Walls.gameObject.SetActive(false);
        // }
        // else
        // {
        //     Walls.gameObject.SetActive(true);
        // }
        //  if(isMode == true){
        //     ActivateControlButtons();
        // }
        // else{
        //     DeactivateControlButtons();

        // }
        // if(isModeJoyStick == true){
        //     ActivateJoyStick();
        // }
        // else{
        //     DeActivateJoyStick();
        // }
        UpdateHighScoreUI();

        
    }
    // Method to update control settings
    private void UpdateControlSettings(bool isMode, bool isModeJoyStick, bool isSwipeMode)
    {
        this.isMode = isMode;
        this.isModeJoyStick = isModeJoyStick;
        this.isSwipeMode = isSwipeMode;

        // Update control mechanism based on settings
        if (isMode)
        {
            ActivateControlButtons();
            DeActivateJoyStick();
        }
        else if (isModeJoyStick)
        {
            ActivateJoyStick();
            DeactivateControlButtons();
        }
        else
        {
            // Handle other control mechanisms`
            DeActivateJoyStick();
            DeactivateControlButtons();
        }
    }
    private void LoadControlSettings() 
    {
    bool isMode = PlayerPrefs.GetInt("IsMode", 0) == 1;
    bool isModeJoyStick = PlayerPrefs.GetInt("IsModeJoyStick", 0) == 1;
    bool isSwipeMode = PlayerPrefs.GetInt("IsSwipeMode",0) == 1;
    UpdateControlSettings(isMode, isModeJoyStick, isSwipeMode);
    }
    private void ActivateJoyStick () 
    {
        // Check if the GameObject reference is null before accessing it
    if (joyStick != null)
    {
        // Enable joystick
        joyStick.SetActive(true);
    }  
    }
    private void DeActivateJoyStick () 
    {
        // Check if the GameObject reference is null before accessing it
    if (joyStick != null)
    {
        // Disable joystick
        joyStick.SetActive(false);
    }   
    }
    // Method to activate control buttons
    private void ActivateControlButtons()
    {
         // Check if the GameObject references are null before accessing them
    if (leftButton != null && rightButton != null && upButton != null && downButton != null)
    {
        // Enable control buttons
        leftButton.SetActive(true);
        rightButton.SetActive(true);
        upButton.SetActive(true);
        downButton.SetActive(true);
    }
    }

    // Method to deactivate control buttons
    private void DeactivateControlButtons()
    {
        // Check if the GameObject references are null before accessing them
    if (leftButton != null && rightButton != null && upButton != null && downButton != null)
    {
        // Disable control buttons
        leftButton.SetActive(false);
        rightButton.SetActive(false);
        upButton.SetActive(false);
        downButton.SetActive(false);
    }
    }
    private void SetMoveDirection(Direction direction)
    {
        gridMoveDirection = direction;
    }
    private void Awake() {
        gridPosition = new Vector3(0, 0);
        gridMoveTimerMax = .1f;
        gridMoveTimer = gridMoveTimerMax;
        gridMoveDirection = Direction.Right;

        snakeMovePositionList = new List<SnakeMovePosition>();
        snakeBodySize = 0;

        snakeBodyPartList = new List<SnakeBodyPart>();

        state = State.Alive;
    }

    private void Update() {
        if(score >= 25){
            
            speed = 1f;
        }
        if(score >= 75){
            speed = 1.2f;  
        }
        if(score >= 250){
            speed = 1.4f;
        }
        if(score >= 500)
        {
            speed = 1.5f;
        }
        switch (state) {
        case State.Alive:
            HandleInput();
            HandleMobileInput();
            HandleGridMovement();
            handlePowerups();
           
            break;
        case State.Dead:
            break;
        }
    }

    private void HandleInput() {
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            if (gridMoveDirection != Direction.Down) {
                gridMoveDirection = Direction.Up;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            if (gridMoveDirection != Direction.Up) {
                gridMoveDirection = Direction.Down;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            if (gridMoveDirection != Direction.Right) {
                gridMoveDirection = Direction.Left;
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            if (gridMoveDirection != Direction.Left) {
                gridMoveDirection = Direction.Right;
            }
         }

    }
        

    private void HandleMobileInput () 
{
    // Handle touch input (For moving snake using swiping the screen)
    if (Input.touchCount > 0)
    {
        Touch touch = Input.GetTouch(0); // Assuming only one touch for simplicity

        if (touch.phase == TouchPhase.Began)
        {
            touchStartPos = touch.position;
        }
        else if (touch.phase == TouchPhase.Moved)
        {
            Vector2 touchDelta = touch.position - touchStartPos;

            // Check if the touch movement is horizontal or vertical
            if (Mathf.Abs(touchDelta.x) > Mathf.Abs(touchDelta.y))
            {
                // Horizontal movement
                if (touchDelta.x > 0 && gridMoveDirection != Direction.Left)
                {
                    gridMoveDirection = Direction.Right;
                }
                else if (touchDelta.x < 0 && gridMoveDirection != Direction.Right)
                {
                    gridMoveDirection = Direction.Left;
                }
            }
            else
            {
                // Vertical movement
                if (touchDelta.y > 0 && gridMoveDirection != Direction.Down)
                {
                    gridMoveDirection = Direction.Up;
                }
                else if (touchDelta.y < 0 && gridMoveDirection != Direction.Up)
                {
                    gridMoveDirection = Direction.Down;
                }
            }
        }
    }    
}

    

    public void ButtonLeftMovement()
        {
            SetMoveDirection(Direction.Left);
        }

    public void ButtonRightMovement()
        {
            SetMoveDirection(Direction.Right);
        }
    public void ButtonUpMovement()
        {
            SetMoveDirection(Direction.Up);
        }   
    public void ButtonDownMovement()
        {
            SetMoveDirection(Direction.Down);
        }


    private void HandleGridMovement() {
        gridMoveTimer += Time.deltaTime * speed;
        
        if (gridMoveTimer >= gridMoveTimerMax) {
            gridMoveTimer -= gridMoveTimerMax;

            SnakeMovePosition previousSnakeMovePosition = null;
            if (snakeMovePositionList.Count > 0) {
                previousSnakeMovePosition = snakeMovePositionList[0];
            }

            SnakeMovePosition snakeMovePosition = new SnakeMovePosition(previousSnakeMovePosition, gridPosition, gridMoveDirection);
            snakeMovePositionList.Insert(0, snakeMovePosition);

            Vector3 gridMoveDirectionVector;
            switch (gridMoveDirection) {
            default:
            case Direction.Right:   gridMoveDirectionVector = new Vector3(+1, 0); break;
            case Direction.Left:    gridMoveDirectionVector = new Vector3(-1, 0); break;
            case Direction.Up:      gridMoveDirectionVector = new Vector3(0, +1); break;
            case Direction.Down:    gridMoveDirectionVector = new Vector3(0, -1); break;
            }

            gridPosition += gridMoveDirectionVector;

            gridPosition = levelGrid.ValidateGridPosition(gridPosition);
           
            bool snakeAteFood = levelGrid.TrySnakeEatFood(gridPosition);
            if (snakeAteFood) {
                // Snake ate food, grow body
                audioSource.PlayOneShot(eatSound);
                snakeBodySize++;
                CreateSnakeBodyPart();
                UpdateScoreUI();
            }

            if (snakeMovePositionList.Count >= snakeBodySize + 1) {
                snakeMovePositionList.RemoveAt(snakeMovePositionList.Count - 1);
            }

            UpdateSnakeBodyParts();

            foreach (SnakeBodyPart snakeBodyPart in snakeBodyPartList) {
                Vector3 snakeBodyPartGridPosition = snakeBodyPart.GetGridPosition();
                if (gridPosition == snakeBodyPartGridPosition) {
                    if(PowerupLifeActive == false)
                    {
                    // Game Over!
                    
                    GameOver();
                    
                    
                    }
                }
            }

            transform.position = new Vector3(gridPosition.x, gridPosition.y);
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirectionVector) - 90);
        }
    }

    
    private void CreateSnakeBodyPart() {
        snakeBodyPartList.Add(new SnakeBodyPart(snakeBodyPartList.Count));
    }

    private void UpdateSnakeBodyParts() {
        for (int i = 0; i < snakeBodyPartList.Count; i++) {
            snakeBodyPartList[i].SetSnakeMovePosition(snakeMovePositionList[i]);
        }
    }


    private float GetAngleFromVector(Vector3 dir) {
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }

    public Vector3 GetGridPosition() {
        return gridPosition;
    }
    public Direction GetDirection()
    {
        return gridMoveDirection;
    }

    // Return the full list of positions occupied by the snake: Head + Body
    public List<Vector3> GetFullSnakeGridPositionList() {
        List<Vector3> gridPositionList = new List<Vector3>() { gridPosition };
        foreach (SnakeMovePosition snakeMovePosition in snakeMovePositionList) {
            gridPositionList.Add(snakeMovePosition.GetGridPosition());
        }
        return gridPositionList;
    }
    void handlePowerups()
    {
        if (powerup2XActive || PowerupLifeActive)
            {
                
                currentPowerupTime -= Time.deltaTime;
                

                powerUpDurationText.text = "Power-up:" + Mathf.Ceil(currentPowerupTime) + "s";
                powerUpDurationText.enabled = true;
                //Debug.Log(powerUpDurationText.text);

            if ( currentPowerupTime > 0)
            {
                //StartCoroutine(BlinkSnake());
            }
                if (currentPowerupTime <= 0)
                {
                    // Power-up duration has ended
                    DeactivatePowerup2X();
                    // PowerupLifeActive = false;
                    DeactivatePowerupLife();
                    //Debug.Log("PowerUpDeactivated");
                    // Stop the blinking coroutine and reset the snake's sprites
                    //StopCoroutine(BlinkSnake());
                    //ResetSnakeSprites();
                    powerUpDurationText.enabled = false;
                }
            }
                else
                {
                    powerUpDurationText.enabled = false; // Hide the text when no power-up is active
                }
    }

    private void OnTriggerEnter2D(Collider2D collision)
{
    Debug.Log("Trigger Entered");

    if (collision.gameObject.tag == "Food" && !foodBeingConsumed)
    {
        

        // Update score UI
        
        
        UpdateScoreUI();

        

        // // Update and save high score if necessary
        // if (score > PlayerPrefs.GetFloat("HighScore", 0))
        // {
        //     PlayerPrefs.SetFloat("HighScore", score);
        //     UpdateHighScoreUI();
        // }

        // Other existing food collision logic...

        // Exit the function to avoid executing power-up collision logic for food collisions
        return;
    }

    if (collision.gameObject.tag == "PowerUp")
    {
        Debug.Log("2xpoerupactive");
        audioSource.PlayOneShot(powerUpSound);
        FindObjectOfType<PowerupManager>().DestroyPowerup();

        ActivatePowerup2X();
    }
    else if (collision.gameObject.tag == "PowerUpLife")
    {
        Debug.Log("powerUpLifeActive");
        
        audioSource.PlayOneShot(powerUpSound);
        FindObjectOfType<PowerupManager>().DestroyPowerup();

        ActivatePowerUpLife();
    }
}

private void OnCollisionEnter2D(Collision2D collision) {
    if(collision.gameObject.tag == "Monster" && PowerupLifeActive == false) 
    {
         
        GameOver();    
    }
    if(collision.gameObject.tag == "Wall" && PowerupLifeActive == false) 
    {
        
         
        GameOver();
        
    }
    
}

public void GameOver()
{
    //  audioSource.PlayOneShot(gameOverSound);
     particleEttcts.gameObject.SetActive(true);
//     pauseButton.gameObject.SetActive(false);
//     gmaeAssets.gameObject.SetActive(false);
//     monsters.gameObject.SetActive(false);
   
//     gameOverCanvas.SetActive(true);
// //    FindObjectOfType<MonsterMovement>().StopMonsterMovements();
//     FindObjectOfType<PowerupManager>().StopPowerupSpawning();
    state = State.Dead;
    SceneManager.LoadScene("GameOverScene");
   
}

public void UpdateScoreUI()
{
    if (powerup2XActive)
        {
            score += 10;
            scoreText.text = "Score: " + score;
        }
        else
        {
            score += 5;
            scoreText.text = "Score: " + score;
        }
         if (score > PlayerPrefs.GetFloat("HighScore", 0))
    {
        PlayerPrefs.SetFloat("HighScore", score);
        UpdateHighScoreUI(); // Call the method to update high score UI
    }

    
}
    public void UpdateHighScoreUI()
    {
        highScoreText.text = "High Score: " + PlayerPrefs.GetFloat("HighScore", 0);
    }

    // }
    public void ActivatePowerup2X()
    {
        powerup2XActive = true;
        currentPowerupTime = powerupDuration;
    }

    public void ActivatePowerUpLife()
    {
        PowerupLifeActive = true;
        // animator.SetActive(true);
        //animator.SetBool("IsFading", true);
        currentPowerupTime = powerupDuration;
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
         // Loop through each Monster and modify the Collider2D components
        foreach (GameObject monster in monsters)
        {
            Collider2D monsterCollider = monster.GetComponent<Collider2D>();
            if (monsterCollider != null)
            {
                monsterCollider.isTrigger = true;
            }
        }
        

        
    }

    private void DeactivatePowerup2X()
    {
        powerup2XActive = false;
    }

    private void DeactivatePowerupLife () 
    {
        PowerupLifeActive = false;
        //animator.SetActive(false);
        //animator.SetBool("IsFading", false);
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster"); 
        foreach (GameObject monster in monsters)
        {
            Collider2D monsterCollider = monster.GetComponent<Collider2D>();
            if (monsterCollider != null)
            {
                monsterCollider.isTrigger = false;
            }
        }
        
    }




    /*
     * Handles a Single Snake Body Part
     * */
    private class SnakeBodyPart {

        private SnakeMovePosition snakeMovePosition;
        private Transform transform;

        public SnakeBodyPart(int bodyIndex) {
            GameObject snakeBodyGameObject = new GameObject("SnakeBody", typeof(SpriteRenderer));
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.i.snakeBodySprite;
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sortingOrder = -1 - bodyIndex;
            transform = snakeBodyGameObject.transform;
        }

        public void SetSnakeMovePosition(SnakeMovePosition snakeMovePosition) {
            this.snakeMovePosition = snakeMovePosition;

            transform.position = new Vector3(snakeMovePosition.GetGridPosition().x, snakeMovePosition.GetGridPosition().y);

            float angle;
            switch (snakeMovePosition.GetDirection()) {
            default:
            case Direction.Up: // Currently going Up
                switch (snakeMovePosition.GetPreviousDirection()) {
                default: 
                    angle = 0; 
                    break;
                case Direction.Left: // Previously was going Left
                    angle = 0 + 45; 
                    transform.position += new Vector3(.2f, .2f);
                    break;
                case Direction.Right: // Previously was going Right
                    angle = 0 - 45; 
                    transform.position += new Vector3(-.2f, .2f);
                    break;
                }
                break;
            case Direction.Down: // Currently going Down
                switch (snakeMovePosition.GetPreviousDirection()) {
                default: 
                    angle = 180; 
                    break;
                case Direction.Left: // Previously was going Left
                    angle = 180 - 45;
                    transform.position += new Vector3(.2f, -.2f);
                    break;
                case Direction.Right: // Previously was going Right
                    angle = 180 + 45; 
                    transform.position += new Vector3(-.2f, -.2f);
                    break;
                }
                break;
            case Direction.Left: // Currently going to the Left
                switch (snakeMovePosition.GetPreviousDirection()) {
                default: 
                    angle = +90; 
                    break;
                case Direction.Down: // Previously was going Down
                    angle = 180 - 45; 
                    transform.position += new Vector3(-.2f, .2f);
                    break;
                case Direction.Up: // Previously was going Up
                    angle = 45; 
                    transform.position += new Vector3(-.2f, -.2f);
                    break;
                }
                break;
            case Direction.Right: // Currently going to the Right
                switch (snakeMovePosition.GetPreviousDirection()) {
                default: 
                    angle = -90; 
                    break;
                case Direction.Down: // Previously was going Down
                    angle = 180 + 45; 
                    transform.position += new Vector3(.2f, .2f);
                    break;
                case Direction.Up: // Previously was going Up
                    angle = -45; 
                    transform.position += new Vector3(.2f, -.2f);
                    break;
                }
                break;
            }

            transform.eulerAngles = new Vector3(0, 0, angle);
        }

        public Vector3 GetGridPosition() {
            return snakeMovePosition.GetGridPosition();
        }
    }



    /*
     * Handles one Move Position from the Snake
     * */
    private class SnakeMovePosition {

        private SnakeMovePosition previousSnakeMovePosition;
        private Vector3 gridPosition;
        private Direction direction;

        public SnakeMovePosition(SnakeMovePosition previousSnakeMovePosition, Vector3 gridPosition, Direction direction) {
            this.previousSnakeMovePosition = previousSnakeMovePosition;
            this.gridPosition = gridPosition;
            this.direction = direction;
        }

        public Vector3 GetGridPosition() {
            return gridPosition;
        }

        public Direction GetDirection() {
            return direction;
        }

        public Direction GetPreviousDirection() {
            if (previousSnakeMovePosition == null) {
                return Direction.Right;
            } else {
                return previousSnakeMovePosition.direction;
            }
        }

    }

    
}

