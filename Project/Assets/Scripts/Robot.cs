using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour {
    
    enum States { walking, working};
    public enum RobotType {investigation, repair, clean, netCutter }

    public Transform target;
    public float speed;

    public float workTime;
    private float waitCounter;

    States state = States.walking;

    public RobotType rT;

    public int cost;

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
                state = States.working;
                waitCounter = workTime;
            }
                
        }
        else if (state == States.working)
        {
            //El temps que dura el treball
            if(waitCounter > 0)
            {
                waitCounter -= Time.deltaTime;
                //Coses que fer mentre treballes
            }
            else //Acció que fer quan acabes
            {
                if(rT == RobotType.investigation)
                {
                    //show info
                }

                //destroy robot
                Destroy(gameObject);
            }
        }        
    }
}
