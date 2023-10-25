using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GhostTimerController : MonoBehaviour
{
    [SerializeField] Text ghostTimerText;
    internal void updateText(string newTime)
    {
        
        ghostTimerText.text = newTime;
    }

    internal void SetActive(bool newActive)
    {
        ghostTimerText.gameObject.SetActive(newActive);
    }
}
