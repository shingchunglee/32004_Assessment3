using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private Tweener tweener;
    // Start is called before the first frame update
    void Start()
    {
        tweener = GetComponent<Tweener>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool gameObjectMove(GameObject item) {
        Move moveComponent = item.GetComponent<Move>();

        return tweener.AddTween(
            item.transform, 
            item.transform.position,
            new Vector3(item.transform.position.x + moveComponent.xVelocity, item.transform.position.y + moveComponent.yVelocity, 0.0f),
            1f
        );
    }
}
