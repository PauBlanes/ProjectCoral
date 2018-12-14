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

        //Afegim els corals actius al array del robot de investigacio
        foreach (GameObject activeCoral in activeCorals)
        {
            Robot.investigableObjects.Add(activeCoral);
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
        while (true)
        {            
            if (!Tutorial.showingInfo && Tutorial.acceptedThreads - 1 >= 0)
            {
                timeToWait = Random.Range(20, 20);
                yield return new WaitForSeconds(timeToWait);

                int threadIndex = Random.Range(0, Tutorial.acceptedThreads - 1);
                if (threadIndex < 3) //si no es el de reparar
                    Instantiate(threats[threadIndex], new Vector3(Random.Range(-15, 15), 15, 0), Quaternion.identity);
                else
                    RepairThreat();
            }
            else
                yield return null;
        }        
    }

    IEnumerator CheckCoralReefState()
    {
        while (true)
        {
            if (!Tutorial.showingInfo) // no fer això mentre estem mostran info del tutorial
            {
                if (ecosystemEvolution - lastCheckpoint > breakPoint)
                {
                    if (hiddenCorals.Count > 0)
                    {
                        GameObject temp = hiddenCorals[Random.Range(0, hiddenCorals.Count - 1)];
                        activeCorals.Add(temp);
                        Robot.investigableObjects.Add(temp);
                        temp.SetActive(true);
                        hiddenCorals.Remove(temp);
                    }
                    lastCheckpoint = ecosystemEvolution;
                    if (Camera.main.GetComponent<FishManager>().maxFishes < 25)
                    {
                        Camera.main.GetComponent<FishManager>().maxFishes++;
                        //Sumar maxFishType
                    }
                }
                if (ecosystemEvolution - lastCheckpoint < -breakPoint)
                {
                    if (activeCorals.Count > 0)
                    {
                        GameObject temp = activeCorals[Random.Range(0, activeCorals.Count - 1)];
                        hiddenCorals.Add(temp);
                        temp.SetActive(false);
                        activeCorals.Remove(temp);
                        Robot.investigableObjects.Remove(temp);
                    }
                    lastCheckpoint = ecosystemEvolution;
                    if (Camera.main.GetComponent<FishManager>().maxFishes > 0)
                    {
                        Camera.main.GetComponent<FishManager>().maxFishes--;
                        //sumar maxFishType
                    }
                }
                yield return new WaitForSeconds(1f);
            }
            else
                yield return null;

        }        
    }
    
    public void HideCoral (GameObject coral)
    {
        hiddenCorals.Add(coral);
        coral.SetActive(false);
        activeCorals.Remove(coral);
    }
    
    void RepairThreat()
    {
        int numOfCoralsToBleach = Mathf.Clamp(Random.Range(3, 8), 3, activeCorals.Count);
        int coralsBleaching = 0;

        while (coralsBleaching < numOfCoralsToBleach)
        {
            GameObject t = activeCorals[Random.Range(0, activeCorals.Count)];
            if (t.GetComponent<Bleaching>() != null && !t.GetComponent<Bleaching>().IsBleaching) //si no es planta i no l'hem activat ja
            {
                t.GetComponent<Bleaching>().StartBleaching();
            }
        }
    }
}
