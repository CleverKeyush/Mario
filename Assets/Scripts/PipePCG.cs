using UnityEngine;

public class PipePCG : MonoBehaviour
{
    public GameObject pipePrefab; // Assign your pipe prefab here
    public int minPipes = 3;      // Minimum number of pipes to spawn
    public int maxPipes = 6;      // Maximum number of pipes to spawn
    public float minX = 30f;      // Minimum x position
    public float maxX = 200f;     // Maximum x position
    public float minY = 2f;      // Minimum y position
    public float maxY = 3f;       // Maximum y position

    public float minSpacing = 30f;
    public float maxSpacing = 90f;

    void Start()
    {
        // Determine the number of pipes to spawn
        int numberOfPipes = Random.Range(minPipes, maxPipes + 1);

        for (int i = 0; i < numberOfPipes; i++)
        {
            // Generate random x and y positions within the specified range
            float randomX = Random.Range(minX, maxX);
            float randomY = Random.Range(minY, maxY);
            float randomSpacing = Random.Range(minSpacing, maxSpacing);

            // Create a new position for the pipe
            Vector3 spawnPosition = new Vector3(randomX, randomY, 0f);

            // Instantiate the pipe at the random position
            Instantiate(pipePrefab, spawnPosition, Quaternion.identity);
        }
    }
}
