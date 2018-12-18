using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy : MonoBehaviour {

    public float speed;
    public float mapYLimit;
    public AudioClip bubbleDestroy;
    
    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {

        //Move
        transform.position += (new Vector3(0, 1, 0) * speed * Time.deltaTime);

        //Destroy if out of map
        if (transform.position.y - GetComponent<SpriteRenderer>().size.y / 2 > mapYLimit)
        {
            Destroy(gameObject);
        }       
    }
    
    //Is clicked?
    private void OnMouseDown()
    {
        
        GetComponent<AudioSource>().Play();
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;//per si a cas que no detecti el click

        Camera.main.GetComponent<EnergyManager>().UpdateCounter(1);
                
        Destroy(gameObject, bubbleDestroy.length);
        
    }
}
