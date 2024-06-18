
using System.Collections;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    public GameObject powerupPrefab2X;
    public GameObject powerupPrefabLife;
    
    public BoxCollider2D powerupSpawnArea;
    public float minSpawnInterval = 5f;
    public float maxSpawnInterval = 10f;
    public float minPowerupDuration = 5f;
    public float maxPowerupDuration = 7f;
    private bool isGameActive = true;
    public bool isPowerupSpawn = false;

    private GameObject currentPowerup;
    public Vector3 powerUpPosition;

    private void Start()
    {
        StartCoroutine(SpawnPowerupRoutine());
    }

    private IEnumerator SpawnPowerupRoutine()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnInterval, maxSpawnInterval));

            if (isGameActive)
            {
                // Randomly choose which powerup to spawn
                if (Random.Range(0f, 1f) < 0.5f)
                    SpawnPowerup(powerupPrefab2X);
                else
                    SpawnPowerup(powerupPrefabLife);

                float powerupDuration = Random.Range(minPowerupDuration, maxPowerupDuration);
                yield return new WaitForSeconds(powerupDuration);
            }

            DestroyPowerup();
        }
    }

    private void SpawnPowerup(GameObject powerupPrefab)
    {
        Bounds bounds = powerupSpawnArea.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        currentPowerup = Instantiate(powerupPrefab, new Vector3(x, y, 0.0f), Quaternion.identity);
        powerUpPosition = currentPowerup.transform.position;
        isPowerupSpawn = true;
    }

    public void DestroyPowerup()
    {
        // Check if the currentPowerup is not null before attempting to destroy it.
        if (currentPowerup != null)
        {
            Destroy(currentPowerup);
            isPowerupSpawn = false;
            //Debug.Log("Destroyed");
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            // Check the type of powerup and handle accordingly
            if (currentPowerup != null)
            {
                if (currentPowerup.CompareTag("Powerup2X"))
                {
                    // Handle 2X powerup collision
                    // For example, double the player's score
                }
                else if (currentPowerup.CompareTag("PowerupLife"))
                {
                    // Handle life powerup collision
                    // For example, increase the player's lives
                }
            }

            DestroyPowerup();
        }
    }

    public void StopPowerupSpawning()
    {
        isGameActive = false;
        DestroyPowerup();
    }
}
