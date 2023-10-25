using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private const int initLives = 3;
    public int lives = 3;
    [SerializeField] private GameObject lifePrefab;
    private Map map = Map.Instance;

    // Start is called before the first frame update
    void Start()
    {
        CreateLifeObjects();
        DisableGhostTimer();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void CreateLifeObjects()
    {
         
        // GameObject lifePrefab = GameObject.FindGameObjectWithTag("Life");
        if (lifePrefab != null)
        {
            RectTransform rectTransform = lifePrefab.GetComponent<RectTransform>();
            float width = rectTransform.sizeDelta.x;
            Transform parent = GameObject.FindGameObjectWithTag("Lives").transform;

            for (int i = 0; i < initLives; i++)
            {
                GameObject newLife = Instantiate(lifePrefab, parent);
                RectTransform newRectTransform = newLife.GetComponent<RectTransform>();
                newRectTransform.anchoredPosition = new Vector2(i * width, 0);
            }
        }
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
    }
}
