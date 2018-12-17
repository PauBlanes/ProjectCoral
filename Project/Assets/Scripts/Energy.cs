using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy : MonoBehaviour {

    public float speed;
    public float mapYLimit;
    public AudioClip bubbleDestroy;
    public AudioSource BDestroy;
    private bool clicked = false;
    // Use this for initialization
    void Start () {
        BDestroy.clip = bubbleDestroy;
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
        //Debug.Log(clicked);
        if (clicked==true) {
            Debug.Log(clicked);
            BDestroy.Play();
            clicked = false;
        }
    }
    
    //Is clicked?
    private void OnMouseDown()
    {
        
        Camera.main.GetComponent<EnergyManager>().UpdateCounter(1);
        clicked = true;
        //Debug.Log(clicked);
        if (clicked == true)
        {
            //Debug.Log(clicked);
            BDestroy.Play();
            clicked = false;
        }
        
        Destroy(gameObject);
        
    }
}
