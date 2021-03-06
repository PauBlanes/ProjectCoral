﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyManager : MonoBehaviour {

    public GameObject energyPrefab;
    public float xRange; //màxima distancia des del centre de la camera per on pot apareixer

    public float averageSpawnTime;
    public float spawnTimeRange; //per controlar si hi ha molta variabilitat de temps de spawn.

    public int energyCounter;
    public Text energyText;

    // Use this for initialization
    void Start () {
        energyText.text = ""+energyCounter;
        StartCoroutine(WaitAndSpawn());
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    IEnumerator WaitAndSpawn()
    {
        yield return new WaitForSeconds(5); //un wait inicial pq es pogui llegir la info primer
        while (true)
        {
            if (!Tutorial.showingInfo)
            {
                yield return new WaitForSeconds(Random.Range(averageSpawnTime - spawnTimeRange, averageSpawnTime + spawnTimeRange));
                SpawnEnergy();
            }
            else
                yield return null;
        }
    }

    public void SpawnEnergy()
    {
        Instantiate(energyPrefab, new Vector3(Random.Range(transform.position.x - xRange, transform.position.x + xRange),
            /*transform.position.y - Camera.main.orthographicSize - energyPrefab.GetComponent<SpriteRenderer>().size.y/2*/-12, 0),
            Quaternion.identity); //cuidao que aixo funciona pq el script el té la camera
    }

    public void UpdateCounter (int sum)
    {
        energyCounter += sum;
        energyText.text = "" + energyCounter;
    }

    public int GetCount ()
    {
        return energyCounter;
    }
    public void InvestigationReward(GameObject info, int reward)
    {        
        UpdateCounter(reward);
        Time.timeScale = 1;
        Destroy(info);
    }
}
