using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownController : MonoBehaviour
{
    [SerializeField] private Text countdownText;
    [SerializeField] private GameSoundController gameSoundController;
    
    public IEnumerator StartCountdown(System.Action value)
    {
        int countdownTime = 3;
    
        while (countdownTime > 0)
        {
            countdownText.text = countdownTime.ToString();
            yield return new WaitForSeconds(1f);
            countdownTime--;
        }
        
        countdownText.text = "GO!";
        yield return new WaitForSeconds(1f);

        value();
        countdownText.gameObject.SetActive(false);
        gameSoundController.playGhostNormal();
    }
}
