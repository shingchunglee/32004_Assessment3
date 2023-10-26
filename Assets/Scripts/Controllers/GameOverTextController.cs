using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverTextController : MonoBehaviour
{
    [SerializeField] private Text countdownText;

    private void Start() {
        countdownText.gameObject.SetActive(false);
    }

    internal void setActive()
    {
        countdownText.gameObject.SetActive(true);
    }
}
