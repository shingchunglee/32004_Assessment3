using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSoundController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip introClip;
    [SerializeField] private AudioClip ghostNormalClip;
    [SerializeField] private AudioClip ghostScaredClip;
    [SerializeField] private AudioClip ghostDeadClip;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playGhostScared()
    {
        audioSource.clip = ghostScaredClip;
        audioSource.Play();
    }

    public void playGhostNormal()
    {
        audioSource.clip = ghostNormalClip;
        audioSource.Play();
    }

    internal void playGhostDead()
    {
        audioSource.clip = ghostDeadClip;
        audioSource.Play();
    }
}
