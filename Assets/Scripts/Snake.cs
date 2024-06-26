using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;


public class Snake : MonoBehaviour
{
    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }

    private enum State
    {
        Alive,
        Dead
    }

    // public enum Difficulty 
    // {
    //     Low,
    //     Medium,
    //     High
    // }
    
    public bool powerup2XActive = false;
    private bool PowerupLifeActive = false;
    private Animator animator;
    public float powerupDuration = 15f;
    public float currentPowerupTime;
    public TextMeshProUGUI powerUpDurationText;
    public GameObject gmaeAssets;
    public GameObject monsters;
    public GameObject particleEffects;
    public GameObject particleEffects2XPowerUp;
    public GameObject particleEffectsLifePowerUp;
    public AudioClip gameOverSound;
    public AudioClip powerUpSound;
    public AudioClip eatSound;
    private AudioSource audioSource;
    private State state;
    public Direction gridMoveDirection;
    public Vector3 gridPosition;
    private float gridMoveTimer;
    private float gridMoveTimerMax;
    private LevelGrid levelGrid;
    private float speed;
    private int snakeBodySize;
    private List<SnakeMovePosition> snakeMovePositionList;
    private List<SnakeBodyPart> snakeBodyPartList;
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
    private bool isWall;
    private bool isSimple;
    public static Snake Instance;
    public Sprite normalHead;
    public Sprite nearFoodHead;
    public Sprite gameOverHead;
    private SpriteRenderer snakeHeadSpriteRenderer;
    private Vector3 targetGridPosition;
    private String difficulty;

    public void Setup(LevelGrid levelGrid)
    {
        this.levelGrid = levelGrid;
        Debug.Log(levelGrid);
        Debug.Log(levelGrid);

    }
    private bool intToBool(int intValue)
    {
        return intValue == 1 ? true : false;
    }
    private void Start()
    {
        difficulty = PlayerPrefs.GetString("Difficulty");
        Debug.Log("Difficulty: " + difficulty);
        PlayerPrefs.SetInt("IsSwipeMode", 1);
        SettingManager.OnControlSettingsChanged += UpdateControlSettings;
        SettingManager.OnLevelSettingsChanged += UpdateLevelSettings;

        LoadControlSettings();
        LoadLevelSettings();

        if(difficulty == "Easy"){
            speed = 0.9f;
        }
        else if(difficulty == "Medium"){
            speed = 1.1f;
        }
        else{
            speed = 1.5f;
        }
        
        audioSource = GameObject.Find("Snake").GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        bool isMode = intToBool(PlayerPrefs.GetInt("IsMode"));
        bool isModeJoyStick = intToBool(PlayerPrefs.GetInt("IsModeJoyStick"));
        //bool isLevel = intToBool(PlayerPrefs.GetInt("isLevel"));

        UpdateHighScoreUI();


    }
    private void UpdateControlSettings(bool isMode, bool isModeJoyStick, bool isSwipeMode)
    {
        this.isMode = isMode;
        this.isModeJoyStick = isModeJoyStick;
        this.isSwipeMode = isSwipeMode;

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
            DeActivateJoyStick();
            DeactivateControlButtons();
        }
    }
    private void UpdateLevelSettings(bool isSimple, bool isWall)
    {
        this.isSimple = isSimple;
        this.isWall = isWall;

        if (isSimple)
        {
            if (Walls != null)
                Walls.gameObject.SetActive(false);
        }
        else if (isWall)
        {
            if (Walls != null)
                Walls.gameObject.SetActive(true);
        }
    }
    private void LoadControlSettings()
    {

        Debug.Log(PlayerPrefs.HasKey("IsMode") || PlayerPrefs.HasKey("IsModeJoyStick") || PlayerPrefs.HasKey("IsSwipeMode"));
        if (!PlayerPrefs.HasKey("IsMode") || !PlayerPrefs.HasKey("IsModeJoyStick") || !PlayerPrefs.HasKey("IsSwipeMode"))
        {
            PlayerPrefs.SetInt("IsMode", 0);
            PlayerPrefs.SetInt("IsModeJoyStick", 0);
            PlayerPrefs.SetInt("IsSwipeMode", 1);
        }

        bool isMode = PlayerPrefs.GetInt("IsMode", 0) == 1;
        bool isModeJoyStick = PlayerPrefs.GetInt("IsModeJoyStick", 0) == 1;
        bool isSwipeMode = PlayerPrefs.GetInt("IsSwipeMode", 0) == 1;

        UpdateControlSettings(isMode, isModeJoyStick, isSwipeMode);
        // bool isMode = PlayerPrefs.GetInt("IsMode", 0) == 1;
        // bool isModeJoyStick = PlayerPrefs.GetInt("IsModeJoyStick", 0) == 1;
        // bool isSwipeMode = PlayerPrefs.GetInt("IsSwipeMode",1) == 1;

        // UpdateControlSettings(isMode, isModeJoyStick, isSwipeMode);
    }

    private void LoadLevelSettings()
    {
        bool isSimple = PlayerPrefs.GetInt("IsSimple", 0) == 1;
        bool isWall = PlayerPrefs.GetInt("IsWall", 0) == 1;

        UpdateLevelSettings(isSimple, isWall);
    }
    private void ActivateJoyStick()
    {
        if (joyStick != null)
        {
            joyStick.SetActive(true);
        }
    }
    private void DeActivateJoyStick()
    {
        if (joyStick != null)
        {
            // Disable joystick
            joyStick.SetActive(false);
        }
    }
    private void ActivateControlButtons()
    {
        if (leftButton != null && rightButton != null && upButton != null && downButton != null)
        {
            leftButton.SetActive(true);
            rightButton.SetActive(true);
            upButton.SetActive(true);
            downButton.SetActive(true);
        }
    }

    private void DeactivateControlButtons()
    {
        if (leftButton != null && rightButton != null && upButton != null && downButton != null)
        {
            leftButton.SetActive(false);
            rightButton.SetActive(false);
            upButton.SetActive(false);
            downButton.SetActive(false);
        }
    }
    public void SetMoveDirection(Direction direction)
    {
        gridMoveDirection = direction;
    }
    private void Awake()
    {
        gridPosition = new Vector3(0, 0);
        gridMoveTimerMax = 0.09f;
        gridMoveTimer = gridMoveTimerMax;
        // gridMoveDirection = Direction.Right;
        snakeHeadSpriteRenderer = GetComponent<SpriteRenderer>();
        snakeMovePositionList = new List<SnakeMovePosition>();
        snakeBodySize = 0;

        snakeBodyPartList = new List<SnakeBodyPart>();

        state = State.Alive;
    }

    private void Update()
    {
        if (score >= 25)
        {
            if(difficulty == "Hard"){
                speed = 1.5f;
            }
            else {
                speed = 1.1f;
            }
        }
        if (score >= 50)
        {
            speed = 1.5f;
        }
        if (score >= 75)
        {
            speed = 1.7f;
        }
        if (score >= 100)
        {
            speed = 1.9f;
        }
        if (score >= 125)
        {
            speed = 2f;
        }
        switch (state)
        {
            case State.Alive:
                HandleInput();
                if (isSwipeMode == true)
                {
                    HandleTouchInput();
                }
                else if (isModeJoyStick == true)
                {
                    FindObjectOfType<JoyStickMovement>().SetMoveDirectionFromJoystick();
                }
                HandleGridMovement();
                handlePowerups();
                CheckProximityToFood();
                CheckProximityToPowerUp();

                break;
            case State.Dead:
                CheckProximityToGameOver();
                break;
        }
    }
    private void FixedUpdate()
    {
        if (state == State.Alive)
        {
            // HandleGridMovement();
            CheckCollision();
        }

    }

    private void CheckProximityToFood()
    {
        Vector3 foodPosition = levelGrid.foodGridPosition;
        float distanceToFood = Vector3.Distance(gridPosition, foodPosition);
        if (distanceToFood < 1.5f)
        {
            ChangeSprite(nearFoodHead);
        }
        else
        {
            ChangeSprite(normalHead);
        }
    }
    private void CheckProximityToPowerUp()
    {
        Vector3 powerUpPosition = FindObjectOfType<PowerupManager>().powerUpPosition;
        float distanceToFood = Vector3.Distance(gridPosition, powerUpPosition);
        if (distanceToFood < 1.5f && FindAnyObjectByType<PowerupManager>().isPowerupSpawn)
        {
            ChangeSprite(nearFoodHead);
        }
    }
    private void CheckProximityToGameOver()
    {
        if (state == State.Dead)
        {
            ChangeSprite(gameOverHead);
        }
        else
        {
            ChangeSprite(normalHead);
        }
    }

    private void ChangeSprite(Sprite sprite)
    {
        snakeHeadSpriteRenderer.sprite = sprite;
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (gridMoveDirection != Direction.Down)
            {
                gridMoveDirection = Direction.Up;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (gridMoveDirection != Direction.Up)
            {
                gridMoveDirection = Direction.Down;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (gridMoveDirection != Direction.Right)
            {
                gridMoveDirection = Direction.Left;
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (gridMoveDirection != Direction.Left)
            {
                gridMoveDirection = Direction.Right;
            }
        }

    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchStartPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                Vector2 touchDelta = touch.position - touchStartPos;

                if (Mathf.Abs(touchDelta.x) > Mathf.Abs(touchDelta.y))
                {
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


    private void HandleGridMovement()
    {
        gridMoveTimer += Time.deltaTime * speed;

        if (gridMoveTimer >= gridMoveTimerMax)
        {
            gridMoveTimer -= gridMoveTimerMax;

            SnakeMovePosition previousSnakeMovePosition = null;
            if (snakeMovePositionList.Count > 0)
            {
                previousSnakeMovePosition = snakeMovePositionList[0];
            }

            SnakeMovePosition snakeMovePosition = new SnakeMovePosition(previousSnakeMovePosition, gridPosition, gridMoveDirection);
            snakeMovePositionList.Insert(0, snakeMovePosition);

            Vector3 gridMoveDirectionVector;
            switch (gridMoveDirection)
            {
                default:
                case Direction.Right: gridMoveDirectionVector = new Vector3(+0.5f, 0); break;
                case Direction.Left: gridMoveDirectionVector = new Vector3(-0.5f, 0); break;
                case Direction.Up: gridMoveDirectionVector = new Vector3(0, +0.5f); break;
                case Direction.Down: gridMoveDirectionVector = new Vector3(0, -0.5f); break;
            }

            gridPosition += gridMoveDirectionVector;

            gridPosition = levelGrid.ValidateGridPosition(gridPosition);



            bool snakeAteFood = levelGrid.TrySnakeEatFood(gridPosition);
            if (snakeAteFood)
            {
                audioSource.PlayOneShot(eatSound);
                snakeBodySize++;
                CreateSnakeBodyPart();
                UpdateScoreUI();
                ChangeSprite(normalHead);
            }

            if (snakeMovePositionList.Count >= snakeBodySize + 1)
            {
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

    private void CheckCollision()
    {
        foreach (SnakeBodyPart snakeBodyPart in snakeBodyPartList)
        {
            if (gridPosition == snakeBodyPart.GetGridPosition() && !PowerupLifeActive)
            {
                GameOver();

                return;
            }
        }
    }

    private void CreateSnakeBodyPart()
    {
        snakeBodyPartList.Add(new SnakeBodyPart(snakeBodyPartList.Count));
    }

    private void UpdateSnakeBodyParts()
    {
        for (int i = 0; i < snakeBodyPartList.Count; i++)
        {
            snakeBodyPartList[i].SetSnakeMovePosition(snakeMovePositionList[i]);
        }
    }


    private float GetAngleFromVector(Vector3 dir)
    {
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }

    public Vector3 GetGridPosition()
    {
        return gridPosition;
    }
    public Direction GetDirection()
    {
        return gridMoveDirection;
    }

    public List<Vector3> GetFullSnakeGridPositionList()
    {
        List<Vector3> gridPositionList = new List<Vector3>() { gridPosition };
        foreach (SnakeMovePosition snakeMovePosition in snakeMovePositionList)
        {
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

            if (currentPowerupTime > 0)
            {
                //StartCoroutine(BlinkSnake());
            }
            if (currentPowerupTime <= 0)
            {
                DeactivatePowerup2X();
                DeactivatePowerupLife();

                powerUpDurationText.enabled = false;
            }
        }
        else
        {
            powerUpDurationText.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger Entered");

        if (collision.gameObject.tag == "Food" && !foodBeingConsumed)
        {
            UpdateScoreUI();
            return;
        }

        if (collision.gameObject.tag == "PowerUp")
        {
            Debug.Log("2xpoerupactive");
            audioSource.PlayOneShot(powerUpSound);
            particleEffects2XPowerUp.gameObject.SetActive(true);
            FindObjectOfType<PowerupManager>().DestroyPowerup();

            ActivatePowerup2X();
        }
        else if (collision.gameObject.tag == "PowerUpLife")
        {
            Debug.Log("powerUpLifeActive");
            audioSource.PlayOneShot(powerUpSound);
            particleEffectsLifePowerUp.gameObject.SetActive(true);
            FindObjectOfType<PowerupManager>().DestroyPowerup();
            ActivatePowerUpLife();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Monster" && PowerupLifeActive == false)
        {
            GameOver();
        }
        if (collision.gameObject.tag == "Wall" && PowerupLifeActive == false)
        {
            GameOver();
        }

    }

    public void GameOver()
    {
        //particleEffects.gameObject.SetActive(true);
        state = State.Dead;
        SceneManager.LoadScene("GameOverScene");
    }

    public void UpdateScoreUI()
    {
        if (powerup2XActive)
        {
            score += 10;
            scoreText.text = "Score:" + score;
        }
        else
        {
            score += 5;
            scoreText.text = "Score:" + score;
        }
        if (score > PlayerPrefs.GetFloat("HighScore", 0))
        {
            PlayerPrefs.SetFloat("HighScore", score);
            UpdateHighScoreUI();
        }
    }

    public void UpdateHighScoreUI()
    {
        highScoreText.text = "High Score:" + PlayerPrefs.GetFloat("HighScore", 0);
    }

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

    private void DeactivatePowerupLife()
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

    private class SnakeBodyPart
    {
        private SnakeMovePosition snakeMovePosition;
        private Transform transform;

        public SnakeBodyPart(int bodyIndex)
        {
            GameObject snakeBodyGameObject = new GameObject("SnakeBody", typeof(SpriteRenderer));
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.i.snakeBodySprite;
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sortingOrder = -1 - bodyIndex;
            transform = snakeBodyGameObject.transform;
        }

        public void SetSnakeMovePosition(SnakeMovePosition snakeMovePosition)
        {
            this.snakeMovePosition = snakeMovePosition;

            transform.position = new Vector3(snakeMovePosition.GetGridPosition().x, snakeMovePosition.GetGridPosition().y);

            float angle;
            switch (snakeMovePosition.GetDirection())
            {
                default:
                case Direction.Up:
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default:
                            angle = 0;
                            break;
                        case Direction.Left:
                            angle = 0 + 45;
                            transform.position += new Vector3(.2f, .2f);
                            break;
                        case Direction.Right:
                            angle = 0 - 45;
                            transform.position += new Vector3(-.2f, .2f);
                            break;
                    }
                    break;
                case Direction.Down:
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default:
                            angle = 180;
                            break;
                        case Direction.Left:
                            angle = 180 - 45;
                            transform.position += new Vector3(.2f, -.2f);
                            break;
                        case Direction.Right:
                            angle = 180 + 45;
                            transform.position += new Vector3(-.2f, -.2f);
                            break;
                    }
                    break;
                case Direction.Left:
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default:
                            angle = +90;
                            break;
                        case Direction.Down:
                            angle = 180 - 45;
                            transform.position += new Vector3(-.2f, .2f);
                            break;
                        case Direction.Up:
                            angle = 45;
                            transform.position += new Vector3(-.2f, -.2f);
                            break;
                    }
                    break;
                case Direction.Right:
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default:
                            angle = -90;
                            break;
                        case Direction.Down:
                            angle = 180 + 45;
                            transform.position += new Vector3(.2f, .2f);
                            break;
                        case Direction.Up:
                            angle = -45;
                            transform.position += new Vector3(.2f, -.2f);
                            break;
                    }
                    break;
            }

            transform.eulerAngles = new Vector3(0, 0, angle);
        }

        public Vector3 GetGridPosition()
        {
            return snakeMovePosition.GetGridPosition();
        }
    }

    private class SnakeMovePosition
    {
        private SnakeMovePosition previousSnakeMovePosition;
        private Vector3 gridPosition;
        private Direction direction;

        public SnakeMovePosition(SnakeMovePosition previousSnakeMovePosition, Vector3 gridPosition, Direction direction)
        {
            this.previousSnakeMovePosition = previousSnakeMovePosition;
            this.gridPosition = gridPosition;
            this.direction = direction;
        }

        public Vector3 GetGridPosition()
        {
            return gridPosition;
        }

        public Direction GetDirection()
        {
            return direction;
        }

        public Direction GetPreviousDirection()
        {
            if (previousSnakeMovePosition == null)
            {
                return Direction.Right;
            }
            else
            {
                return previousSnakeMovePosition.direction;
            }
        }

    }

}
