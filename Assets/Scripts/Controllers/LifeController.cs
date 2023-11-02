using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeController : MonoBehaviour
{
    public int lives = 3;
    
    public void LoseLife()
    {
        lives--;
    }
}
