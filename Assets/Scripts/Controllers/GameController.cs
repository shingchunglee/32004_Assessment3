using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private Map map = Map.Instance;
    [SerializeField] private CountdownController countdownController;
    [SerializeField] private LifeController lifeController;
    [SerializeField] private LifeIndicatorController lifeIndicatorController;
    [SerializeField] private GameOverTextController gameOverTextController;
    [SerializeField] private GameTimerController gameTimerController;
    [SerializeField] private ScoreController scoreController;
    public bool isPause = true;

    // Start is called before the first frame update
    void Start()
    {
        DisableGhostTimer();
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (AllPelletsEaten() || lifeController.lives <= 0)
        {
            gameOverTextController.setActive();
            isPause = true;

            int currHighScore = PlayerPrefs.GetInt("HighScore", 0);
            float currBestTime = PlayerPrefs.GetFloat("BestTime", 0f);
            
            PlayerPrefs.SetInt("HighScore", scoreController.currentScore > currHighScore ? scoreController.currentScore : currHighScore);
            PlayerPrefs.SetFloat("BestTime", (gameTimerController.GetTime() > currBestTime) && scoreController.currentScore == currHighScore ? gameTimerController.GetTime() : currBestTime);
            PlayerPrefs.Save();
            
            Invoke("ReturnToStartScene", 3f);
        }
    }

    private void ReturnToStartScene()
    {
        GameObject.FindWithTag("Managers")?.GetComponent<SceneController>().LoadStart();
    }

    private bool AllPelletsEaten()
    {
        return GameObject.FindWithTag("Pellet") == null;
    }

    private void DisableGhostTimer()
    {
        GameObject ghostTimerObject = GameObject.FindGameObjectWithTag("GhostTimer");
        if (ghostTimerObject != null)
        {
            ghostTimerObject.SetActive(false);
        }
    }

    public void Init(UnityEngine.Events.UnityAction call)
    {
        GameObject exitButton = GameObject.FindGameObjectWithTag("ExitButton");

        if (exitButton != null)
        {
            exitButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                call();
            });
        }

        StartCoroutine(countdownController.StartCountdown(() => {isPause = false;}));
    }

    public void Init()
    {
        lifeIndicatorController.UpdateLifeObjects(lifeController.lives);
        StartCoroutine(countdownController.StartCountdown(() => {isPause = false;}));
    }
}
