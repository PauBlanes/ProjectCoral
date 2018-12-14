using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bleaching : MonoBehaviour {

    private GameObject bleachedSprite;    
    
    private bool cured;
    public bool Cured
    {
        get { return cured; }
        set { cured = value; }
    }
    private bool isBleaching;
    public bool IsBleaching
    {
        get { return isBleaching; }
        set { isBleaching = value; }
    }

    private bool bleached;
    private bool blinking;

	// Use this for initialization
	void Start () {
        bleachedSprite = transform.GetChild(0).gameObject;        
	}
	
	// Update is called once per frame
	void Update () {
		if (bleached && cured) //si el curen mes tard
        {
            Heal(15);
        }
	}

    public void Heal(int howMuch)
    {
        //Deixar el color inicial
        Color newColor = bleachedSprite.GetComponent<SpriteRenderer>().color;
        newColor.a = 0;
        bleachedSprite.GetComponent<SpriteRenderer>().color = newColor;

        //Sumar punts al sistema, i sumem bastant pq ho ha curat ràpid
        Camera.main.GetComponent<EcosystemManager>().ecosystemEvolution += howMuch;

        bleached = false;
        isBleaching = false; //ja es pot tornar a activar
    }

    public void StartBleaching ()
    {
        isBleaching = true;
        StartCoroutine(Bleach());
    }    
    IEnumerator Bleach()
    {
        blinking = true;
        StartCoroutine(BleachBlink(0.5f));
        yield return new WaitForSeconds(10);
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

        //Mirar si hem de deixar el estat blanc o el normal
        if (cured) //per si ho curen mentre parpalleja
        {
            Heal(25);            
        }
        else
        {
            bleached = true;

            //Deixar el color blanc
            Color newColor = bleachedSprite.GetComponent<SpriteRenderer>().color;
            newColor.a = 1;
            bleachedSprite.GetComponent<SpriteRenderer>().color = newColor;

            //restar punts al sistema
            Camera.main.GetComponent<EcosystemManager>().ecosystemEvolution -= 10;
        }
    }
}
