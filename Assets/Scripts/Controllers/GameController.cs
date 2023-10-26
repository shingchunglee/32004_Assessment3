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
    public int currentScore = 0;
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
