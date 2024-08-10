using UnityEngine;

public class HardBlock : MonoBehaviour
{
    public GameObject items;
    public int numberOfInstances = 3;
    public float minSpacing = 70f; // Minimum spacing between instances
    public float maxSpacing = 120f; // Maximum spacing between instances
    public float minX = 25f; // Minimum X value
    public float maxX = 230f; // Maximum X value

    void Start()
    {
        Vector3 position = transform.position; // Starting position
        position.x = Mathf.Clamp(position.x, minX, maxX); // Ensure the starting X is within the desired range

        float currentX = position.x; // Track the current X position

        for (int i = 0; i < numberOfInstances; i++)
        {
            // Randomize spacing between minSpacing and maxSpacing
            float randomSpacing = Random.Range(minSpacing, maxSpacing);

            // Ensure the next block position does not exceed maxX
            if (currentX + randomSpacing > maxX)
            {
                randomSpacing = maxX - currentX; // Adjust spacing to fit within maxX
            }

            // Update position for the new block
            position.x = currentX + randomSpacing;
            position.y = 1f;

            // Instantiate the item at the calculated position
            Instantiate(items, position, Quaternion.identity);

            // Update currentX to the new position for the next block
            currentX = position.x;
        }
    }
}
