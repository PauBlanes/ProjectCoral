using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {

    public GameObject[] infoImages;
    public GameObject[] skillButtons;

    public static bool showingInfo;
    public static int acceptedThreads = 0; //al principi no deixem fer spawn a cap amenaça

	// Use this for initialization
	void Start () {
        //Amagar tot
        foreach (GameObject i in infoImages)
        {
            i.SetActive(false);
        }
        foreach (GameObject button in skillButtons)
        {
            button.SetActive(false);
        }

        //Començar routina
        StartCoroutine(TutorialCoroutine());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator TutorialCoroutine()
    {
        //Mostrem imatge amb info general
        infoImages[0].SetActive(true);
        showingInfo = true;
        Time.timeScale = 0;
        yield return new WaitUntil(() => !showingInfo);

        //amagar imatge i esperar
        infoImages[0].SetActive(false);
        yield return new WaitForSeconds(3f);

        //Mostrar primera info de botó i activar el primer botó
        infoImages[1].SetActive(true);               
        showingInfo = true;
        Time.timeScale = 0;
        yield return new WaitUntil(() => !showingInfo);

        //amagar imatge i esperar
        infoImages[1].SetActive(false);
        yield return new WaitForSeconds(20f);

        //Mostrar segona info de botó i activar el primer botó
        infoImages[2].SetActive(true);        
        acceptedThreads++;
        showingInfo = true;
        Time.timeScale = 0;
        yield return new WaitUntil(() => !showingInfo);

        //amagar imatge i esperar
        infoImages[2].SetActive(false);
        yield return new WaitForSeconds(20f);

        //Mostrar tercera info de botó i activar el primer botó
        infoImages[3].SetActive(true);             
        acceptedThreads++;
        showingInfo = true;
        Time.timeScale = 0;
        yield return new WaitUntil(() => !showingInfo);

        //amagar imatge i esperar
        infoImages[3].SetActive(false);
        yield return new WaitForSeconds(20f);

        //Mostrar quarta info de botó i activar el primer botó
        infoImages[4].SetActive(true);       
        acceptedThreads++;
        showingInfo = true;
        Time.timeScale = 0;
        yield return new WaitUntil(() => !showingInfo);

        //amagar imatge i esperar
        infoImages[4].SetActive(false);
        yield return new WaitForSeconds(20f);

        //Mostrar cinquena info de botó i activar el primer botó
        infoImages[5].SetActive(true);        
        
        acceptedThreads++;
        showingInfo = true;
        Time.timeScale = 0;
        yield return new WaitUntil(() => !showingInfo);

        //amagar imatge i esperar
        infoImages[5].SetActive(false);
        yield return new WaitForSeconds(20f);
    }

    IEnumerator WaitAndSpawnThreat(int i)
    {
        yield return new WaitForSeconds(2);
        if (i < 3)
            Camera.main.GetComponent<EcosystemManager>().SpawnConcreteThread(i);
        else
            Camera.main.GetComponent<EcosystemManager>().RepairThreat();

    }

    public void Exit(int buttonIndex)
    {
        showingInfo = false;
        Time.timeScale = 1;

        if (buttonIndex >= 0)
            skillButtons[buttonIndex].SetActive(true);

        if (buttonIndex > 0)    //no es de investigació
        {
            StartCoroutine(WaitAndSpawnThreat(buttonIndex-1));
        }
    }
}
