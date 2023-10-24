using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private void Start() {
        DontDestroyOnLoad(gameObject);
    }

    public void LoadLevel1()
    {
        StartCoroutine(LoadSceneAsync("Level1"));
    }

    public void LoadLevel2()
    {
        StartCoroutine(LoadSceneAsync("Level2"));
    }

    IEnumerator LoadSceneAsync(string level)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Scenes/" + level);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
