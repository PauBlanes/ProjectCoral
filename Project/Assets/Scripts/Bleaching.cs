﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bleaching : MonoBehaviour {

    private GameObject bleachedSprite;    
    
    private bool blinking;

    private readonly int noBleachLevel = 100; //a quin numero hem d'arrivar per considerarlo curat
    private float currentBleachLevel = 100;
    private int secondsToFullyHeal = 3;

	// Use this for initialization
	void Start () {
        bleachedSprite = transform.GetChild(0).gameObject;        
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public bool Healing()
    {
        //Anem curant
        currentBleachLevel += Time.deltaTime * (noBleachLevel/secondsToFullyHeal);
        if (!blinking) //si no esta parpellejant fem que es vagi traient el blanc
        {
            Color newColor = bleachedSprite.GetComponent<SpriteRenderer>().color;
            newColor.a = 1 - (currentBleachLevel/100);
            bleachedSprite.GetComponent<SpriteRenderer>().color = newColor;
        }

        //S'ha curat?
        if (currentBleachLevel >= noBleachLevel)
        {
            if (blinking)
            {
                Healed(25);
                blinking = false;
            }
                
            else
                Healed(15);

            return true;
        }

        return false;
    }

    void Healed(int reward)
    {
        //Deixar el color inicial
        Color newColor = bleachedSprite.GetComponent<SpriteRenderer>().color;
        newColor.a = 0;
        bleachedSprite.GetComponent<SpriteRenderer>().color = newColor;

        //Sumar punts al sistema, i sumem bastant pq ho ha curat ràpid
        Camera.main.GetComponent<EcosystemManager>().UpdateSystemHealth(reward);        
        
        Robot.bleachedCorals.Remove(gameObject);
    }

    public void StartBleaching ()
    {
        Robot.bleachedCorals.Add(gameObject);
        currentBleachLevel = 50; //mentre parpellegi es 50 encara
        StartCoroutine(Bleach());
    }    

    IEnumerator Bleach()
    {
        blinking = true;
        StartCoroutine(BleachBlink(0.3f));
        yield return new WaitForSeconds(5);
        blinking = false;        

    }

    IEnumerator BleachBlink (float aTime)
    {
        //Parpallejar
        while (blinking)
        {
            //Fade in
            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
            {
                Color newColor = bleachedSprite.GetComponent<SpriteRenderer>().color;
                newColor.a = Mathf.Lerp(0.0f, 1.0f, t);
                bleachedSprite.GetComponent<SpriteRenderer>().color = newColor;
                yield return null;
            }

            yield return new WaitForSeconds(0.5f);

            //Fade out
            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / (aTime * 2))
            {
                Color newColor = bleachedSprite.GetComponent<SpriteRenderer>().color;
                newColor.a = Mathf.Lerp(1.0f, 0.0f, t);
                bleachedSprite.GetComponent<SpriteRenderer>().color = newColor;
                yield return null;
            }
        }        

        if (currentBleachLevel < noBleachLevel) //si s'ha passat el temps de parpelleig i no l'hem curat
        {            
            currentBleachLevel = 0; //abans tenia 50 ara ja 0
            StartCoroutine(Starving());

            //Deixar el color blanc
            Color newColor = bleachedSprite.GetComponent<SpriteRenderer>().color;
            newColor.a = 1;
            bleachedSprite.GetComponent<SpriteRenderer>().color = newColor;

            //restar punts al sistema
            Camera.main.GetComponent<EcosystemManager>().UpdateSystemHealth(-10);            
        }        
    }

    IEnumerator Starving()
    {
        while(currentBleachLevel <= 0)
        {
           GetComponent<HealthSystem>().UpdateHealth(-5); 
           yield return new WaitForSeconds(5);
        }
    }
}
