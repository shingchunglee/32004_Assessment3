using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentAnimator : MonoBehaviour
{
    public Animator animatorController;
    [SerializeField] private int state;

    // Start is called before the first frame update
    void Start()
    {
        animatorController.SetInteger("State", state);
    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }
    public void Down()
    {
        animatorController.SetInteger("Direction", 0);
    }
    public void Up()
    {
        animatorController.SetInteger("Direction", 1);
    }
    public void Left()
    {
        animatorController.SetInteger("Direction", 2);
    }
    public void Right()
    {
        animatorController.SetInteger("Direction", 3);
    }
    public void Normal()
    {
        animatorController.SetInteger("State", 0);
    }
    public void Dead()
    {
        animatorController.SetInteger("State", 1);
    }
}
