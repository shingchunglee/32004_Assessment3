using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{

    private void Start() {
        DontDestroyOnLoad(this);
    }

    public void LoadStart()
    {
        StartCoroutine(LoadSceneAsync(
            "StartScene",
            () => {}
        ));
    }

    public void LoadLevel1()
    {
        StartCoroutine(LoadSceneAsync(
            "Level1", 
            () => {
                GameObject gameControllerObject = GameObject.FindGameObjectWithTag("GameController");

                if (gameControllerObject != null)
                {
                    GameController gameController = gameControllerObject.GetComponent<GameController>();
                    if (gameController != null)
                    {
                        gameController.Init(() =>
                        {
                            SceneManager.LoadScene("StartScene");
                        });
                    }
                }

                
            }
        ));
    }

    public void LoadLevel2()
    {
        StartCoroutine(LoadSceneAsync("Level2", () => {}));
    }

    IEnumerator LoadSceneAsync(string level, System.Action callback)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(level);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        callback();
    }
}
