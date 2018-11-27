using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour {
    
    enum States { walking, working};
    public enum RobotType {investigation, repair, clean, netCutter, petrol }

    private Transform target;
    public float speed;

    public float workTime;
    private float waitCounter;

    States state = States.walking;

    public RobotType rT;

	// Use this for initialization
	void Start () {
        switch (rT)
        {
            case RobotType.investigation:
                break;
            case RobotType.repair:
                break;
            case RobotType.clean:
                break;
            case RobotType.netCutter:
                break;
            case RobotType.petrol:
                target = GetCloserObject(GameObject.FindGameObjectsWithTag("Petrol"));
                break;
            default:
                break;
        }
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

    Transform GetCloserObject(GameObject[] objs)
    {
        float maxDist = 0;
        Transform result = null;

        foreach (GameObject o in objs)
        {
            float distance = (o.transform.position - transform.position).magnitude;
            if (distance > maxDist)
                result = o.transform;
        }
        return result;
    }
}
