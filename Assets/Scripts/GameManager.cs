using System.Collections;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

//public delegate void GameAcceleratorDelegate();

public class GameManager : MonoBehaviour
{
    [SerializeField] Text timerText, speedText, gameOverText, pointsText;
    [SerializeField] Button returnToMenuButton;
    GameObject mainCamera;
    PlayerController playerController;
    TreeSpawnManager treeSpawnManager;
    RoadObjectSpawnManager roadObjectSpawnManager;

    //static GameAcceleratorDelegate gameAcceleration;
    int timer;
    static int points;
    const float TIME_BETWEEN_ACCELERATIONS = 7.0f;
    const float GAME_OVER_TEXT_SPEED = 250.0f;
    const float POINTS_TEXT_SPEED = 500.0f;
    bool isGameOverTasksDone;
    bool isGamePaused;

    //public static event GameAcceleratorDelegate GameAcceleration
    //{
    //    add { gameAcceleration += value; }
    //    remove { gameAcceleration -= value; }
    //}

    public static float GameAccelerator { get; set; }
    public static bool IsGameActive { get; set; }
    public static float CharacterSpeed { get; set; }
    public static int Stars { get; set; }
    public static int Points { get { return points; } }

    public void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        treeSpawnManager = GameObject.Find("Spawner").GetComponent<TreeSpawnManager>();
        roadObjectSpawnManager = GameObject.Find("Spawner").GetComponent<RoadObjectSpawnManager>();

        mainCamera = GameObject.Find("Main Camera");
        IsGameActive = true;
        CharacterSpeed = 12;
        GameAccelerator = 1.1f;
        StartCoroutine(UpdateTimer());
        StartCoroutine(AccelerateGame());
    }

    public void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.P))
        {
            CharacterSpeed *= GameAccelerator;
            IncreaseSpawnerFrequency();
            IncreasePlayerAnimationSpeed();
            //gameAcceleration.Invoke();
            speedText.text = string.Format("Speed : {0:F1} m/s", CharacterSpeed);
        }
#endif
        if (IsGameActive)
        {
        }
        else
        {
            MoveGameOverTexts();

            if (!isGameOverTasksDone)
            {
                CalculatePoints();
                StopAllCoroutines();
                gameOverText.gameObject.SetActive(true);
                StartCoroutine(ShowReturnToMenuButton(secondsToWait: 3));
                StartCoroutine(ShowPoints(secondsToWait: 3));
                isGameOverTasksDone = true;
            }
        }

        CheckQuitRequest();
    }

    void IncreaseSpawnerFrequency()
    {
        if (treeSpawnManager != null)
        treeSpawnManager.RestartSpawning();

        if(roadObjectSpawnManager != null)
        roadObjectSpawnManager.RestartSpawning();
    }

    void IncreasePlayerAnimationSpeed()
    {
        if(playerController != null)
        playerController.ChangeAnimationSpeed();
    }
    IEnumerator AccelerateGame()
    {
        while (IsGameActive)
        {
            CharacterSpeed *= GameAccelerator;

            speedText.text = string.Format("Speed : {0:F1} m/s", CharacterSpeed);

            IncreaseSpawnerFrequency();
            IncreasePlayerAnimationSpeed();
            //gameAcceleration.Invoke();

            yield return new WaitForSeconds(TIME_BETWEEN_ACCELERATIONS);
        }
    }
    IEnumerator UpdateTimer()
    {
        while (IsGameActive)
        {
            yield return new WaitForSeconds(1);
            timer++;
            timerText.text = "Timer : " + timer + " s";
        }
    }
    IEnumerator ShowReturnToMenuButton(int secondsToWait)
    {
        yield return new WaitForSeconds(secondsToWait);
        returnToMenuButton.gameObject.SetActive(true);
    }

    IEnumerator ShowPoints(int secondsToWait)
    {
        yield return new WaitForSeconds(secondsToWait);
        pointsText.gameObject.SetActive(true);
        pointsText.text = "Points : " + points;
    }
    private void CalculatePoints()
    {
        const int POINTS_PER_STAR = 10;
        points = timer + Stars * POINTS_PER_STAR;
        Storage.instance.CheckRecord();
    }

    private void MoveGameOverTexts()
    {
        if (gameOverText.rectTransform.localPosition.y > 0)
        {
            gameOverText.rectTransform.Translate(GAME_OVER_TEXT_SPEED * Time.deltaTime * Vector2.down);
        }
        else if (pointsText.rectTransform.localPosition.x > 0)
        {
            pointsText.rectTransform.Translate(POINTS_TEXT_SPEED * Time.deltaTime * Vector2.left);
        }
    }

    void CheckQuitRequest()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && IsGameActive)
        {
            if (!isGamePaused)
            {
                isGamePaused = true;
                StartCoroutine(ShowReturnToMenuButton(secondsToWait: 0));
                Time.timeScale = 0;
                mainCamera.GetComponent<AudioSource>().Stop();
            }
            else
            {
                isGamePaused = false;
                Time.timeScale = 1;
                returnToMenuButton.gameObject.SetActive(false);
                mainCamera.GetComponent<AudioSource>().Play();
            }
        }
    }
}



