using UnityEngine;
using UnityEngine.UI;

public class GameTimerController : MonoBehaviour
{
    [SerializeField] private Text gameTimerText;
    [SerializeField] private GameController gameController;
    private float timer;

    private void Start()
    {
    }

    private void Update()
    {
        if (!gameController.isPause) 
        {
            timer += Time.deltaTime; 

            int minutes = Mathf.FloorToInt(timer / 60);
            int seconds = Mathf.FloorToInt(timer % 60);
            int milliseconds = Mathf.FloorToInt(timer % 1 * 100);

            string formattedTime = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);

            gameTimerText.text = formattedTime;
        }
    }
}