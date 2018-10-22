using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyManager : MonoBehaviour {

    public GameObject energyPrefab;
    public float xRange;

    public float averageSpawnTime;
    public float spawnTimeRange;

    public List<GameObject> allEnergy;

    public float speed;
    public float mapYLimit;

    // Use this for initialization
    void Start () {
        StartCoroutine(WaitAndSpawn());
    }
	
	// Update is called once per frame
	void Update () {
        //Move Energy
		foreach (GameObject e in allEnergy)
        {
            e.transform.position += (new Vector3(0, 1, 0) * speed * Time.deltaTime);
        }

        //Kill Energy if out of map limits
        for (int i = 0; i < allEnergy.Count; i++)
        {
            if (allEnergy[i].transform.position.y - allEnergy[i].GetComponent<SpriteRenderer>().size.y / 2 > mapYLimit)
            {
                Destroy(allEnergy[i]);
                allEnergy.RemoveAt(i);
            }
        }
    }

    IEnumerator WaitAndSpawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(averageSpawnTime - spawnTimeRange, averageSpawnTime + spawnTimeRange));
            SpawnEnergy();
        }
    }

    public void SpawnEnergy()
    {
        GameObject temp = Instantiate(energyPrefab, new Vector3(Random.Range(transform.position.x - xRange, transform.position.x + xRange),
            transform.position.y - Camera.main.orthographicSize - energyPrefab.GetComponent<SpriteRenderer>().size.y/2, 0),
            Quaternion.identity); //cuidao que aixo funciona pq el script el té la camera

        allEnergy.Add(temp);
    }
}
