using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EcosystemManager : MonoBehaviour {

    [System.Serializable]
    public struct Popup
    {
        public GameObject popup;
        private bool alreadyShown;
        public bool AlreadyShown
        {
            get { return alreadyShown; }
            set { alreadyShown = value; }
        }
    }

    private int ecosystemEvolution; //per controlar cada quan passa algo. Aquest es el numero que es necessita pq passi algo
    private int lastCheckpoint;
    public int breakPoint = 5; //quant ha d'augmentar o disminuir pq passi algo
    public int maxHealth;

    int minThreatTime = 9;
    int maxThreadTime = 20;

    public List<GameObject> activeCorals;
    List<GameObject> hiddenCorals = new List<GameObject>();

    public GameObject[] threats;
    private float timeToWait;

    //per mostrar quantes coses hi ha investigades
    public Text investigationText;

    //Per mostrar barra de vida
    public Image healthBar;
    public Popup[] positivePopups;
    public Popup[] negativePopups;

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

        //Actualitzar UI inicialment
        ecosystemEvolution = maxHealth / 2;
        lastCheckpoint = ecosystemEvolution;
        UpdateSystemHealth(0);    
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
                timeToWait = Random.Range(minThreatTime, maxThreadTime);
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
                if (ecosystemEvolution - lastCheckpoint > breakPoint) //si hem creuat un limit
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
                    }

                    //Mostrar popup si cal
                    if (ecosystemEvolution >= ((float)maxHealth/2 + (float)maxHealth/4) && !positivePopups[0].AlreadyShown)
                    {
                        maxThreadTime = 16; //fem més difícil
                        positivePopups[0].AlreadyShown = true; //no em deixa dins de la funció
                        positivePopups[0].popup.SetActive(true);
                        Time.timeScale = 0;
                        //ShowPopup(positivePopups[0], true);
                    }
                        
                    if (ecosystemEvolution >= maxHealth && !positivePopups[1].AlreadyShown)
                    {
                        maxThreadTime = 12; //fem més difícil
                        positivePopups[1].AlreadyShown = true; //no em deixa dins de la funció
                        positivePopups[1].popup.SetActive(true);
                        Time.timeScale = 0;
                        //ShowPopup(positivePopups[1], true);
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
                    }

                    //Mostrar popup si cal
                    if (ecosystemEvolution <= ((float)maxHealth/2 - (float)maxHealth/4) && !negativePopups[0].AlreadyShown)
                    {
                        negativePopups[0].AlreadyShown = true;
                        negativePopups[0].popup.SetActive(true);
                        Time.timeScale = 0;
                        //ShowPopup(negativePopups[0], true);
                    }                        
                    if (ecosystemEvolution <= 0 && !negativePopups[1].AlreadyShown)
                    {
                        negativePopups[1].AlreadyShown = true;
                        negativePopups[1].popup.SetActive(true);
                        Time.timeScale = 0;
                        ShowPopup(negativePopups[1], true);
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

    public void UpdateSystemHealth(int value)
    {
        //Actualitzar dades
        ecosystemEvolution += value;
        float uiHealth = Mathf.Clamp(ecosystemEvolution, 0, maxHealth);       

        //Pintar UI
        healthBar.fillAmount = uiHealth / maxHealth;        
    }

    public void CloseUIElement (GameObject e)
    {
        e.SetActive(false);
        Time.timeScale = 1;
    }
    void ShowPopup (Popup p, bool state)
    {
        p.popup.SetActive(state);        
        Time.timeScale = 0;
    }
    
}
