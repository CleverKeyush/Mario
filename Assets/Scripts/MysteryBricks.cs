using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryBricks : MonoBehaviour
{
    public GameObject items;
    public int numberOfInstances = 40;
    public float spacing = 30f; // Adjust the spacing between instances

    void Start()
    {
        Vector3 position = transform.position; // Starting position

        for (int i = 0; i < numberOfInstances; i++)
        {
            float randomY = Random.Range(5f, 9f); // Generate a random Y value
            
            Instantiate(items, position, Quaternion.identity);
            position.x += spacing;
            position.y = randomY;
        }
    }
}