using UnityEngine;

public class AltMysBri : MonoBehaviour
{
    public GameObject brickPrefab;         // The Brick prefab
    public GameObject mysteryBlockPrefab;  // The Mystery Block prefab
    public int minBlocks = 3;              // Minimum number of blocks in a set (alternating)
    public int maxBlocks = 7;              // Maximum number of blocks in a set
    public float spacing = 1f;           // Spacing between blocks in a set
    public float minX = 0f;                // Minimum X range for placement
    public float maxX = 200f;              // Maximum X range for placement
    public float minSpacingBetweenSets = 10f;  // Minimum X spacing between sets
    public float startY = 4f;              // Fixed Y position

    private void Start()
    {
        float currentX = minX; // Start placing at minX

        for (int setIndex = 0; setIndex < 8; setIndex++)
        {
            // Randomize the number of blocks in this set (alternating between brick and mystery block)
            int numberOfBlocks = Random.Range(minBlocks, maxBlocks + 1);

            // Randomize the starting X position of the current set
            currentX += Random.Range(minSpacingBetweenSets, minSpacingBetweenSets + 50f);

            // Ensure we don't go beyond maxX for the set's starting position
            if (currentX > maxX)
                break;

            Vector3 position = new Vector3(currentX, startY, 0); // Starting position for this set

            // Spawn alternating blocks in this set
            for (int i = 0; i < numberOfBlocks; i++)
            {
                GameObject blockToInstantiate = (i % 2 == 0) ? brickPrefab : mysteryBlockPrefab;
                Instantiate(blockToInstantiate, position, Quaternion.identity);

                // Move the position for the next block in the set
                position.x += spacing;
            }

            // Move currentX for the next set, adding extra spacing after the current set
            currentX += numberOfBlocks * spacing;
        }
    }
}
