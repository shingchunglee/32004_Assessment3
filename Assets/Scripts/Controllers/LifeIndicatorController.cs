using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeIndicatorController : MonoBehaviour
{
    [SerializeField] private GameObject LifeTracker;
    [SerializeField] private GameObject lifePrefab;

    public void UpdateLifeObjects(int lives)
    {
         
        // GameObject lifePrefab = GameObject.FindGameObjectWithTag("Life");
        if (lifePrefab != null)
        {
            RectTransform rectTransform = lifePrefab.GetComponent<RectTransform>();
            float width = rectTransform.sizeDelta.x;
            Transform parent = GameObject.FindGameObjectWithTag("Lives").transform;

            foreach (Transform child in parent) {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < lives; i++)
            {
                GameObject newLife = Instantiate(lifePrefab, parent);
                RectTransform newRectTransform = newLife.GetComponent<RectTransform>();
                newRectTransform.anchoredPosition = new Vector2(i * width, 0);
            }
        }
    }
}
