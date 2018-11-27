using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EcosystemManager : MonoBehaviour {

    public int ecosystemEvolution; //per controlar cada quan passa algo. Aquest es el numero que es necessita pq passi algo
    private int lastCheckpoint;
    public int breakPoint = 5; //quant ha d'augmentar o disminuir pq passi algo

    public List<GameObject> activeCorals;
    List<GameObject> hiddenCorals = new List<GameObject>();

    public GameObject[] threats;
    private float timeToWait;

	// Use this for initialization
	void Start () {
        //amaguem uns quants corals
        int coralsToHide = Random.Range((activeCorals.Count/2), (activeCorals.Count/2) +3);
        while(hiddenCorals.Count < coralsToHide)
        {
            int index = Random.Range(0, activeCorals.Count);
            hiddenCorals.Add(activeCorals[index]);
            activeCorals.RemoveAt(index);
        }
        foreach (GameObject c in hiddenCorals)
        {
            c.SetActive(false);
        }
        //Setegem els peixos
        Camera.main.GetComponent<FishManager>().maxFishes = 5;

        //Spawnejar amenaces
        StartCoroutine(SpawnThreads());

        //Començar rutina de mirar levolucio del nivell
        StartCoroutine(CheckCoralReefState());
    }

    // Update is called once per frame
    void Update () {
        
    }
    
    IEnumerator SpawnThreads()
    {
        timeToWait = Random.Range(3, 10);
        yield return new WaitForSeconds(timeToWait);
        Instantiate(threats[Random.Range(0, threats.Length)], new Vector3(Random.Range(-15, 15), 15, 0), Quaternion.identity);
    }

    IEnumerator CheckCoralReefState()
    {
        while (true)
        {
            if (ecosystemEvolution - lastCheckpoint > breakPoint)
            {
                if (hiddenCorals.Count > 0)
                {
                    GameObject temp = hiddenCorals[Random.Range(0, hiddenCorals.Count - 1)];
                    activeCorals.Add(temp);
                    temp.SetActive(true);
                    hiddenCorals.Remove(temp);
                }
                lastCheckpoint = ecosystemEvolution;
                if (Camera.main.GetComponent<FishManager>().maxFishes < 25)
                    Camera.main.GetComponent<FishManager>().maxFishes++;
            }
            if (ecosystemEvolution - lastCheckpoint < -breakPoint)
            {
                if (activeCorals.Count > 0)
                {
                    GameObject temp = activeCorals[Random.Range(0, activeCorals.Count - 1)];
                    hiddenCorals.Add(temp);
                    temp.SetActive(false);
                    activeCorals.Remove(temp);
                }
                lastCheckpoint = ecosystemEvolution;
                if (Camera.main.GetComponent<FishManager>().maxFishes > 0)
                    Camera.main.GetComponent<FishManager>().maxFishes--;
            }
            yield return new WaitForSeconds(1f);
        }        
    }
}
