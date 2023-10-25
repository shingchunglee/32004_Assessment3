using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayable : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip walking;
    [SerializeField] private AudioClip wallCollision;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void PlayWalkSound()
    {
        audioSource.PlayOneShot(walking);
    }

    internal void PlayWallCollisionSound()
    {
       audioSource.PlayOneShot(wallCollision);
    }
}
