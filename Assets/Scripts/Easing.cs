using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum EasingTypes
{
    linear
}
public class Easing
{
    internal static float Linear(float timeFraction)
    {
        return timeFraction;
    }

    internal static float EaseFraction(float timeFraction, EasingTypes easing)
    {
        switch (easing)
        {
            case EasingTypes.linear:
                return Linear(timeFraction);
            default:
                return Linear(timeFraction);
        }
    }
}
