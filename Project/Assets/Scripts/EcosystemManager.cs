using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EcosystemManager : MonoBehaviour {

    public int ecosystemEvolution; //per controlar cada quan passa algo. Aquest es el numero que es necessita pq passi algo
    private int lastCheckpoint;
    public int breakPoint = 5; //quant ha d'augmentar o disminuir pq passi algo

    public List<GameObject> activeCorals;
    List<GameObject> hiddenCorals = new List<GameObject>();

    public GameObject[] threats;
    private float timeToWait;

    //per mostrar quantes coses hi ha investigades
    public Text investigationText;

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

        UpdateInvestigationUI();

    }

    // Update is called once per frame
    void Update () {
        
    }
    
    public void SpawnConcreteThread(int i)
    {
        Instantiate(threats[i], new Vector3(Random.Range(-12, 12), 15, 0), Quaternion.identity);
    }

    IEnumerator SpawnThreads()
    {        
        while (true)
        {            
            if (!Tutorial.showingInfo && Tutorial.acceptedThreads - 1 >= 0)
            {
                timeToWait = Random.Range(10, 25);
                yield return new WaitForSeconds(timeToWait);

                int threadIndex = Random.Range(0, Tutorial.acceptedThreads - 1);                
                if (threadIndex < 3) //si no es el de reparar
                    Instantiate(threats[threadIndex], new Vector3(Random.Range(-12, 12), 15, 0), Quaternion.identity);
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

                    //tots els corals guanyen una mica de vida
                    foreach (GameObject coral in activeCorals)
                    {
                        coral.GetComponent<HealthSystem>().UpdateHealth(20);
                    }

                    //apareixen nous corals i peixos
                    if (hiddenCorals.Count > 0)
                    {
                        GameObject temp = hiddenCorals[Random.Range(0, hiddenCorals.Count - 1)];
                        activeCorals.Add(temp);

                        if(!Robot.alreadyInvestigated.Contains(temp))
                            Robot.investigableObjects.Add(temp);

                        temp.SetActive(true);
                        hiddenCorals.Remove(temp);
                    }
                    lastCheckpoint = ecosystemEvolution;
                    if (Camera.main.GetComponent<FishManager>().maxFishes < 25)
                    {
                        Camera.main.GetComponent<FishManager>().maxFishes++;
                        Camera.main.GetComponent<FishManager>().maxFishType++;
                        //ensenyar popup
                    }
                }
                if (ecosystemEvolution - lastCheckpoint < -breakPoint)
                {

                    //tots els corals perden una mica de vida
                    foreach (GameObject coral in activeCorals)
                    {
                        coral.GetComponent<HealthSystem>().UpdateHealth(-10);
                    }

                    //desapareixen corals
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
                        Camera.main.GetComponent<FishManager>().maxFishType--;
                        //ensenyar popup
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
    
    public void RepairThreat()
    {
        List<GameObject> coralsToBleach = new List<GameObject>(); //corals possibles per a que es blanquegin
        foreach (GameObject coral in activeCorals)
        {
            if (coral.GetComponent<Bleaching>() != null && !Robot.bleachedCorals.Contains(coral)) //si no és planta i no està ja blanquejat
            {
                coralsToBleach.Add(coral);
            }
        }
        int numOfCoralsToBleach = Mathf.Clamp(Random.Range(2, 4), 0, coralsToBleach.Count);
        
        for (int i = 0; i < numOfCoralsToBleach; i++)
        {
            GameObject t = coralsToBleach[Random.Range(0, coralsToBleach.Count)];
            t.GetComponent<Bleaching>().StartBleaching();
            coralsToBleach.Remove(t);
        }        
    }

    public void UpdateInvestigationUI()
    {
        investigationText.text = Robot.alreadyInvestigated.Count + "/43";
    }
}
