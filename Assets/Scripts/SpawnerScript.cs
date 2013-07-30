using UnityEngine;
using System.Collections;

public class SpawnerScript : MonoBehaviour {

    public Transform eggPrefab;
	
    private float nextEggTime = 0.0f;
    private float spawnRate = 3.1f;
 	
	void Update () {
        if (nextEggTime < Time.time)
        {
            SpawnEgg();
            nextEggTime = Time.time + spawnRate;

            //Speed up the spawnrate for the next egg
            spawnRate *= 2.2f;
            spawnRate = Mathf.Clamp(spawnRate, 0.3f, 99f);
        }
	}

    void SpawnEgg()
    {
        float addXPos = Random.Range(-13.6f, 13.6f);
        Vector3 spawnPos = transform.position + new Vector3(addXPos,0,0);
        Instantiate(eggPrefab, spawnPos, Quaternion.identity);
    }
	
}
