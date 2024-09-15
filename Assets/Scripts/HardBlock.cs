using UnityEngine;

public class HardBlock : MonoBehaviour
{
    public GameObject items;
    public int minInstances = 3;  // Minimum number of instances
    public int maxInstances = 5;  // Maximum number of instances
    public float minSpacing = 50f; // Minimum spacing between instances
    public float maxSpacing = 75f; // Maximum spacing between instances
    public float minX = 25f; // Minimum X value
    public float maxX = 225f; // Maximum X value

    void Start()
    {
        // Randomize the number of instances
        int numberOfInstances = Random.Range(minInstances, maxInstances + 1);

        Vector3 position = transform.position; // Starting position
        position.x = Mathf.Clamp(position.x, minX, maxX); // Ensure the starting X is within the desired range

        float currentX = position.x; // Track the current X position

        for (int i = 0; i < numberOfInstances; i++)
        {
            // Randomize spacing between minSpacing and maxSpacing
            float randomSpacing = Random.Range(minSpacing, maxSpacing);

            // Calculate the next X position
            float nextX = currentX + randomSpacing;

            // Ensure the next block position does not exceed maxX
            if (nextX > maxX)
            {
                nextX = maxX; // Adjust to fit within maxX
                randomSpacing = nextX - currentX; // Recalculate spacing
            }

            // Update position for the new block
            position.x = nextX;
            position.y = 1f;

            // Instantiate the item at the calculated position
            Instantiate(items, position, Quaternion.identity);

            // Update currentX to the new position for the next block
            currentX = position.x;

            // Break the loop if we've reached the maxX value
            if (currentX >= maxX)
            {
                break;
            }
        }
    }
}
