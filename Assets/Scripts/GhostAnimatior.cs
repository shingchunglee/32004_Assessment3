using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GhostAnimatior : MonoBehaviour
{
    public Animator animatorController;

    // Start is called before the first frame update
    void Start()
    {
        int ghostColor = (int)gameObject.GetComponent<GhostController>()?.ghostType;
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
