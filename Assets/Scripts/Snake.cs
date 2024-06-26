using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
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
    // Public variables accessible in Unity Inspector
    public float speed = 0.9f;
    // public float powerupDuration = 15f; // Duration of the power-up in seconds
    // public float currentPowerupTime; // Time remaining for the current power-up
    // public  bool powerup2XActive = false;
    public Button pauseButton;
    public GameObject leftButton;
    public GameObject rightButton;
    public GameObject upButton;
    public GameObject downButton;
    public GameObject joyStick;
    public GameObject Walls;
    public GameObject snakeBodyPrefab;
   
    public GameObject particleEffects;
    public GameObject particleEffects2XPowerUp;
    public GameObject particleEffectsLifePowerUp;
    public GameObject gmaeAssets;
    public GameObject monsters;
    public GameObject gameoverPanel;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI coinsCollectedText;
    public TextMeshProUGUI powerUpDurationText;
    public Sprite normalHead;
    public Sprite nearFoodHead;
    public Sprite gameOverHead;
    public AudioClip gameOverSound;
    public AudioClip powerUpSound;
    public AudioClip eatSound;
    

    // Private variables for internal use
    private State state;
    private Animator animator;
     private Direction gridMoveDirection;
    private Vector3 gridPosition;
    private float gridMoveTimer;
    private float gridMoveTimerMax = 0.09f;
    private List<SnakeBodyPart> snakeBodyPartList;
    private List<SnakeMovePosition> snakeMovePositionList;
    private LevelGrid levelGrid;
    private AudioSource audioSource;
    private SpriteRenderer snakeHeadSpriteRenderer;
    public static float score;
    private float powerupDuration = 15f;
    private float currentPowerupTime;
    private bool powerup2XActive = false;
    private bool PowerupLifeActive = false;
    private int snakeBodySize;
    private Vector2 touchStartPos;
    private bool foodBeingConsumed = false;
    private bool isMode;
    private bool isModeJoyStick;
    private bool isSwipeMode;
    private bool isWall;
    private bool isSimple;
    
    private Vector3 targetGridPosition;
   private Vector3 targetGridPositionMax;
   private Coroutine powerupCoroutine;

   private const string HIGH_SCORE_KEY = "HighScore";
    private const float BASE_SCORE_INCREMENT = 5f;
    private const float SCORE_INCREMENT_2X_POWERUP = 10f;
    
    
    private static Snake _instance;

    public static Snake Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Snake>();
                if (_instance == null)
                {
                    Debug.LogError("No Snake instance found in the scene.");
                }
            }
            return _instance;
        }
    }
    

    
    public void Setup(LevelGrid levelGrid) {
        this.levelGrid = levelGrid;
        // Debug.Log(levelGrid);
        // Debug.Log(levelGrid);
        
    }
     private bool intToBool(int intValue)
     {
        return intValue == 1 ? true : false;
     }
    private void Start() {
        PlayerPrefs.SetInt("IsSwipeMode", 1);
         // Subscribe to the event for control settings changes
        SettingManager.OnControlSettingsChanged += UpdateControlSettings;
        SettingManager.OnLevelSettingsChanged += UpdateLevelSettings;

       
        LoadControlSettings();
        LoadLevelSettings();
        UpdateUI();

        
        audioSource = GameObject.Find("Snake").GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        bool isMode = intToBool(PlayerPrefs.GetInt("IsMode"));
        bool isModeJoyStick = intToBool(PlayerPrefs.GetInt("IsModeJoyStick"));
        Time.timeScale = 1;
        GameManager.LoadGameState();  // Load game state if continuing
        
       
        
        
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
    private void UpdateLevelSettings(bool isSimple, bool isWall)
    {
        this.isSimple = isSimple;
        this.isWall = isWall;

        if(isSimple)
        {
            if(Walls != null)
                Walls.gameObject.SetActive(false);
        }
        else if(isWall)
        {
            if(Walls!= null)
                Walls.gameObject.SetActive(true);
        }
    }
    private void LoadControlSettings() 
    {

    //Debug.Log(PlayerPrefs.HasKey("IsMode") || PlayerPrefs.HasKey("IsModeJoyStick") || PlayerPrefs.HasKey("IsSwipeMode"));
         // Only load control settings if they are not already set
    if (!PlayerPrefs.HasKey("IsMode") || !PlayerPrefs.HasKey("IsModeJoyStick") || !PlayerPrefs.HasKey("IsSwipeMode"))
    {
        // Set default control settings
        PlayerPrefs.SetInt("IsMode", 0);
        PlayerPrefs.SetInt("IsModeJoyStick", 0);
        PlayerPrefs.SetInt("IsSwipeMode", 1);
    }

    // Load and update control settings
    bool isMode = PlayerPrefs.GetInt("IsMode", 0) == 1;
    bool isModeJoyStick = PlayerPrefs.GetInt("IsModeJoyStick", 0) == 1;
    bool isSwipeMode = PlayerPrefs.GetInt("IsSwipeMode", 0) == 1;
    
    UpdateControlSettings(isMode, isModeJoyStick, isSwipeMode);
    
    }

    private void LoadLevelSettings() 
    {
        bool isSimple = PlayerPrefs.GetInt("IsSimple", 0) == 1;
        bool isWall = PlayerPrefs.GetInt("IsWall",0) == 1;

        UpdateLevelSettings(isSimple,isWall);
    }
   
    public void SetMoveDirection(Direction direction)
    {
        gridMoveDirection = direction;
    }
    private void Awake() {
        gridPosition = new Vector3(0, 0);
        gridMoveTimer = gridMoveTimerMax;
       // gridMoveDirection = Direction.Right;
        snakeHeadSpriteRenderer = GetComponent<SpriteRenderer>();
        snakeMovePositionList = new List<SnakeMovePosition>();
        snakeBodySize = 0;

        snakeBodyPartList = new List<SnakeBodyPart>();

        state = State.Alive;
    }

    private void Update() {
        if(score >= 25){
            
            speed = 1.1f;
        }
        if(score >= 50){
            speed = 1.5f;  
        }
        if(score >= 75){
            speed = 1.7f;
        }
        if(score >= 100)
        {
            speed = 1.9f;
        }
        if(score >= 125)
        {
            speed = 2f;
        }
        switch (state) {
        case State.Alive:
            HandleInput();
            if(isSwipeMode == true){
                HandleTouchInput();
            }
            else if(isModeJoyStick == true){
                FindObjectOfType<JoyStickMovement>().SetMoveDirectionFromJoystick();
            }
            HandleGridMovement();
            HandlePowerups();
            CheckProximityToFood();
            CheckProximityToPowerUp();
           
            break;
        case State.Dead:
        CheckProximityToGameOver();
            break;
        }
    }
    private void FixedUpdate() {
    if (state == State.Alive) {
       // HandleGridMovement();
        CheckCollision();
    }
    
}
    private void UpdateUI()
    {
        scoreText.text = "Score: " + score.ToString();
        highScoreText.text = "High Score: " + PlayerPrefs.GetFloat(HIGH_SCORE_KEY, 0f).ToString();
        
    }

     private void CheckProximityToFood() {
        Vector3 foodPosition = levelGrid.foodGridPosition;
        float distanceToFood = Vector3.Distance(gridPosition, foodPosition);
        if (distanceToFood < 1.5f) {
            ChangeSprite(nearFoodHead);
        } else {
            ChangeSprite(normalHead);
        }
    }
    private void CheckProximityToPowerUp() {
        Vector3 powerUpPosition = FindObjectOfType<PowerupManager>().powerUpPosition;
        float distanceToFood = Vector3.Distance(gridPosition, powerUpPosition);
        if (distanceToFood < 1.5f && FindAnyObjectByType<PowerupManager>().isPowerupSpawn) {
            ChangeSprite(nearFoodHead);
        }
    }
    private void CheckProximityToGameOver() {
        if (state == State.Dead) {
            ChangeSprite(gameOverHead);
        } else {
            ChangeSprite(normalHead);
        }
    }

    private void ChangeSprite(Sprite sprite) {
        snakeHeadSpriteRenderer.sprite = sprite;
    }

    // Handles inputs for keyboard
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
        
    // Touch Controls
    private void HandleTouchInput () 
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

    
    // For Button Controls
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
            case Direction.Right:   gridMoveDirectionVector = new Vector3(+0.5f, 0); break;
            case Direction.Left:    gridMoveDirectionVector = new Vector3(-0.5f, 0); break;
            case Direction.Up:      gridMoveDirectionVector = new Vector3(0, +0.5f); break;
            case Direction.Down:    gridMoveDirectionVector = new Vector3(0, -0.5f); break;
            }

            gridPosition += gridMoveDirectionVector;

            gridPosition = levelGrid.ValidateGridPosition(gridPosition);

        
          
            bool snakeAteFood = levelGrid.TrySnakeEatFood(gridPosition);
            if (snakeAteFood) {
                // Snake ate food, grow body
                audioSource.PlayOneShot(eatSound);
                snakeBodySize++;
                CreateSnakeBodyPart();
                //UpdateScoreUI();
                UpdateScore(BASE_SCORE_INCREMENT);
                ChangeSprite(normalHead);
            }

            if (snakeMovePositionList.Count >= snakeBodySize + 1) {
                snakeMovePositionList.RemoveAt(snakeMovePositionList.Count - 1);
            }

            UpdateSnakeBodyParts();

            // foreach (SnakeBodyPart snakeBodyPart in snakeBodyPartList) {
            //     Vector3 snakeBodyPartGridPosition = snakeBodyPart.GetGridPosition();
            //     if (gridPosition == snakeBodyPartGridPosition && PowerupLifeActive == false)
            //     {
            //         GameOver();    
            //     }
            // }

            transform.position = new Vector3(gridPosition.x, gridPosition.y);
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirectionVector) - 90);
        }
    }

    private void CheckCollision() {
        // Check for collision with body parts
        foreach (SnakeBodyPart snakeBodyPart in snakeBodyPartList) {
            if (gridPosition == snakeBodyPart.GetGridPosition() && !PowerupLifeActive) {
                GameOver();
                
                return;
            }
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

    private void UpdateScore(float increment)
    {
        if (powerup2XActive)
        {
            score += SCORE_INCREMENT_2X_POWERUP;
        }
        else
        {
            score += increment;
        }
        UpdateUI();

        if (score > PlayerPrefs.GetFloat(HIGH_SCORE_KEY, 0f))
        {
            PlayerPrefs.SetFloat(HIGH_SCORE_KEY, score);
            UpdateUI();
        }
    }
    

    private void HandlePowerups()
    {
        if (powerup2XActive || PowerupLifeActive)
        {
            if (powerupCoroutine == null)
            {
                powerupCoroutine = StartCoroutine(PowerupDurationCoroutine());
            }
        }
    }

    private IEnumerator PowerupDurationCoroutine()
    {
        float currentPowerupTime = powerupDuration;

        while (currentPowerupTime > 0)
        {
            currentPowerupTime -= Time.deltaTime;
            // Update UI or perform other actions related to power-up duration
            powerUpDurationText.text = "Power-up:" + Mathf.Ceil(currentPowerupTime) + "s";
            powerUpDurationText.enabled = true;
            yield return null;
        }
        
        powerUpDurationText.enabled = false; // Hide the text when no power-up is active
        DeactivatePowerup2X();
        DeactivatePowerupLife();
        powerupCoroutine = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
{
    //Debug.Log("Trigger Entered");

    if (collision.gameObject.tag == "Food" && !foodBeingConsumed)
    {
    
        return;
    }

    if (collision.gameObject.tag == "PowerUp")
    {
        //Debug.Log("2xpoerupactive");
        audioSource.PlayOneShot(powerUpSound);
        particleEffects2XPowerUp.gameObject.SetActive(true);
        FindObjectOfType<PowerupManager>().DestroyPowerup();

        ActivatePowerup2X();
    }
    else if (collision.gameObject.tag == "PowerUpLife")
    {
        //Debug.Log("powerUpLifeActive");
        
        audioSource.PlayOneShot(powerUpSound);
        particleEffectsLifePowerUp.gameObject.SetActive(true);
        FindObjectOfType<PowerupManager>().DestroyPowerup();

        ActivatePowerUpLife();
    }
    else if(collision.gameObject.tag == "Coin")
    {
        audioSource.PlayOneShot(powerUpSound);
        FindObjectOfType<PowerupManager>().DestroyPowerup(); 
        GameManager.AddCoins(1); // Update the coin count using GameManager
        //GameManager.coinCount++;
        UpdateCoinsCollectedUI();   
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
   
    //particleEffects.gameObject.SetActive(true);
    GameManager.SaveGameState();  // Save the game state
    state = State.Dead;
    Time.timeScale = 0;
    // FindObjectOfType<MonsterMovement>().StopMonsterMovements();
    // FindObjectOfType<PowerupManager>().StopPowerupSpawning();
    gameoverPanel.SetActive(true);
    pauseButton.gameObject.SetActive(false);
    scoreText.gameObject.SetActive(false);
    highScoreText.gameObject.SetActive(false);

    // SceneManager.LoadScene("GameOverScene");
   
}
public void ContinueGame()
{
    state = State.Alive;
    Time.timeScale = 1;
    pauseButton.gameObject.SetActive(true);
    scoreText.gameObject.SetActive(true);
    highScoreText.gameObject.SetActive(true);
    // Resume any other game logic or states that need to be reset
    UpdateUI();  // Update the UI to reflect the loaded score and other states
    StartCoroutine(ActivatePowerupLife());
}

private IEnumerator ActivatePowerupLife()
    {
        PowerupLifeActive = true;
        // Blink effect
        
        float blinkDuration = 0.2f; // Duration for each blink
        float blinkTimer = 0f;
        SpriteRenderer snakeSpriteRenderer = GetComponent<SpriteRenderer>();
        while (PowerupLifeActive)
        {
            blinkTimer += Time.deltaTime;

            if (blinkTimer >= blinkDuration)
            {
                snakeSpriteRenderer.enabled = !snakeSpriteRenderer.enabled; // Toggle visibility
                
                blinkTimer = 0f;
            }

            yield return null;
        }

        // Ensure the snake is visible at the end of the power-up duration
        snakeSpriteRenderer.enabled = true;
        yield return new WaitForSeconds(5f);
        PowerupLifeActive = false;
    }



    public void UpdateCoinsCollectedUI()
    {
    coinsCollectedText.text = "Coins: " + PlayerPrefs.GetInt("CoinCount");
    Debug.Log("Collected Coins Snake:" + coinsCollectedText.text);

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
        particleEffects2XPowerUp.gameObject.SetActive(false);
    }

    private void DeactivatePowerupLife () 
    {
        PowerupLifeActive = false;
        particleEffectsLifePowerUp.gameObject.SetActive(false);
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
            // GameObject snakeBodyGameObject = new GameObject("SnakeBody", typeof(SpriteRenderer));
            // snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.i.snakeBodySprite;
            // snakeBodyGameObject.GetComponent<SpriteRenderer>().sortingOrder = -1 - bodyIndex;
            // transform = snakeBodyGameObject.transform;
            // Instantiate the snake body part prefab
            GameObject snakeBodyGameObject = Object.Instantiate(GameAssets.i.snakeBodySprite);

            // Set the sorting order
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sortingOrder = -1 - bodyIndex;

            // Assign the transform
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
