using UnityEngine;

public class Flag : MonoBehaviour
{
    public GameObject[] items; // Array to hold the two objects

    void Start()
    {
        // Check if the array has at least two elements
        if (items.Length >= 2)
        {
            // Define positions for the objects
            Vector3 position1 = new Vector3(270f, 1f, 0f); // Position for object 1
            Vector3 position2 = new Vector3(280f, 1f, 0f); // Position for object 2

            // Instantiate the objects at the defined positions
            Instantiate(items[0], position1, Quaternion.identity);
            Instantiate(items[1], position2, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Please assign at least two objects to the items array.");
        }
    }
}
