using UnityEngine;

public class AltMysBri : MonoBehaviour
{
    public GameObject brickPrefab;         // The Brick prefab
    public GameObject mysteryBlockPrefab;  // The Mystery Block prefab
    public int minBlocks = 3;              // Minimum number of blocks in a set (alternating)
    public int maxBlocks = 7;              // Maximum number of blocks in a set
    public float spacing = 1f;             // Spacing between blocks in a set
    public float minX = 0f;                // Minimum X range for placement
    public float maxX = 250f;              // Maximum X range for placement
    public float minSpacingBetweenSets = 10f;  // Minimum X spacing between sets
    public float startY = 4f;              // Fixed Y position for the first layer
    public float secondLayerY = 8f;        // Y position for the second layer
    public float xOffsetSecondLayer = 2f;  // X offset for the second layer
    public int maxMysteryBlocksInSecondLayer = 2; // Max number of mystery blocks in the second layer

    private void Start()
    {
        float currentX = minX; // Start placing at minX

        for (int setIndex = 0; setIndex < 15; setIndex++)
        {
            // Randomize the number of blocks in this set (alternating between brick and mystery block)
            int numberOfBlocks = Random.Range(minBlocks, maxBlocks + 1);

            // Randomize the starting X position of the current set
            currentX += Random.Range(minSpacingBetweenSets, minSpacingBetweenSets + 10f);

            // Ensure we don't go beyond maxX for the set's starting position
            if (currentX > maxX)
                break;

            Vector3 position = new Vector3(currentX, startY, 0); // Starting position for this set

            // Track X positions for second layer
            float[] blockPositions = new float[numberOfBlocks];

            // Spawn alternating blocks in this set
            for (int i = 0; i < numberOfBlocks; i++)
            {
                GameObject blockToInstantiate = (i % 2 == 0) ? brickPrefab : mysteryBlockPrefab;
                Instantiate(blockToInstantiate, position, Quaternion.identity);

                // Store X position for second layer
                blockPositions[i] = position.x;

                // Move the position for the next block in the set
                position.x += spacing;
            }

            // Move currentX for the next set, adding extra spacing after the current set
            currentX += numberOfBlocks * spacing;

            // Spawn the second layer of mystery blocks with a limit of 2
            SpawnSecondLayer(blockPositions);
        }
    }

    void SpawnSecondLayer(float[] blockPositions)
    {
        // Randomly select 1 or 2 positions to place mystery blocks
        int numberOfMysteryBlocks = Random.Range(1, maxMysteryBlocksInSecondLayer + 1);

        for (int i = 0; i < numberOfMysteryBlocks; i++)
        {
            // Randomly pick one of the X positions from the first layer
            int randomIndex = Random.Range(0, blockPositions.Length);
            float xPos = blockPositions[randomIndex];

            // Create a new position for the second layer
            Vector3 secondLayerPosition = new Vector3(xPos + xOffsetSecondLayer, secondLayerY, 0);

            // Spawn a single mystery block at the new position
            Instantiate(mysteryBlockPrefab, secondLayerPosition, Quaternion.identity);
        }
    }
}