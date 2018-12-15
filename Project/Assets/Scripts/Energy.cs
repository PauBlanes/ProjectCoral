using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy : MonoBehaviour {

    public float speed;
    public float mapYLimit;

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
        Camera.main.GetComponent<EnergyManager>().UpdateCounter(1);
        Destroy(gameObject);
    }
}
