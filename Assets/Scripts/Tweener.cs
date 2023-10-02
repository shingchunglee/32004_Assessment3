using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tweener : MonoBehaviour {
    //private Tween activeTween;
    private List<Tween> activeTweens;

    void Start() {
        activeTweens = new List<Tween>();
    }

    void Update() {
        Tween activeTween;
        for (int i = activeTweens.Count-1; i >=0; i--) //Tween activeTween in activeTweens.Reverse<Tween>())
        {
            activeTween = activeTweens[i];

            if (Vector3.Distance(activeTween.Target.position, activeTween.EndPos) > 0.01f) {
                float timeFraction = (Time.time - activeTween.StartTime) / activeTween.Duration;
                activeTween.Target.position = Vector3.Lerp(activeTween.StartPos,
                                                          activeTween.EndPos,
                                                           timeFraction);                
            } else {
                activeTween.Target.position = activeTween.EndPos;
                //activeTween = null;
                activeTween.Callback();
                activeTweens.RemoveAt(i);
            }
        }
    }

    public bool AddTween(Transform targetObject, Vector3 startPos, Vector3 endPos, float duration)
    {
        if (!TweenExists(targetObject))
        {
            activeTweens.Add(new Tween(targetObject, startPos, endPos, Time.time, duration, () => {
                Debug.Log("Tween completed");
            }));
            return true;
        }
        return false;
    }


    public bool TweenExists(Transform target) {
        foreach (Tween activeTween in activeTweens) {
            if (activeTween.Target.transform == target)
                return true;
        }
        return false;
    }
}
