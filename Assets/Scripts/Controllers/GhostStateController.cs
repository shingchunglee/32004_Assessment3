using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GhostStateController : MonoBehaviour
{
    [SerializeField] private GhostController[] ghostControllers;
    [SerializeField] private GhostTimerController ghostTimerController;
    [SerializeField] private GameSoundController gameSoundController;
    
    public void SetScared() 
    {
        foreach (var ghostController in ghostControllers)
        {
            ghostController.updateState(1);
            StartCoroutine(StartGhostTimer());
        }
    }

    public void Die(GhostController ghostController)
    {
        ghostController.updateState(3);
        gameSoundController.playGhostDead();
    
        StartCoroutine(TransitionToWalkingState(5f, ghostController));
    }
    
    private IEnumerator TransitionToWalkingState(float duration, GhostController ghostController)
    {
        yield return new WaitForSeconds(duration);
    
        gameSoundController.playGhostNormal();
        ghostController.updateState(0);
    }

    private IEnumerator StartGhostTimer()
    {
        ghostTimerController.SetActive(true);
        gameSoundController.playGhostScared();

        float timer = 10f;

        while (timer > 0f)
        {
            timer -= Time.deltaTime;

            ghostTimerController.updateText(timer.ToString("F1"));

            if (timer <= 3f)
            {
                // Set the state of ghostControllers to 2
                foreach (var ghostController in ghostControllers)
                {
                    if (ghostController.state == 1) 
                    {
                        ghostController.updateState(2);
                    }
                }
            }

            yield return null;
        }

        ghostTimerController.SetActive(false);
        gameSoundController.playGhostNormal();
        foreach (var ghostController in ghostControllers)
        {
            if (ghostController.state == 2)
            {
                ghostController.updateState(0);
            }
        }
    }
}
