using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTrial : MonoBehaviour
{
    public GameObject[] items;
    public int numberOfInstances = 10;
    public float spacing = 10f; // Adjust the spacing between instances

    void Start()
    {
        Vector3 position = transform.position; // Starting position

        for (int i = 0; i < numberOfInstances; i++)
        {
            float randomY = Random.Range(-2f, 4f); // Generate a random Y value
            
            Instantiate(items[1], position, Quaternion.identity);
            position.x += spacing;
            position.y = randomY;
        }
    }
}