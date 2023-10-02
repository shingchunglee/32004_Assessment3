using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Movable : MonoBehaviour
{
    public float xVelocity { get; private set; } = 0f;
    public float yVelocity { get; private set; } = 0f;

    private Tween activeTween;
    public bool finishedTween = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (activeTween == null) return;
        if (Time.time - activeTween.StartTime > activeTween.Duration)
        {
            EndTween();
            return;
        }
        if (Vector3.Distance(activeTween.Target.position, activeTween.EndPos) <= 0.1f)
        {
            EndTween();
            return;
        }
        float timeFraction = (Time.time - activeTween.StartTime) / activeTween.Duration;
        timeFraction = Easing.EaseFraction(timeFraction, activeTween.Easing);
        transform.position = Vector3.Lerp(activeTween.StartPos, activeTween.EndPos, timeFraction);
    }

    public void AddTween()
    {
       if (activeTween != null)
       {
            activeTween.Queue = new Tween(
                gameObject.transform, 
                activeTween.EndPos,
                new Vector3(activeTween.EndPos.x + xVelocity, activeTween.EndPos.y + yVelocity, 0.0f),
                activeTween.StartTime + activeTween.Duration,
                1f,
                () => { finishedTween = true; }
            );
       }
       else
       {
            activeTween = new Tween(
                gameObject.transform, 
                gameObject.transform.position,
                new Vector3(gameObject.transform.position.x + xVelocity, gameObject.transform.position.y + yVelocity, 0.0f),
                Time.time,
                1f,
                () => { finishedTween = true; }
            );
       }
    }

    private void EndTween()
    {
        activeTween.Target.position = activeTween.EndPos;
        activeTween.Callback();
        if (activeTween.Queue != null)
        {
            activeTween = activeTween.Queue;
        }
        else 
        {
            activeTween = null;
        }
    }

    public void Down()
    {
        xVelocity = 0;
        yVelocity = -1f;
    }
    public void Left()
    {
        xVelocity = -1f;
        yVelocity = 0;
    }
    public void Up()
    {
        xVelocity = 0;
        yVelocity = 1f;
    }
    public void Right()
    {
        xVelocity = 1f;
        yVelocity = 0;
    }
}