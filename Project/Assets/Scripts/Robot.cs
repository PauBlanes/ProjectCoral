using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour {
    
    enum States { walking, working, wander};
    public enum RobotType {Investigation, Repair, Rubbish, NetCutter, Petrol }

    private Transform target;
    public float speed;

    public float workTime;
    private float waitCounter;

    States state = States.walking;

    public float workPerSecond; //quants destrueixes per segon

    public RobotType rT;

    float randX, randY;

    // Use this for initialization
    void Start()
    {
        waitCounter = workTime;

        //Cojemos el más cercano como target
        target = GetCloserObject(GameObject.FindGameObjectsWithTag(rT.ToString()));
        if (target == null) //si ja no hi ha més
        {
            state = States.wander;
            StartCoroutine(ChoseDirection());
        }

    }
	
	// Update is called once per frame
	void Update () {

        if (waitCounter > 0)
        {
            //el teu temps de vida
            waitCounter -= Time.deltaTime;

            //anar cap a un objecte
            if (state == States.walking)
            {
                //Per si tenim més d'un robot alhora i ens el roben
                if (target == null)
                {
                    state = States.wander;
                }
                else
                {
                    //Mirar al target
                    Vector3 vectorToTarget = target.transform.position - transform.position;
                    float angle = Mathf.Atan2(-vectorToTarget.y, -vectorToTarget.x) * Mathf.Rad2Deg;
                    Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
                    transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 2);

                    //Moures
                    if ((transform.position - target.position).magnitude < 0.1f)
                    {
                        state = States.working;
                    }
                    else
                    {
                        // The step size is equal to speed times frame time.
                        float step = speed * Time.deltaTime;

                        // Move our position a step closer to the target.
                        transform.position = Vector2.MoveTowards(transform.position, target.position, step);
                    }
                }                        

            } //treballar
            else if (state == States.working)
            {
                //manternirse enganxat
                transform.position = target.position; //per si encara estava moventse l'enemic quan l'atrapem que el continui seguint

                //Quant triga a cargar-se l'enemic
                Color col = target.GetComponent<SpriteRenderer>().color;
                col.a -= Time.deltaTime*workPerSecond;
                target.GetComponent<SpriteRenderer>().color = col;
                if (col.a <= 0)
                {
                    Camera.main.GetComponent<EcosystemManager>().ecosystemEvolution += 10;
                    Destroy(target.gameObject);
                    target = GetCloserObject(GameObject.FindGameObjectsWithTag(rT.ToString()));
                    if (target == null) //si ja no hi ha més
                    {                        
                        state = States.wander;                        
                        StartCoroutine(ChoseDirection());
                    }
                    else
                        state = States.walking;
                }
            }
            else if (state == States.wander)
            {

             //if its moving and didn't move too much            
             transform.position += new Vector3(randX, randY, 0).normalized/2* speed * Time.deltaTime;
                    

            }
        }
        else //Acció que fer quan acabes
        {
            if (rT == RobotType.Investigation)
            {
                //show info
            }

            //destroy robot
            Destroy(gameObject);
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

    IEnumerator ChoseDirection ()
    {
        
        while (state == States.wander)
        {
            randX = Random.Range(-1.0f, 1.0f);
            if (transform.position.x > 15 || transform.position.x < -15)
                randX *= -1;
            randY = Random.Range(-1.0f, 1.0f);
            if (transform.position.y > 7 || transform.position.y < 7)
                randY *= -1;

            yield return new WaitForSeconds(1f);

            //Provar si hi ha algun nou
            target = GetCloserObject(GameObject.FindGameObjectsWithTag(rT.ToString()));
            if (target != null) 
            {
                state = States.walking;               
            }
        }
        
    }
}
