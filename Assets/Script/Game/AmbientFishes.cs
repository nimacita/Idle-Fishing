using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientFishes : MonoBehaviour
{

    [Header("Shallow Fishes")]
    public GameObject[] shallowFishes;
    [Range(0,10)]
    public int everyShallowChance;

    [Header("Average Fishes")]
    public GameObject[] averageFishes;
    [Range(0, 10)]
    public int everyAverageChance;

    [Header("Deep Fishes")]
    public GameObject[] deepFishes;
    [Range(0, 10)]
    public int everyDeepChance;


    void Start()
    {
        
    }

    public void SpawnRandomFishes()
    {
        ShallowSpawn();
        AverageSpawn();
        DeepSpawn();
    }

    private void ShallowSpawn()
    {
        foreach (GameObject fish in shallowFishes)
        {
            int rand = Random.Range(0, everyShallowChance);
            if (rand == 0) ActiveFish(fish); 
            else fish.SetActive(false);
        }
    }

    private void AverageSpawn()
    {
        foreach (GameObject fish in averageFishes)
        {
            int rand = Random.Range(0, everyAverageChance);
            if (rand == 0) ActiveFish(fish);
            else fish.SetActive(false);
        }
    }

    private void DeepSpawn()
    {
        foreach (GameObject fish in deepFishes)
        {
            int rand = Random.Range(0, everyDeepChance);
            if (rand == 0) ActiveFish(fish);
            else fish.SetActive(false);
        }
    }

    private void ActiveFish(GameObject fish)
    {
        fish.GetComponent<FishController>().SetStartStats();
        fish.SetActive(true);
    }
}
