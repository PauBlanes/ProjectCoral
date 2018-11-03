﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotInvest : MonoBehaviour {
    
    enum States { walking, investigating};


    public Transform target;
    public float speed;

    public float waitTime;
    private float waitCounter;

    States state = States.walking;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if(state == States.walking)
        {
            if (Mathf.Abs((transform.position - target.position).magnitude) > 0.01)
            {
                // The step size is equal to speed times frame time.
                float step = speed * Time.deltaTime;

                // Move our position a step closer to the target.
                transform.position = Vector2.MoveTowards(transform.position, target.position, step);
            }
            else
            {
                state = States.investigating;
                waitCounter = waitTime;
            }
                
        }
        else if (state == States.investigating)
        {
            if(waitCounter > 0)
                waitCounter -= Time.deltaTime;
            else
            {
                //show info

                //destroy robot
                Destroy(gameObject);
            }

        }

        
        
    }
}
