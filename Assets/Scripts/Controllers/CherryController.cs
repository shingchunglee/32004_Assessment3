using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryController : MonoBehaviour
{
    public GameObject cherryPrefab;
    private float spawnInterval = 10f;
    private float moveSpeed = 1.5f;

    private float timer;

    private void Start()
    {
        timer = spawnInterval;
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            SpawnCherry();
            timer = spawnInterval;
        }
    }

    private void SpawnCherry()
    {
        Vector3 spawnPosition = CalculateSpawnPosition();

        GameObject cherry = Instantiate(cherryPrefab, spawnPosition, Quaternion.identity);

        StartCoroutine(MoveCherry(cherry));
    }

    private Vector3 CalculateSpawnPosition()
    {
        Camera mainCamera = Camera.main;

        float cameraHeight = mainCamera.orthographicSize * 2f;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        // Calculate random spawn position on any side of the level
        float spawnX = 0f;
        float spawnY = 0f;

        int side = Random.Range(0, 4);
        switch (side)
        {
            case 0: 
                spawnX = Random.Range(-cameraWidth / 2f, cameraWidth / 2f);
                spawnY = cameraHeight / 2f + 1f;
                break;
            case 1: // right
                spawnX = cameraWidth / 2f + 1f;
                spawnY = Random.Range(-cameraHeight / 2f, cameraHeight / 2f);
                break;
            case 2: // bottom
                spawnX = Random.Range(-cameraWidth / 2f, cameraWidth / 2f);
                spawnY = -cameraHeight / 2f - 1f;
                break;
            case 3: // left
                spawnX = -cameraWidth / 2f - 1f;
                spawnY = Random.Range(-cameraHeight / 2f, cameraHeight / 2f);
                break;
        }

        return new Vector3(spawnX, spawnY, 0f);
    }

    private IEnumerator MoveCherry(GameObject cherry)
    {
        Vector3 startPosition = cherry.transform.position;
        Vector3 endPosition = -startPosition;

        float distance = Vector3.Distance(startPosition, endPosition);
        float duration = distance / moveSpeed;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            if (cherry == null)
            {
                yield return null;
                continue;
            }
            cherry.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(cherry);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PacStudent"))
        {
        }
    }

    private void addScore()
    {
        
    }
}