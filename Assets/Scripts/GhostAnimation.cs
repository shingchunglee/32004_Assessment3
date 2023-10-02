using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAnimation : MonoBehaviour
{
    public Animator animatorController;
    [SerializeField] private int ghostColor;

    // Start is called before the first frame update
    void Start()
    {
        animatorController.SetInteger("GhostColor", ghostColor);
    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }
    public void Down()
    {
        animatorController.SetInteger("GhostDirection", 0);
    }
    public void Up()
    {
        animatorController.SetInteger("GhostDirection", 1);
    }
    public void Left()
    {
        animatorController.SetInteger("GhostDirection", 2);
    }
    public void Right()
    {
        animatorController.SetInteger("GhostDirection", 3);
    }
    public void Normal()
    {
        animatorController.SetInteger("GhostState", 0);
    }
    public void Scared()
    {
        animatorController.SetInteger("GhostState", 1);
    }
    public void Recovering()
    {
        animatorController.SetInteger("GhostState", 2);
    }
    public void Dead()
    {
        animatorController.SetInteger("GhostState", 3);
    }
}
