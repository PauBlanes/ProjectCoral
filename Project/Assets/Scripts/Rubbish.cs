using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rubbish : MonoBehaviour {
    enum States { spawn, floating};

    States state = States.spawn;
    int targetY;
	// Use this for initialization
	void Start () {
        targetY = Random.Range(7, -1);
	}
	
	// Update is called once per frame
	void Update () {
	    if(state == States.spawn)
        {
            float step = 2 * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3 (transform.position.x,targetY, transform.position.z), step);
            if(Mathf.Approximately(transform.position.y, targetY))
            {
                state = States.floating;
            }
        }        
        else if (state == States.floating)
        {
            Vector3 _newPosition = transform.position;
            _newPosition.y += Mathf.Sin(Time.time) * 0.005f;
            transform.position = _newPosition;
        }
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Contains("FISH"))
        {   
            //Si un peix el toca es debilita l'ecosistema
            Camera.main.GetComponent<EcosystemManager>().UpdateSystemHealth(-5);
            
            //debilitar el peix
            collision.gameObject.GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 0.3f);
            if (collision.gameObject.GetComponent<SpriteRenderer>().color.a <= 0)
                Destroy(collision.gameObject);
        }
    }
}
