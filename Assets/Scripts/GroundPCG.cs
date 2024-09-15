using UnityEngine;

public class GroundPCG : MonoBehaviour
{
    public GameObject itemPrefab; // Reference to the ground block prefab
    public int numberOfInstances = 30; // Number of ground blocks to generate
    public float spacing = 3f; // Adjust the spacing between instances
    public int minWidth = 20; // Minimum number of tiles for the ground block
    public int maxWidth = 90; // Maximum number of tiles for the ground block
    public float fixedHeight = 0f; // Fixed height of the ground block
    public float maxX = 300f; // Maximum x position
    public float spacingStopX = 225f; // X position where spacing should stop

    void Start()
    {
        Vector3 position = transform.position; // Starting position

        for (int i = 0; i < numberOfInstances; i++)
        {
            // Check if the next ground block would exceed the max x position
            if (position.x >= maxX)
            {
                break; // Exit the loop if the position exceeds maxX
            }

            int randomWidth = Random.Range(minWidth, maxWidth); // Random width in terms of number of tiles

            // Ensure the ground block does not extend beyond maxX
            if (position.x + randomWidth >= maxX)
            {
                randomWidth = (int)(maxX - position.x); // Adjust the width to fit within maxX
            }

            // Create ground blocks side by side to form the desired width
            for (int j = 0; j < randomWidth; j++)
            {
                // Instantiate the top layer
                GameObject topBlock = Instantiate(itemPrefab, new Vector3(position.x + j, position.y, position.z), Quaternion.identity);

                // Instantiate the bottom layer
                GameObject bottomBlock = Instantiate(itemPrefab, new Vector3(position.x + j, position.y - 1, position.z), Quaternion.identity);
            }

            // Adjust the position for the next ground block
            if (position.x + randomWidth < spacingStopX)
            {
                position.x += randomWidth + spacing; // Move to the next position, adding randomWidth and spacing
            }
            else
            {
                position.x += randomWidth; // Move to the next position, without adding spacing
            }
        }
    }
}