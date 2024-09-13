using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy2pcg : MonoBehaviour
{
    public GameObject items;
    public int numberOfInstances = 5;
    public float spacing = 10f; // Adjust the spacing between instances

    void Start()
    {
        Vector3 position = transform.position; // Starting position

        for (int i = 0; i < numberOfInstances; i++)
        {
            float randomX = Random.Range(15f, 250f); // Generate a random X value within the range 15 to 250
            float randomY = Random.Range(-2f, 4f); // Generate a random Y value
            
            position.x = randomX;
            position.y = randomY;

            Instantiate(items, position, Quaternion.identity);
        }
    }
}
